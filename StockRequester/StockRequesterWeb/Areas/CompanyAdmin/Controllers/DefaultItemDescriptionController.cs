using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;
using StockRequester.Models.ViewModels;
using StockRequester.Utility;
using StockRequesterWeb.Areas.CompanyUser.Controllers;
using StockRequesterWeb.Controllers;

namespace StockRequesterWeb.Areas.CompanyAdmin.Controllers
{
    [Area(nameof(CompanyAdmin))]
    [Authorize(Roles = $"{SD.Role_CompanyAdmin},{SD.Role_SiteAdmin}")]
    public class DefaultItemDescriptionController : StockRequesterBaseController
    {
        public DefaultItemDescriptionController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
            : base(unitOfWork, userManager)
        {
        }

        public IActionResult Upsert()
        {
            Company companyFromDb = _unitOfWork.Company.Get(
                u => u.Id == CurrentUserCompanyId()
            );

            string str = companyFromDb.DefaultItemDescription is not null ? companyFromDb.DefaultItemDescription : "";

            DefaultItemDescriptionViewModel vm = new DefaultItemDescriptionViewModel
            {
                DefaultItemDescription = str
            };


            return View(vm);
        }

        [HttpPost]
        public IActionResult Upsert(DefaultItemDescriptionViewModel vm)
        {
            Company companyFromDb = _unitOfWork.Company.Get(
                u => u.Id == CurrentUserCompanyId()
            );
            companyFromDb.DefaultItemDescription = vm.DefaultItemDescription;

            _unitOfWork.Company.Update(companyFromDb);
            _unitOfWork.Save();

            TempData["success"] = $"Default Item Description updated successfully";

            return RedirectToAction(
                nameof(CompanyController.Index), 
                ControllerUtility.ControllerName(typeof(CompanyController)),
                new { area = nameof(CompanyUser) }
            );
        }
    }
}
