using StockRequester.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.Models
{
    public class RequestStatus
    {
        [Key] public int Id { get; set; }

        [Required] public string Status { get; set; }

        public bool CanBeArchived()
        {
            return ((Status == SD.RequestStatus_Fulfilled) || (Status == SD.RequestStatus_Cancelled));
        }

        // - Pending, by Destination
        //    - Ability for Origin to reply e.g. "we have these books but they are not signed, do you still want them?" 
        // - On stock transfer shelf, by Origin
        // - Sent(can add deliver tracking numbers), by Origin
        // - Cancelled (must give reason) therefore archived, by either Origin or Destination
        // - Fulfilled therefore archived, by Destination

    }
}
