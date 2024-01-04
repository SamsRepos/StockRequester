using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;
using StockRequester.Utility;

namespace StockRequesterWeb.Areas.CompanyAdmin.Controllers
{
    [Area(nameof(CompanyAdmin))]
    [Authorize(Roles = $"{SD.Role_CompanyAdmin},{SD.Role_SiteAdmin}")]
    public class CompanyUsers : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;


        public CompanyUsers(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ApplicationUser? applicationUser = IdentityUtility.CurrentApplicationUser(_userManager, HttpContext);
            if (applicationUser is null)
            {
                //Response.StatusCode = 404;
                return NotFound();
            }

            int? companyId = applicationUser.CompanyId;
            if (companyId is null || companyId == 0) return NotFound();

            Company companyFromDb = _unitOfWork.Company.Get(
                u => u.Id == companyId,
                includeProperties: nameof(Company.Users)
            );

            return View(companyFromDb);
        }
    }
}
