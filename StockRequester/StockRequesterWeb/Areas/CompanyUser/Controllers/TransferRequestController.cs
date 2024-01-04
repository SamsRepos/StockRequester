﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;
using StockRequester.Models.ViewModels;
using StockRequester.Utility;
using System.Transactions;

namespace StockRequesterWeb.Areas.CompanyUser.Controllers
{
    [Authorize]
    [Area(nameof(CompanyUser))]
    public class TransferRequestController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public TransferRequestController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Upsert(int? id, int? originLocationId, int? destinationLocationId, int? backLocationId)
        {
            ApplicationUser? applicationUser = IdentityUtility.CurrentApplicationUser(_userManager, HttpContext);
            if (applicationUser is null) return NotFound();            

            int? companyId = applicationUser.CompanyId;
            if (companyId is null || companyId == 0)
            {
                string errorMessage = "No company ID for Upsert action";
                ModelState.AddModelError(
                    "",
                    errorMessage
                );
                TempData["error"] = errorMessage;
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            Company companyFromDb = _unitOfWork.Company.Get(
                u => u.Id == companyId,
                includeProperties: nameof(Company.Locations)
            );

            TransferRequest tr;
            bool locationAlreadyExists = !(id is null || id == 0);
            if (locationAlreadyExists) // update
            {
                tr = _unitOfWork.TransferRequest.Get(
                    u => u.Id == id,
                    includeProperties: $"{nameof(TransferRequest.Company)},{nameof(TransferRequest.OriginLocation)},{nameof(TransferRequest.DestinationLocation)}"
                );

                if (!(IdentityUtility.UserHasAccess(applicationUser, tr)) &&
                   !(User.IsInRole(SD.Role_SiteAdmin))
                )
                {
                    return RedirectToPage($"/Identity/Account/AccessDenied");
                }
            }
            else // insert
            {
                tr = new TransferRequest();
                tr.CompanyId = (int)companyId;

                tr.Company = companyFromDb;
            }

            if (originLocationId is not null && originLocationId != 0)
            {
                tr.OriginLocationId = originLocationId;
            }

            if (destinationLocationId is not null && destinationLocationId != 0)
            {
                tr.DestinationLocationId = destinationLocationId;
            }

            IEnumerable<SelectListItem> companyLocationsList = companyFromDb.Locations.Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
            );

            TransferRequestViewModel trVm = new()
            {
                TransferRequest = tr,
                CompanyLocationsList = companyLocationsList,
                BackLocation = backLocationId is not null ? _unitOfWork.Location.Get(u => u.Id == backLocationId) : null
            };

            return View(trVm);
        }

        [HttpPost]
        public IActionResult Upsert(TransferRequestViewModel trVm)
        {
            TransferRequest obj = trVm.TransferRequest;

            Company companyFromDb = _unitOfWork.Company.Get(
                u => u.Id == obj.CompanyId,
                includeProperties: nameof(Company.Locations)
            );

            if (!ModelState.IsValid)
            {
                TempData["error"] = $"ModelState is not valid";

                trVm.CompanyLocationsList = companyFromDb.Locations.Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                );

                return View(trVm);
            }

            bool trAlreadyExists = !(obj.Id == 0);

            if (trAlreadyExists)
            {
                _unitOfWork.TransferRequest.Update(obj);
            }
            else
            {
                _unitOfWork.TransferRequest.Add(obj);
            }
            _unitOfWork.Save();
            TempData["success"] = $"Transfer request {(trAlreadyExists ? "updated" : "created")} successfully";

            return RedirectToAction(nameof(CompanyController.Index), ControllerUtility.ControllerName(typeof(CompanyController)));
        }


        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }

            TransferRequest? trFromDb = _unitOfWork.TransferRequest.Get(
                u => u.Id == id,
                includeProperties: $"{nameof(TransferRequest.DestinationLocation)},{nameof(TransferRequest.OriginLocation)}"
            );

            if (trFromDb is null)
            {
                return NotFound();
            }

            ApplicationUser? applicationUser = IdentityUtility.CurrentApplicationUser(_userManager, HttpContext);
            if (!(IdentityUtility.UserHasAccess(applicationUser, trFromDb)) &&
                !(User.IsInRole(SD.Role_SiteAdmin))
            )
            {
                return RedirectToPage($"/Identity/Account/AccessDenied");
            }

            return View(trFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }

            TransferRequest? trFromDb = _unitOfWork.TransferRequest.Get(u => u.Id == id);

            if (trFromDb is null)
            {
                return NotFound();
            }

            ApplicationUser? applicationUser = IdentityUtility.CurrentApplicationUser(_userManager, HttpContext);
            if (!(IdentityUtility.UserHasAccess(applicationUser, trFromDb)) &&
                !(User.IsInRole(SD.Role_SiteAdmin))
            )
            {
                return RedirectToPage($"/Identity/Account/AccessDenied");
            }

            _unitOfWork.TransferRequest.Remove(trFromDb);
            _unitOfWork.Save();
            TempData["success"] = $"Transfer Request deleted successfully";

            return RedirectToAction(nameof(CompanyController.Index), ControllerUtility.ControllerName(typeof(CompanyController)));
        }
    }
}
