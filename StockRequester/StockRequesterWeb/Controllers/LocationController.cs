using Microsoft.AspNetCore.Mvc;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;
using StockRequester.Utility;

namespace StockRequesterWeb.Controllers
{
    public class LocationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int companyId)
        {
            Company company = _unitOfWork.Company.Get(
                (u => u.Id == companyId),
                includeProperties: nameof(Company.Locations)
            );

            return View(company);
        }

        public IActionResult Upsert(int companyId, int? id)
        {
            if(companyId == 0)
            {
                string errorMessage = "No company ID for Upsert action";
                ModelState.AddModelError(
                    "",
                    errorMessage
                );
                TempData["error"] = errorMessage;
            }

            if(!ModelState.IsValid)
            {
                return View();
            }

            Location location;
            bool locationAlreadyExists = !(id is null || id == 0);
            if(locationAlreadyExists) // update
            {
                location = _unitOfWork.Location.Get(
                    (u => u.Id == id),
                    includeProperties: nameof(Company)
                );
            }
            else // insert
            {
                location = new Location();
                location.CompanyId = companyId;

                Company companyFromDb = _unitOfWork.Company.Get(u => u.Id == companyId);
                location.Company = companyFromDb;

            }
            
            return View(location);
        }

        [HttpPost]
        public IActionResult Upsert(Location obj)
        {
            Company company = _unitOfWork.Company.Get(
                (u => u.Id == obj.CompanyId),
                includeProperties: nameof(Company.Locations)
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

            if(locationAlreadyExists)
            {
                _unitOfWork.Location.Update(obj);
            }
            else
            {
                _unitOfWork.Location.Add(obj);
            }
            _unitOfWork.Save();
            TempData["success"] = $"Location \"{obj.Name}\" {(locationAlreadyExists ? "updated" : "created")} successfully";

            return RedirectToAction(nameof(Index), ControllerUtility.ControllerName(typeof(CompanyController)), new { companyId = obj.CompanyId } );
        }

        public IActionResult Delete(int? id)
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

            int companyId = locationFromDb.CompanyId;
            
            _unitOfWork.Location.Remove(locationFromDb);
            _unitOfWork.Save();
            TempData["success"] = $"Location \"{locationFromDb.Name}\" deleted successfully";

            return RedirectToAction(nameof(Index), ControllerUtility.ControllerName(typeof(CompanyController)), new { companyId = companyId });
        }

    }
}
