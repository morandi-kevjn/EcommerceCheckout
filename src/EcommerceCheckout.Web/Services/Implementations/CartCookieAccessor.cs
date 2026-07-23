using EcommerceCheckout.Web.Services.Interfaces;

namespace EcommerceCheckout.Web.Services.Implementations;

public class CartCookieAccessor : ICartCookiesAccessor
{
    private const string CookieName = "cart_token";

    public Guid? ReadToken(HttpRequest request)
    {
        if (request.Cookies.TryGetValue(CookieName, out var raw) && Guid.TryParse(raw, out var parsed))
            return parsed;

        return null;
    }

    public void WriteToken(HttpResponse response, Guid token, bool isHttps)
    {
        response.Cookies.Append(CookieName, token.ToString(), new CookieOptions
        {
            HttpOnly = true,
            Secure = isHttps,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(30)
        });
    }
}