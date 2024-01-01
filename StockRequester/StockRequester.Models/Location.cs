using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
    public class Location
    {
        [Key] public int Id { get; set; }
        
        [Required]
        [DisplayName("Location Name")]
        public string Name { get; set; }

        [Required] public int CompanyId {  get; set; }

        [ValidateNever]
        [ForeignKey(nameof(CompanyId))]
        [InverseProperty(nameof(Company.Locations))]
        public Company Company { get; set; }

        [ValidateNever] public ICollection<TransferRequest> TransferRequestsToThisDestination { get; set; }
        [ValidateNever] public ICollection<TransferRequest> TransferRequestsFromThisOrigin { get; set; }

    }
}
