using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Week3.Model;
using Week3.Model.User;
using Week3.Service.User;

namespace Week3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly IMemoryCache memoryCache;
        private readonly IUserService userService;

        public LoginController(IMemoryCache _memoryCache, IUserService _userService)
        {
            memoryCache = _memoryCache;
            userService = _userService;
        }

        [HttpPost]
        public General<bool> Login([FromBody] UserLoginViewModel loginUser)
        {
            General<bool> response = new() { Entity = false };
            General<UserLoginViewModel> result = userService.Login(loginUser);

            if (result.IsSuccess)
            {
                if (!memoryCache.TryGetValue("LoginUser", out UserViewModel _loginUser))
                {
                    memoryCache.Set("LoginUser", result.Entity);
                }

                response.Entity = true;
                response.IsSuccess = true;
                response.Message = "İşlem başarılı";
            }
            else
            {
                response.ExceptionMessage = "Tekrar giriş yapın";
            }

            return response;
        }
    }
}
