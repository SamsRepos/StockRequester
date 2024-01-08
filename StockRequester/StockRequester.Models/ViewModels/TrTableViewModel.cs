using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.Models.ViewModels
{
    public class TrTableViewModel
    {
        public TrTableOptions TableOptions { get; set; }
        public ICollection<TransferRequest> TransferRequests { get; set; }

        public TrTableViewModel()
        {
            TableOptions = new TrTableOptions();
        }
    }
}
