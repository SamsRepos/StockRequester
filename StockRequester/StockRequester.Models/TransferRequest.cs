using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
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

        [Required] public string Item { get; set; }
        ////[Required] public Item Item {  get; set; }
        //[Required] public int Quantity { get; set; }
        //[Required] public string Reason { get; set; }

        //public int LocationFromId { get; set; }

        //[ValidateNever]
        //[InverseProperty(nameof(Location.TransferRequestsFromHere))]
        //[ForeignKey(nameof(LocationFromId))]
        //public Location LocationFrom { get; set; }

        //public int LocationToId { get; set; }

        //[ValidateNever]
        //[InverseProperty(nameof(Location.TransferRequestsToHere))]
        //[ForeignKey(nameof(LocationToId))]
        //public Location LocationTo { get; set; }

        //[Required] public int CompanyId { get; set; }

        //[ValidateNever]
        //[ForeignKey(nameof(CompanyId))]
        //public Company Company { get; set; }

        //// status - requested, pending, sent, arrived/complete
        //// estimated date - a response from locationFrom
        //public string? MiscNotes { get; set; }
    }
}
