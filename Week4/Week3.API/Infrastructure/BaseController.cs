﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Week3.Model.User;

namespace Week3.API.Infrastructure
{
    public class BaseController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        public BaseController(IMemoryCache _memoryCache)
        {
            memoryCache = _memoryCache;
        }
        public UserViewModel CurrentUser
        {
            get
            {
                return GetCurrentUser();
            }
        }

        private UserViewModel GetCurrentUser()
        {
            var response = new UserViewModel();

            if (memoryCache.TryGetValue("LoginUser", out UserViewModel _loginUser))
            {
                response = _loginUser;
            }

            return response;
        }
    }

}
