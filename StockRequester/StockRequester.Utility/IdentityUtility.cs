using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockRequester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.Utility
{
    public static class IdentityUtility
    {
        public static ApplicationUser? CurrentApplicationUser(UserManager<IdentityUser> userManager, HttpContext httpContext)
        {
            IdentityUser? userAsIdentityUser = userManager.GetUserAsync(httpContext.User).GetAwaiter().GetResult();
            return (ApplicationUser)userAsIdentityUser;
        }
    }
}
