using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;
using StockRequester.Utility;
using StockRequesterWeb.Areas.SiteAdmin.Controllers;

namespace StockRequesterWeb.Areas.CompanyUser.Controllers
{
    [Authorize]
    [Area(nameof(CompanyUser))]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public CompanyController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            IdentityUser? userAsIdentityUser = _userManager.GetUserAsync(HttpContext.User).GetAwaiter().GetResult();
            ApplicationUser? user = (ApplicationUser)userAsIdentityUser;
            if (user is null)
            {
                //Response.StatusCode = 404;
                return NotFound();
            }

            int? companyId = user.CompanyId;
            if (companyId is null || companyId == 0)
            {
                if (User.IsInRole(SD.Role_CompanyAdmin))
                {
                    return RedirectToAction(nameof(RegisterCompany));
                }
                else if (User.IsInRole(SD.Role_CompanyUser))
                {
                    return RedirectToAction(nameof(JoinCompany));
                }
            }

            Company obj = _unitOfWork.Company.Get(
                u => u.Id == companyId,
                includeProperties: $"{nameof(Company.TransferRequests)}, {nameof(Company.Locations)}, {nameof(Company.Users)}"
            );

            return View(obj);
        }

        [Authorize(Roles = SD.Role_CompanyAdmin)]
        public IActionResult RegisterCompany()
        {
            return View();
        }

        [Authorize(Roles = SD.Role_CompanyUser)]
        public IActionResult JoinCompany()
        {
            return View();
        }
    }
}
