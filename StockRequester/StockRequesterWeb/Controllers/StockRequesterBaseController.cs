using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;
using StockRequester.Utility;

namespace StockRequesterWeb.Controllers
{
    public class StockRequesterBaseController : Controller
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly UserManager<ApplicationUser> _userManager;

        protected StockRequesterBaseController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        protected ApplicationUser? CurrentApplicationUser()
        {
            ApplicationUser? userAsIdentityUser = _userManager.GetUserAsync(HttpContext.User).GetAwaiter().GetResult();
            return userAsIdentityUser;
        }

        protected int? CurrentUserCompanyId()
        {
            ApplicationUser? applicationUser = CurrentApplicationUser();
            if (applicationUser is null) return null;
            return applicationUser.CompanyId;
        }

        protected bool CurrentUserHasAccess(Company? company)
        {
            if (User.IsInRole(SD.Role_SiteAdmin))
            {
                return true;
            }

            ApplicationUser? applicationUser = CurrentApplicationUser();

            if (applicationUser is null || company is null)
            {
                return false;
            }

            if (applicationUser.CompanyId == 0 || company.Id == 0)
            {
                return false;
            }

            return (applicationUser.CompanyId == company.Id);
        }

        protected bool CurrentUserHasAccess(Location? location)
        {
            if(User.IsInRole(SD.Role_SiteAdmin))
            {
                return true;
            }

            ApplicationUser? applicationUser = CurrentApplicationUser();

            if (applicationUser is null || location is null)
            {
                return false;
            }

            if (applicationUser.CompanyId == 0 || location.CompanyId == 0)
            {
                return false;
            }

            return (applicationUser.CompanyId == location.CompanyId);
        }

        protected bool CurrentUserHasAccess(TransferRequest? transferRequest)
        {
            if (User.IsInRole(SD.Role_SiteAdmin))
            {
                return true;
            }

            ApplicationUser? applicationUser = CurrentApplicationUser();

            if (applicationUser is null || transferRequest is null)
            {
                return false;
            }

            if (applicationUser.CompanyId == 0 || transferRequest.CompanyId == 0)
            {
                return false;
            }

            return (applicationUser.CompanyId == transferRequest.CompanyId);
        }

        protected bool CurrentUserHasAccess(InvitedEmail? invitedEmail)
        {
            if (User.IsInRole(SD.Role_SiteAdmin))
            {
                return true;
            }

            ApplicationUser? applicationUser = CurrentApplicationUser();

            if (applicationUser is null || invitedEmail is null)
            {
                return false;
            }

            if (applicationUser.CompanyId == 0 || invitedEmail.CompanyId == 0)
            {
                return false;
            }

            return (applicationUser.CompanyId == invitedEmail.CompanyId);
        }
    }
}
