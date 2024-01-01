using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
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
        //
        //

        [Required] public string Item { get; set; }
        //[Required] public Item Item {  get; set; }
        
        [Required]
        [Range(1, 1000)]
        public int Quantity { get; set; }
        
        [Required] public string Reason { get; set; }


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

        // status - requested, pending, sent, arrived/complete
        // estimated date - a response from origin

        [DisplayName("Miscellaneous Notes")]
        public string? MiscNotes { get; set; }
    }
}
