using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using EcommerceCheckout.Web.Models.Entities;
using EcommerceCheckout.Web.Services.Interfaces;

namespace EcommerceCheckout.Web.Services.Implementations;

public class PayPalPaymentService : IPaymentService
{
    private readonly HttpClient _httpClient;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _baseUrl;

    private async Task<string> GetAccessTokenAsync()
    {
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
        
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/v1/oauth2/token");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
        });
        
        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        return json.GetProperty("access_token").GetString()!;
    }
    
    public PayPalPaymentService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _clientId = configuration["PayPal:ClientId"]
            ?? throw new InvalidOperationException("PayPal:ClientId is not set.");
        _clientSecret = configuration["PayPal:ClientSecret"]
            ?? throw new InvalidOperationException("PayPal:ClientSecret is not set.");
        _baseUrl = configuration["PayPal:BaseUrl"]
            ?? "https://api-m.sandbox.paypal.com";
    }
    
    public PaymentProviderType Provider => PaymentProviderType.PayPal;

    public async Task<PaymentInitResult> CreatePaymentAsync(Order order, string successUrl, string cancelUrl)
    {
        var accessToken = await GetAccessTokenAsync();

        var payload = new
        {
            intent = "CAPTURE",
            purchase_units = new[]
            {
                new
                {
                    reference_id = order.OrderNumber,
                    amount = new
                    {
                        currency_code = order.Currency,
                        value = order.TotalAmount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
                    }
                }
            },
            application_context = new
            {
                return_url = successUrl,
                cancel_url = cancelUrl,
                brand_name = "EcommerceCheckout",
                user_action = "PAY_NOW"
            }
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/v2/checkout/orders");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Content = JsonContent.Create(payload);
        
        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        var paypalOrderId = json.GetProperty("id").GetString()!;
        var approveLink = json.GetProperty("links")
            .EnumerateArray()
            .First(l => l.GetProperty("rel").GetString() == "approve")
            .GetProperty("href").GetString()!;
        
        return new PaymentInitResult { RedirectUrl = approveLink, ProviderReferenceId = paypalOrderId };
    }

    public async Task<bool> ConfirmPaymentAsync(Order order, string providerReferenceId)
    {
        var accessToken = await GetAccessTokenAsync();

        using var request = new HttpRequestMessage(HttpMethod.Post,
            $"{_baseUrl}/v2/checkout/orders/{providerReferenceId}/capture");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

        using var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return false;

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        var status = json.GetProperty("status").GetString();
        return status == "COMPLETED";
    }
}