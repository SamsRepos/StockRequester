using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.Models
{
    public class TransferRequest
    {
        [Key] public int Id { get; set; }
        [Required] public Item Item {  get; set; }
        [Required] public int Quantity { get; set; }
        [Required] public string Reason { get; set; }
        [Required] public Location LocationFrom { get; set; }
        [Required] public Location LocationTo { get; set; }
        // this even allows for transfers between locations at different companies
        //   potentially - future implementation maybe

        // status - requested, pending, sent, arrived/complete
        // estimated date - a response from locationFrom
        public string MiscNotes { get; set; }
    }
}
