using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.Models
{
    public class Company
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        // collection of locations
        // collection of transfer requests, or, rather, transfer request ids
        // collection of reason tags
    }
}
