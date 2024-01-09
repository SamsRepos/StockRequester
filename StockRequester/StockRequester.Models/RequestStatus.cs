using StockRequester.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Tracking Info (Optional)")]
        public string? TrackingInfo {  get; set; }

        [DisplayName("Cancellation Reason (Required)")]
        public string? CancellationReason { get; set; }

    }
}
