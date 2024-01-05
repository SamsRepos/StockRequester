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
using StockRequesterWeb.Controllers;

namespace StockRequesterWeb.Areas.CompanyUser.Controllers
{
    [Area(nameof(CompanyUser))]
    [Authorize]
    public class CompanyController : StockRequesterBaseController
    {
        public CompanyController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
            : base(unitOfWork, userManager)
        {
        }

        public IActionResult Index()
        {
            int? companyId = CurrentUserCompanyId();
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

            Company companyFromDb = _unitOfWork.Company.Get(
                u => u.Id == companyId,
                includeProperties: $"{nameof(Company.TransferRequests)},{nameof(Company.Locations)},{nameof(Company.Users)}"
            );

            foreach (TransferRequest tr in companyFromDb.TransferRequests)
            {
                tr.Status = _unitOfWork.RequestStatus.Get(u => u.Id == tr.StatusId);
            }

            return View(companyFromDb);
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

            ApplicationUser? applicationUser = CurrentApplicationUser();

            if (applicationUser is null)
            {
                return NotFound();
            }

            if(obj.Id != 0)
            {
                applicationUser.CompanyId = obj.Id;
                _unitOfWork.ApplicationUser.Update(applicationUser);
                _unitOfWork.Save();
            }
            else
            {
                TempData["error"] = $"Company ID was not saved to this user's data";
            }

            TempData["success"] = $"Company \"{obj.Name}\" created successfully";
            return RedirectToAction(nameof(Index));

        }

        [Authorize(Roles = SD.Role_CompanyUser)]
        public IActionResult JoinCompany()
        {
            ApplicationUser? applicationUser = CurrentApplicationUser();
            if(applicationUser is null)
            {
                return NotFound();
            }

            InvitedEmail? invitedEmailFromDb = _unitOfWork.InvitedEmail.Get(
                (u=>u.Email == applicationUser.Email),
                includeProperties: nameof(InvitedEmail.Company)
            );

            if (invitedEmailFromDb is not null)
            {
                return View(invitedEmailFromDb);
            }
            else
            {
                return View("NoInvite", applicationUser.Email);
            }
            
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_CompanyUser)]
        public IActionResult JoinCompany(InvitedEmail invitedEmail)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            ApplicationUser? applicationUser = CurrentApplicationUser();

            if (applicationUser is null)
            {
                return NotFound();
            }

            if (invitedEmail.CompanyId != 0)
            {
                applicationUser.CompanyId = invitedEmail.CompanyId;
                _unitOfWork.ApplicationUser.Update(applicationUser);
                InvitedEmail invitedEmailFromDb = _unitOfWork.InvitedEmail.Get(u=>u.Id == invitedEmail.Id);
                _unitOfWork.InvitedEmail.Remove(invitedEmailFromDb);
                _unitOfWork.Save();
                TempData["success"] = $"Joined company successfully";
            }
            else
            {
                TempData["error"] = $"Company ID was not saved to this user's data";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
