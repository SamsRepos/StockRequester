using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StockRequester.DataAccess.Repository.IRepository;
using StockRequester.Models;
using StockRequester.Models.ViewModels;
using StockRequester.Utility;
using StockRequesterWeb.Controllers;
using System.Transactions;

namespace StockRequesterWeb.Areas.CompanyUser.Controllers
{
    [Authorize]
    [Area(nameof(CompanyUser))]
    public class TransferRequestController : StockRequesterBaseController
    {
        public TransferRequestController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
            : base(unitOfWork, userManager)
        {
        }

        public IActionResult Upsert(int? id, int? originLocationId, int? destinationLocationId, int? backLocationId)
        {
            int? companyId = CurrentUserCompanyId();
            if (companyId is null || companyId == 0)
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
                u => u.Id == companyId,
                includeProperties: nameof(Company.Locations)
            );

            TransferRequest tr;
            bool locationAlreadyExists = !(id is null || id == 0);
            if (locationAlreadyExists) // update
            {
                tr = _unitOfWork.TransferRequest.Get(
                    u => u.Id == id,
                    includeProperties: $"{nameof(TransferRequest.Company)},{nameof(TransferRequest.OriginLocation)},{nameof(TransferRequest.DestinationLocation)}"
                );

                if(!CurrentUserHasAccess(tr))
                {
                    return RedirectToPage($"/Identity/Account/AccessDenied");
                }
            }
            else // insert
            {
                tr = new TransferRequest();
                tr.CompanyId = (int)companyId;

                tr.Company = companyFromDb;
            }

            if (originLocationId is not null && originLocationId != 0)
            {
                tr.OriginLocationId = originLocationId;
            }

            if (destinationLocationId is not null && destinationLocationId != 0)
            {
                tr.DestinationLocationId = destinationLocationId;
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
                Item = Item.BlobToItem(tr.ItemBlob),
                CompanyLocationsList = companyLocationsList,
                BackLocation = backLocationId is not null ? _unitOfWork.Location.Get(u => u.Id == backLocationId) : null
            };

            return View(trVm);
        }

        [HttpPost]
        public IActionResult Upsert(TransferRequestViewModel trVm)
        {
            TransferRequest tr = trVm.TransferRequest;

            if (!ModelState.IsValid)
            {
                TempData["error"] = $"ModelState is not valid";

                Company companyFromDb = _unitOfWork.Company.Get(
                    u => u.Id == CurrentUserCompanyId(),
                    includeProperties: nameof(Company.Locations)
                );

                trVm.CompanyLocationsList = companyFromDb.Locations.Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                );

                return View(trVm);
            }

            tr.ItemBlob = trVm.Item.ToBlob();

            bool trAlreadyExists = !(tr.Id == 0);

            if (trAlreadyExists)
            {
                if (!CurrentUserHasAccess(tr)) return RedirectToPage($"/Identity/Account/AccessDenied");
                _unitOfWork.TransferRequest.Update(tr);
                _unitOfWork.Save();
            }
            else
            {
                RequestStatus rs = new RequestStatus
                {
                    Status = SD.RequestStatus_Pending
                };
                _unitOfWork.RequestStatus.Add(rs);
                _unitOfWork.Save();
                if(rs.Id == 0)
                {
                    TempData["Error"] = $"RequestStatus added to DB but still has Id 0";
                    return RedirectToAction(nameof(CompanyController.Index), ControllerUtility.ControllerName(typeof(CompanyController)));
                }

                tr.StatusId = rs.Id;
                _unitOfWork.TransferRequest.Add(tr);
                _unitOfWork.Save();
            }

            TempData["success"] = $"Transfer request {(trAlreadyExists ? "updated" : "created")} successfully";

            return RedirectToAction(nameof(CompanyController.Index), ControllerUtility.ControllerName(typeof(CompanyController)));
        }

        public IActionResult UpdateStatus(int id, int? backLocationId)
        {
            TransferRequest? tr = _unitOfWork.TransferRequest.Get(
                (u => u.Id == id),
                includeProperties: $"{nameof(TransferRequest.Status)},{nameof(TransferRequest.OriginLocation)},{nameof(TransferRequest.DestinationLocation)}"
            );

            if(tr is null || !CurrentUserHasAccess(tr)) return NotFound();

            UpdateStatusViewModel vm = new UpdateStatusViewModel
            {
                TransferRequest = tr,
                StatusesList    = SD.StatusesList(),
                BackLocation    = backLocationId is not null ? _unitOfWork.Location.Get(u => u.Id == backLocationId) : null
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult UpdateStatus(UpdateStatusViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = $"ModelState is not valid";

                Company companyFromDb = _unitOfWork.Company.Get(
                    u => u.Id == CurrentUserCompanyId(),
                    includeProperties: nameof(Company.Locations)
                );

                vm.StatusesList = SD.StatusesList();

                return View(vm);
            }

            if (!CurrentUserHasAccess(vm.TransferRequest)) return NotFound();

            _unitOfWork.RequestStatus.Update(vm.TransferRequest.Status);
            _unitOfWork.Save();

            TempData["success"] = $"Status updated: {vm.TransferRequest.Status.Status}";

            return RedirectToAction(nameof(CompanyController.Index), ControllerUtility.ControllerName(typeof(CompanyController)));
        }

        [Authorize(Roles = SD.Role_CompanyAdmin)]
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }

            TransferRequest? trFromDb = _unitOfWork.TransferRequest.Get(
                u => u.Id == id,
                includeProperties: $"{nameof(TransferRequest.DestinationLocation)},{nameof(TransferRequest.OriginLocation)}"
            );

            if (trFromDb is null)
            {
                return NotFound();
            }

            if (!CurrentUserHasAccess(trFromDb))
            {
                return RedirectToPage($"/Identity/Account/AccessDenied");
            }

            return View(trFromDb);
        }

        [Authorize(Roles = SD.Role_CompanyAdmin)]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }

            TransferRequest? trFromDb = _unitOfWork.TransferRequest.Get(
                (u => u.Id == id),
                includeProperties: nameof(TransferRequest.Status)
            );
            RequestStatus? statusFromDb = trFromDb.Status;

            if (trFromDb is null)
            {
                return NotFound();
            }

            if (!CurrentUserHasAccess(trFromDb))
            {
                return RedirectToPage($"/Identity/Account/AccessDenied");
            }

            _unitOfWork.TransferRequest.Remove(trFromDb);
            _unitOfWork.RequestStatus.Remove(statusFromDb);
            _unitOfWork.Save();
            TempData["success"] = $"Transfer Request deleted successfully";

            return RedirectToAction(nameof(CompanyController.Index), ControllerUtility.ControllerName(typeof(CompanyController)));
        }
    }
}
