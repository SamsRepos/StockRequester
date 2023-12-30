using Microsoft.AspNetCore.Mvc;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;

namespace StockRequesterWeb.Controllers
{
    public class LocationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int? companyId)
        {
            List<Location> allLocationsList = _unitOfWork.Location.GetAll(includeProperties:"Company").ToList();

            List<Location> locationsList = allLocationsList.Where(u => u.CompanyId == companyId).ToList();
            
            return View(locationsList);
        }
    }
}
