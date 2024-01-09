using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StockRequester.Models.ViewModels
{
    public  class UpdateStatusViewModel
    {
        public TransferRequest TransferRequest { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> StatusesList { get; set; }

        public Location? BackLocation { get; set; }
    }
}
