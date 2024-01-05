﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;
using StockRequester.Utility;
using StockRequesterWeb.Controllers;

namespace StockRequesterWeb.Areas.CompanyAdmin.Controllers
{
    [Area(nameof(CompanyAdmin))]
    [Authorize(Roles = $"{SD.Role_CompanyAdmin},{SD.Role_SiteAdmin}")]
    public class CompanyUsers : StockRequesterBaseController
    {
        public CompanyUsers(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
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

            return RedirectToAction(nameof(Index));
        }
    }
}
