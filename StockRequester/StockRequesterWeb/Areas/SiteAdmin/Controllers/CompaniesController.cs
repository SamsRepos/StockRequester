using Microsoft.AspNetCore.Mvc;
using StockRequester.DataAccess.Data;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;
using StockRequester.Utility;

namespace StockRequesterWeb.Areas.SiteAdmin.Controllers
{
    [Area(nameof(SiteAdmin))]
    public class CompaniesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompaniesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> companiesList = _unitOfWork.Company.GetAll().ToList();
            return View(companiesList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Company obj)
        {
            if (obj.Name is not null &&
              _unitOfWork.Company.Get(u => u.Name.ToLower() == obj.Name.ToLower()) is not null)
            //(_db.Companies.FirstOrDefault(u => u.Name.ToLower() == obj.Name.ToLower()) is not null))
            {
                ModelState.AddModelError(
                    "name",
                    $"Unfortunately, the company name \"{obj.Name}\"already exists on our system"
                );
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            _unitOfWork.Company.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = $"Company \"{obj.Name}\" created successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }

            Company? companyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
            if (companyFromDb is null)
            {
                return NotFound();
            }

            return View(companyFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Company obj)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            _unitOfWork.Company.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = $"Company \"{obj.Name}\" updated successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }

            Company? companyFromDb = _unitOfWork.Company.Get(u => u.Id == id);

            if (companyFromDb is null)
            {
                return NotFound();
            }

            return View(companyFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }

            Company? companyFromDb = _unitOfWork.Company.Get(u => u.Id == id);

            if (companyFromDb is null)
            {
                return NotFound();
            }

            _unitOfWork.Company.Remove(companyFromDb);
            _unitOfWork.Save();
            TempData["success"] = $"Company \"{companyFromDb.Name}\" deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
