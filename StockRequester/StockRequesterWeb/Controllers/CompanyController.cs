using Microsoft.AspNetCore.Mvc;
using StockRequester.DataAccess.Repository.IRepository;

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
            //List<TransferRequest> allTransferRequests = _unitOfWork.TransferRequest.GetAll(includeProperties: "Company").ToList();
            //List<Location> companyLocationsList = allLocationsList.Where(u => u.CompanyId == companyId).ToList();
            //return companyLocationsList;

            //CompanyViewModel viewModel = new CompanyViewModel
            //{
            //    // Company = _unitOfWork.Company.Get(u => u.Id == companyId)

            //};

            return View();
        }
    }
}
