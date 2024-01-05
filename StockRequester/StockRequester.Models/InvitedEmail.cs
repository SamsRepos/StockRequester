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
    public class InvitedEmail
    {
        [Key] public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //
        // Company:
        //

        public int? CompanyId { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(CompanyId))]
        [InverseProperty(nameof(Company.InvitedEmails))]
        public Company Company { get; set; }

        //
        //
        //
    }
}
