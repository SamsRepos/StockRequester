using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;
using StockRequester.Models.ViewModels;
using StockRequester.Utility;
using StockRequesterWeb.Controllers;

namespace StockRequesterWeb.Areas.CompanyAdmin.Controllers
{
    [Area(nameof(CompanyAdmin))]
    [Authorize(Roles = $"{SD.Role_CompanyAdmin},{SD.Role_SiteAdmin}")]
    public class CompanyUsersController : StockRequesterBaseController
    {
        public CompanyUsersController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
            : base(unitOfWork, userManager)
        {
        }

        public IActionResult Index()
        {
            int? companyId = CurrentUserCompanyId();
            if (companyId is null || companyId == 0) return NotFound();

            Company companyFromDb = _unitOfWork.Company.Get(
                u => u.Id == companyId,
                includeProperties: $"{nameof(Company.Users)},{nameof(Company.InvitedEmails)}"
            );

            var companyAdmins = _userManager.GetUsersInRoleAsync(
                SD.Role_CompanyAdmin
            ).Result.Select(u => u);

            //CompanyUsersViewModel vm = new CompanyUsersViewModel()
            //{
            //    CompanyAdmins = companyFromDb.Users.Select(u => u.CompanyId ==)
            //}

            return View(companyFromDb);
        }

        public IActionResult Invite()
        {
            InvitedEmail obj = new InvitedEmail()
            {
                CompanyId = CurrentUserCompanyId()
            };

            return View(obj);
        }

        [HttpPost]
        public IActionResult Invite(InvitedEmail obj)
        {
            int? companyId = CurrentUserCompanyId();
            if (companyId is null || companyId == 0) return NotFound();

            Company companyFromDb = _unitOfWork.Company.Get(
                (u => u.Id == companyId),
                includeProperties: nameof(Company.InvitedEmails)
            );

            companyFromDb.InvitedEmails.Add(obj);
            _unitOfWork.Company.Update(companyFromDb);
            _unitOfWork.Save();

            TempData["success"] = $"Created company invite for {obj.Email}";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteInvite(int id)
        {
            InvitedEmail? invitedEmailFromDb = _unitOfWork.InvitedEmail.Get(u=>u.Id == id);

            if(invitedEmailFromDb is null) return NotFound();

            if(!CurrentUserHasAccess(invitedEmailFromDb)) return NotFound();

            _unitOfWork.InvitedEmail.Remove(invitedEmailFromDb);
            _unitOfWork.Save();

            TempData["success"] = $"Email invite \"{invitedEmailFromDb.Email}\" deleted successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}
