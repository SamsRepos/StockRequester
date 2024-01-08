using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.Models.ViewModels
{
    public class TrUpsertViewModel
    {
        public TransferRequest TransferRequest { get; set; }
        public Item Item { get; set; }
        
        [ValidateNever]
        public IEnumerable<SelectListItem> CompanyLocationsList { get; set; }

        public Location? BackLocation { get; set; }
    }
}
