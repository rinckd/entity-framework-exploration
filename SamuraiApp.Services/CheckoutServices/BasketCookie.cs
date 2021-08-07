using Microsoft.AspNetCore.Http;

namespace SamuraiApp.Services.CheckoutServices
{
    public class BasketCookie : CookieTemplate
    {
        public const string BasketCookieName = "EfCoreInAction2-basket";

        public BasketCookie(IRequestCookieCollection cookiesIn, IResponseCookies cookiesOut = null) 
            : base(BasketCookieName, cookiesIn, cookiesOut)
        {
        }

        protected override int ExpiresInThisManyDays => 200;    //Make this last, as it holds the user id for checking orders
    }
}