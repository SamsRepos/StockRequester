using Microsoft.AspNetCore.Mvc;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;

namespace StockRequesterWeb.Areas.User.Controllers
{
    [Area(nameof(User))]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int companyId)
        {
            Company obj = _unitOfWork.Company.Get(
                u => u.Id == companyId,
                includeProperties: $"{nameof(Company.TransferRequests)}, {nameof(Company.Locations)}"
            );

            return View(obj);
        }
    }
}
