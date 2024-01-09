using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.Models.ViewModels
{
    public class TrInfoViewModel
    {
        public TransferRequest TransferRequest { get; set; }
        public Item Item { get; set; }
        public Location? BackLocation { get; set; }

        public int? GetBackLocationId()
        {
            return (BackLocation is not null) ? BackLocation.Id : null;
        }
    }
}
