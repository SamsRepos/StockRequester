﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.Models.ViewModels
{
    public class CompanyUsersViewModel
    {
        [ValidateNever]
        public IEnumerable<ApplicationUser> CompanyAdmins { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CompanyUsers { get; set; }

        [ValidateNever]
        public ICollection<InvitedEmail> InvitedEmails { get; set; }
    }
}
