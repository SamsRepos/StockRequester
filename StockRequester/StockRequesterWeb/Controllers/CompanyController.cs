using Microsoft.AspNetCore.Mvc;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;

namespace StockRequesterWeb.Controllers
{
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
                (u=>u.Id == companyId),
                includeProperties: "Locations"
            );

            obj.TransferRequests = new List<TransferRequest>();

            return View(obj);
        }
    }
}
