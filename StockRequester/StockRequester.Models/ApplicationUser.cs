using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required] public string Name { get; set; }

        //
        // COMPANY:
        //

        public int? CompanyId { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(CompanyId))]
        [InverseProperty(nameof(Company.Users))]
        public Company Company { get; set; }

        //
        //
        //

        // associated location
        //   ... (easily changable.
        //   ... Maybe in navbar on right, location name.
        //   ... Click for drop-down "--Change Location--")
    }
}
