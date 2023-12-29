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
            if(!ModelState.IsValid)
            {
                return View();
            }

            _db.Companies.Add(obj);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
