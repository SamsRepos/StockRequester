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

        public static bool UserHasAccess(ApplicationUser? user, Company? company)
        {
            if(user is null || company is null)
            {
                return false;
            }

            if(user.CompanyId == 0 || company.Id == 0)
            {
                return false;
            }

            return (user.CompanyId == company.Id);
        }

        public static bool UserHasAccess(ApplicationUser? user, Location? location)
        {
            if (user is null || location is null)
            {
                return false;
            }

            if (user.CompanyId == 0 || location.CompanyId == 0)
            {
                return false;
            }

            return (user.CompanyId == location.CompanyId);
        }

        public static bool UserHasAccess(ApplicationUser? user, TransferRequest? transferRequest)
        {
            if (user is null || transferRequest is null)
            {
                return false;
            }

            if (user.CompanyId == 0 || transferRequest.CompanyId == 0)
            {
                return false;
            }

            return (user.CompanyId == transferRequest.CompanyId);
        }
    }
}
