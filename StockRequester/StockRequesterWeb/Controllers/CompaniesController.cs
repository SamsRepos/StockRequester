using Microsoft.AspNetCore.Mvc;
using StockRequester.DataAccess.Data;
using StockRequester.Models;

namespace StockRequesterWeb.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CompaniesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Company> objCompaniesList = _db.Companies.ToList();
            return View(objCompaniesList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Company obj)
        {
            if(obj.Name is not null &&
              (_db.Companies.FirstOrDefault(u => u.Name.ToLower() == obj.Name.ToLower()) is not null))
            {
                ModelState.AddModelError(
                    "name",
                    $"Unfortunately, the company name \"{obj.Name}\"already exists on our system"
                );
            }

            if(!ModelState.IsValid)
            {
                return View();
            }

            _db.Companies.Add(obj);
            _db.SaveChanges();
            TempData["success"] = $"Company \"{obj.Name}\" created successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if(id is null || id == 0)
            {
                return NotFound();
            }

            Company? companyFromDb = _db.Companies.Find(id);
            if(companyFromDb is null)
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

            _db.Companies.Update(obj);
            _db.SaveChanges();
            TempData["success"] = $"Company \"{obj.Name}\" updated successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }

            Company? companyFromDb = _db.Companies.Find(id);

            if (companyFromDb is null)
            {
                return NotFound();
            }

            return View(companyFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Company? companyFromDb = _db.Companies.Find(id);

            if (companyFromDb is null)
            {
                return NotFound();
            }

            _db.Companies.Remove(companyFromDb);
            _db.SaveChanges();
            TempData["success"] = $"Company \"{companyFromDb.Name}\" deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
