using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _context;
        public UserService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string GetUserName()
        {
            return _context.HttpContext.User?.Identity?.Name;
        }
    }
}
