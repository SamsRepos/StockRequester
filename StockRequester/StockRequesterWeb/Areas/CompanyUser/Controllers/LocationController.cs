using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;
using StockRequester.Utility;
using StockRequesterWeb.Controllers;
using System.ComponentModel.Design;

namespace StockRequesterWeb.Areas.CompanyUser.Controllers
{
    [Authorize]
    [Area(nameof(CompanyUser))]
    public class LocationController : StockRequesterBaseController
    {

        public LocationController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
            : base(unitOfWork, userManager)
        {
        }

        public IActionResult Index(int id)
        {
            Location? locationFromDb = _unitOfWork.Location.Get(
                u => u.Id == id,
                includeProperties: $"{nameof(Location.TransferRequestsFromThisOrigin)},{nameof(Location.TransferRequestsToThisDestination)}"
            );

            if (locationFromDb is null)
            {
                return NotFound();
            }

            if(!CurrentUserHasAccess(locationFromDb))
            {
                return RedirectToPage($"/Identity/Account/AccessDenied");
            }

            foreach (TransferRequest tr in locationFromDb.TransferRequestsFromThisOrigin)
            {
                tr.OriginLocation = _unitOfWork.Location.Get(u => u.Id == tr.OriginLocationId);
                tr.DestinationLocation = _unitOfWork.Location.Get(u => u.Id == tr.DestinationLocationId);
                tr.Status = _unitOfWork.RequestStatus.Get(u => u.Id == tr.StatusId);
            }

            foreach (TransferRequest tr in locationFromDb.TransferRequestsToThisDestination)
            {
                tr.OriginLocation = _unitOfWork.Location.Get(u => u.Id == tr.OriginLocationId);
                tr.DestinationLocation = _unitOfWork.Location.Get(u => u.Id == tr.DestinationLocationId);
                tr.Status = _unitOfWork.RequestStatus.Get(u => u.Id == tr.StatusId);
            }

            return View(locationFromDb);
        }

        [Authorize(Roles = SD.Role_CompanyAdmin)]
        public IActionResult Upsert(int? id)
        {
            int? companyId = CurrentUserCompanyId();
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

            Location location;
            bool locationAlreadyExists = !(id is null || id == 0);
            if (locationAlreadyExists) // update
            {
                location = _unitOfWork.Location.Get(
                    u => u.Id == id,
                    includeProperties: nameof(Location.Company)
                );

                if (!CurrentUserHasAccess(location))
                {
                    return RedirectToPage($"/Identity/Account/AccessDenied");
                }
            }
            else // insert
            {
                location = new Location();
                location.CompanyId = (int)companyId;

                Company companyFromDb = _unitOfWork.Company.Get(u => u.Id == companyId);
                location.Company = companyFromDb;

            }

            return View(location);
        }

        [Authorize(Roles = SD.Role_CompanyAdmin)]
        [HttpPost]
        public IActionResult Upsert(Location obj)
        {
            Company company = _unitOfWork.Company.Get(
                u => u.Id == CurrentUserCompanyId(),
                includeProperties: nameof(Company.Locations),
                noTracking: true
            );

            Location? anyOtherWithSameName = company.Locations.ToList().Find(location => location.Name.ToLower() == obj.Name.ToLower());
            if (
              obj.Name is not null &&
              anyOtherWithSameName is not null &&
              anyOtherWithSameName.Id != obj.Id
            )
            {
                ModelState.AddModelError(
                    "name",
                    $"Your company already has a location named \"{obj.Name}\""
                );
            }

            if (!ModelState.IsValid)
            {
                TempData["error"] = $"ModelState is not valid";
                return View(obj);
            }

            bool locationAlreadyExists = !(obj.Id == 0);

            if (locationAlreadyExists)
            {
                if (!CurrentUserHasAccess(obj)) return RedirectToPage($"/Identity/Account/AccessDenied");
                _unitOfWork.Location.Update(obj);
            }
            else
            {
                _unitOfWork.Location.Add(obj);
            }
            _unitOfWork.Save();
            TempData["success"] = $"Location \"{obj.Name}\" {(locationAlreadyExists ? "updated" : "created")} successfully";

            return RedirectToAction(nameof(CompanyController.Index), ControllerUtility.ControllerName(typeof(CompanyController)));
        }

        [Authorize(Roles = SD.Role_CompanyAdmin)]
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }

            Location? locationFromDb = _unitOfWork.Location.Get(
                u => u.Id == id,
                includeProperties: $"{nameof(Location.TransferRequestsFromThisOrigin)},{nameof(Location.TransferRequestsToThisDestination)}"
            );

            if (locationFromDb is null)
            {
                return NotFound();
            }

            if (!CurrentUserHasAccess(locationFromDb))
            {
                return RedirectToPage($"/Identity/Account/AccessDenied");
            }

            return View(locationFromDb);
        }

        [Authorize(Roles = SD.Role_CompanyAdmin)]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }

            Location? locationFromDb = _unitOfWork.Location.Get(u => u.Id == id);

            if (locationFromDb is null)
            {
                return NotFound();
            }

            if (!CurrentUserHasAccess(locationFromDb))
            {
                return RedirectToPage($"/Identity/Account/AccessDenied");
            }

            _unitOfWork.Location.Remove(locationFromDb);
            _unitOfWork.Save();
            TempData["success"] = $"Location \"{locationFromDb.Name}\" deleted successfully";

            return RedirectToAction(nameof(CompanyController.Index), ControllerUtility.ControllerName(typeof(CompanyController)));
        }

    }
}
