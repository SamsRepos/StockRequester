using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;
using StockRequester.Utility;
using System.ComponentModel.Design;

namespace StockRequesterWeb.Areas.CompanyUser.Controllers
{
    [Authorize]
    [Area(nameof(CompanyUser))]
    public class LocationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public LocationController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork  = unitOfWork;
            _userManager = userManager;
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

            ApplicationUser? applicationUser = IdentityUtility.CurrentApplicationUser(_userManager, HttpContext);
            if(!(IdentityUtility.UserHasAccess(applicationUser, locationFromDb)) &&
               !(User.IsInRole(SD.Role_SiteAdmin))
            )
            {
                return RedirectToPage($"/Identity/Account/AccessDenied");
            }

            foreach (TransferRequest tc in locationFromDb.TransferRequestsFromThisOrigin)
            {
                tc.OriginLocation = _unitOfWork.Location.Get(u => u.Id == tc.OriginLocationId);
                tc.DestinationLocation = _unitOfWork.Location.Get(u => u.Id == tc.DestinationLocationId);
            }

            foreach (TransferRequest tc in locationFromDb.TransferRequestsToThisDestination)
            {
                tc.OriginLocation = _unitOfWork.Location.Get(u => u.Id == tc.OriginLocationId);
                tc.DestinationLocation = _unitOfWork.Location.Get(u => u.Id == tc.DestinationLocationId);
            }

            return View(locationFromDb);
        }

        public IActionResult Upsert(int? id)
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

            Location location;
            bool locationAlreadyExists = !(id is null || id == 0);
            if (locationAlreadyExists) // update
            {
                location = _unitOfWork.Location.Get(
                    u => u.Id == id,
                    includeProperties: nameof(Location.Company)
                );

                if(!(IdentityUtility.UserHasAccess(applicationUser, location)) &&
                   !(User.IsInRole(SD.Role_SiteAdmin))
                )
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

        [HttpPost]
        public IActionResult Upsert(Location obj)
        {
            Company company = _unitOfWork.Company.Get(
                u => u.Id == obj.CompanyId,
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
                _unitOfWork.Location.Update(obj);
            }
            else
            {
                _unitOfWork.Location.Add(obj);
            }
            _unitOfWork.Save();
            TempData["success"] = $"Location \"{obj.Name}\" {(locationAlreadyExists ? "updated" : "created")} successfully";

            return RedirectToAction(nameof(Index), ControllerUtility.ControllerName(typeof(CompanyController)));
        }

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

            ApplicationUser? applicationUser = IdentityUtility.CurrentApplicationUser(_userManager, HttpContext);
            if (!(IdentityUtility.UserHasAccess(applicationUser, locationFromDb)) &&
               !(User.IsInRole(SD.Role_SiteAdmin))
            )
            {
                return RedirectToPage($"/Identity/Account/AccessDenied");
            }

            return View(locationFromDb);
        }

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

            ApplicationUser? applicationUser = IdentityUtility.CurrentApplicationUser(_userManager, HttpContext);
            if (!(IdentityUtility.UserHasAccess(applicationUser, locationFromDb)) &&
                !(User.IsInRole(SD.Role_SiteAdmin))
            )
            {
                return RedirectToPage($"/Identity/Account/AccessDenied");
            }

            int companyId = locationFromDb.CompanyId;

            _unitOfWork.Location.Remove(locationFromDb);
            _unitOfWork.Save();
            TempData["success"] = $"Location \"{locationFromDb.Name}\" deleted successfully";

            return RedirectToAction(nameof(Index), ControllerUtility.ControllerName(typeof(CompanyController)));
        }

    }
}
