using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using Week3.Model.User;

namespace Week3.API.Infrastructure
{
    public class LoginFilter : Attribute, IActionFilter
    {
        private readonly IMemoryCache memoryCache;
        public LoginFilter(IMemoryCache _memoryCache)
        {
            memoryCache = _memoryCache;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            return;
        }

        public void OnActionExecuting(ActionExecutingContext context)//, IServiceProvider serviceProvider)
        {
            //var memoryCache = context.HttpContext.RequestServices.GetService<IMemoryCache>();

            if (!memoryCache.TryGetValue("LoginUser", out UserViewModel _loginUser))
            {
                context.Result = new UnauthorizedObjectResult("Lütfen giriş yapınız");
            }
        }
    }
}
