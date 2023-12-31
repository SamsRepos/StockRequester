using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;
using StockRequester.Models.ViewModels;
using System.ComponentModel.Design;

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
            var viewModel = GetLocationsViewModel(companyId);
            return View(viewModel);
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
                    u => u.Id == id,
                    includeProperties:"Company"
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
            Location? anyOtherWithSameName = LocationsAtCompany(obj.CompanyId).Find(location => location.Name.ToLower() == obj.Name.ToLower());
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

            return View(nameof(Index), GetLocationsViewModel(obj.CompanyId));
            //return RedirectToAction(nameof(Index), new { id = obj.CompanyId } );
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

            int companyId = locationFromDb.CompanyId;

            if (locationFromDb is null)
            {
                return NotFound();
            }

            _unitOfWork.Location.Remove(locationFromDb);
            _unitOfWork.Save();
            TempData["success"] = $"Location \"{locationFromDb.Name}\" deleted successfully";
            return View(nameof(Index), GetLocationsViewModel(companyId));
        }

        //
        // PRIVATE:
        //

        private List<Location> LocationsAtCompany(int companyId)
        {
            List<Location> allLocationsList = _unitOfWork.Location.GetAll(includeProperties: "Company").ToList();
            List<Location> companyLocationsList = allLocationsList.Where(u => u.CompanyId == companyId).ToList();
            return companyLocationsList;
        }

        private LocationsViewModel GetLocationsViewModel(int companyId)
        {
            LocationsViewModel result = new LocationsViewModel()
            {
                Locations = LocationsAtCompany(companyId),
                Company = _unitOfWork.Company.Get(u=>u.Id == companyId)
            };

            return result;
        }
        
    }
}
