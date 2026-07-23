namespace EcommerceCheckout.Web.Services.Interfaces;

public interface ICartCookiesAccessor
{
    Guid? ReadToken(HttpRequest request);
    void WriteToken(HttpResponse response, Guid token, bool isHttps);
}