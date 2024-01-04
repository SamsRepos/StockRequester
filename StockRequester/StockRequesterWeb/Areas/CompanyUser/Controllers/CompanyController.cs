using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;
using StockRequester.Models.ViewModels;
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
            ApplicationUser? applicationUser = IdentityUtility.CurrentApplicationUser(_userManager, HttpContext);
            if (applicationUser is null)
            {
                //Response.StatusCode = 404;
                return NotFound();
            }

            int? companyId = applicationUser.CompanyId;
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

        [HttpPost]
        [Authorize(Roles = SD.Role_CompanyAdmin)]
        public IActionResult RegisterCompany(Company obj)
        {
            //if (obj.Name is not null &&
            //  _unitOfWork.Company.Get(u => u.Name.ToLower() == obj.Name.ToLower()) is not null)
            //{
            //    ModelState.AddModelError(
            //        "name",
            //        $"Unfortunately, the company name \"{obj.Name}\"already exists on our system"
            //    );
            //}

            if (!ModelState.IsValid)
            {
                return View();
            }

            _unitOfWork.Company.Add(obj);
            _unitOfWork.Save();

            ApplicationUser? user = IdentityUtility.CurrentApplicationUser(_userManager, HttpContext);
            //IdentityUser? user = _userManager.GetUserAsync(HttpContext.User).GetAwaiter().GetResult();

            if (user is null)
            {
                return NotFound();
            }

            if(obj.Id != 0)
            {
                user.CompanyId = obj.Id;
                _unitOfWork.ApplicationUser.Update(user);
                _unitOfWork.Save();
            }

            TempData["success"] = $"Company \"{obj.Name}\" created successfully";
            return RedirectToAction(nameof(Index));

        }

        [Authorize(Roles = SD.Role_CompanyUser)]
        public IActionResult JoinCompany()
        {
            SelectCompanyViewModel vm = new SelectCompanyViewModel();

            vm.CompaniesList = _unitOfWork.Company.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
            );

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_CompanyUser)]
        public IActionResult JoinCompany(SelectCompanyViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            ApplicationUser? user = IdentityUtility.CurrentApplicationUser(_userManager, HttpContext);

            if (user is null)
            {
                return NotFound();
            }

            if (vm.CompanyId != 0)
            {
                user.CompanyId = vm.CompanyId;
                _unitOfWork.ApplicationUser.Update(user);
                _unitOfWork.Save();
            }

            TempData["success"] = $"Joined company successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
