﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StockRequester.Models
{
    public class Company
    {
        [Key] public int Id { get; set; }
        
        [Required(ErrorMessage="Company Name is required!")]
        [MaxLength(40)]
        [DisplayName("Company Name")]
        public string Name { get; set; }

        [ValidateNever]
        public ICollection<Location> Locations { get; set; }

        [ValidateNever]
        public ICollection<TransferRequest> TransferRequests { get; set; }

        [ValidateNever]
        public ICollection<ApplicationUser> Users { get; set; }
      
        // reason tags
    }
}
