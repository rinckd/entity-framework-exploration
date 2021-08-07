using System;
using Microsoft.AspNetCore.Http;
using SamuraiApp.Data;
using SamuraiApp.Services.CheckoutServices;

namespace SamuraiApp.Services
{
    public class UserIdService : IUserIdService
    {
        private readonly IHttpContextAccessor _httpAccessor;            //#A

        public UserIdService(IHttpContextAccessor httpAccessor)         //#A
        {                                                               //#A
            _httpAccessor = httpAccessor;                               //#A
        }                                                               //#A

        public Guid GetUserId()
        {
            var httpContext = _httpAccessor.HttpContext;                //#B
            if (httpContext == null)                                    //#B
                return Guid.Empty;                                      //#B

            var cookie = new BasketCookie(httpContext.Request.Cookies); //#C
            if (!cookie.Exists())                                       //#C
                return Guid.Empty;                                      //#C

            var service = new CheckoutCookieService(cookie.GetValue()); //#D
            return service.UserId;                                      //#D
        }
    }
}