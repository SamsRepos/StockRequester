using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;
using StockRequester.Models.ViewModels;
using StockRequester.Utility;
using System.Transactions;

namespace StockRequesterWeb.Controllers
{
    public class TransferRequestController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransferRequestController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Upsert(int companyId, int? id)
        {
            if (companyId == 0)
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

            Company companyFromDb = _unitOfWork.Company.Get(
                (u => u.Id == companyId),
                includeProperties: nameof(Company.Locations)
            );

            TransferRequest tr;
            bool locationAlreadyExists = !(id is null || id == 0);
            if (locationAlreadyExists) // update
            {
                tr = _unitOfWork.TransferRequest.Get(
                    (u => u.Id == id),
                    includeProperties: $"{nameof(TransferRequest.Company)},{nameof(TransferRequest.OriginLocation)},{nameof(TransferRequest.DestinationLocation)}"
                );
            }
            else // insert
            {
                tr = new TransferRequest();
                tr.CompanyId = companyId;

                tr.Company = companyFromDb;
            }

            IEnumerable<SelectListItem> companyLocationsList = companyFromDb.Locations.Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
            );

            TransferRequestViewModel trVm = new()
            {
                TransferRequest = tr,
                CompanyLocationsList = companyLocationsList
            };

            return View(trVm);
        }

        [HttpPost]
        public IActionResult Upsert(TransferRequestViewModel trVm)
        {
            TransferRequest obj = trVm.TransferRequest;

            Company companyFromDb = _unitOfWork.Company.Get(
                (u => u.Id == obj.CompanyId),
                includeProperties: nameof(Company.Locations)
            );

            if (!ModelState.IsValid)
            {
                TempData["error"] = $"ModelState is not valid";

                trVm.CompanyLocationsList = companyFromDb.Locations.Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                );

                return View(trVm);
            }

            bool trAlreadyExists = !(obj.Id == 0);

            if (trAlreadyExists)
            {
                _unitOfWork.TransferRequest.Update(obj);
            }
            else
            {
                _unitOfWork.TransferRequest.Add(obj);
            }
            _unitOfWork.Save();
            TempData["success"] = $"Transfer request {(trAlreadyExists ? "updated" : "created")} successfully";

            return RedirectToAction(nameof(Index), ControllerUtility.ControllerName(typeof(CompanyController)), new { companyId = obj.CompanyId });
        }


        public IActionResult Delete()
        {
            return View();
        }
    }
}
