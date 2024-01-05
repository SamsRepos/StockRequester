using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.Models
{
    public class TransferRequest
    {
        [Key] public int Id { get; set; }

        //
        // Company:
        //

        // non-nullable id for cascade delete
        [Required] public int CompanyId { get; set; }

        [ValidateNever]
        [InverseProperty(nameof(Company.TransferRequests))]
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        //
        // Requesting User:
        //

        //[Required]
        //public int RequestingUserId { get; set; }

        //[ValidateNever]
        //[ForeignKey(nameof(RequestingUserId))]
        //public ApplicationUser RequestingUser { get; set; }

        //
        //
        //

        [Required] public string Item { get; set; }
        //[Required] public Item Item {  get; set; }
        
        [Required]
        [Range(1, 10000)]
        public int Quantity { get; set; }
        

        //
        // Destination Location (where the request comes from):
        //

        [Required]
        [Display(Name = "Destination Location")]
        public int? DestinationLocationId { get; set; }

        [ValidateNever]
        [InverseProperty(nameof(Location.TransferRequestsToThisDestination))]
        [ForeignKey(nameof(DestinationLocationId))]
        public Location DestinationLocation { get; set; }

        //
        // Origin Location:
        //

        [Required]
        [Display(Name = "Origin Location")]
        public int? OriginLocationId { get; set; }

        [ValidateNever]
        [InverseProperty(nameof(Location.TransferRequestsFromThisOrigin))]
        [ForeignKey(nameof(OriginLocationId))]
        public Location OriginLocation { get; set; }

        //
        //
        //

        //[Required] public string Status { get; set; }



        // - Pending, by Destination
        //    - Ability for Origin to reply e.g. "we have these books but they are not signed, do you still want them?" 
        // - On stock transfer shelf, by Origin
        // - Sent(can add deliver tracking numbers), by Origin
        // - Cancelled (must give reason) therefore archived, by either Origin or Destination
        // - Fulfilled therefore archived, by Destination

        // - The ability to query/flag up requests that are taking a while

        // - Estimated date?, by Origin

        [DisplayName("Miscellaneous Notes")]
        public string? MiscNotes { get; set; }

        
    }
}
