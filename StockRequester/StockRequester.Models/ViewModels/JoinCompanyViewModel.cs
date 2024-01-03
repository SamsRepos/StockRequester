using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace StockRequester.Models.ViewModels
{
    public class SelectCompanyViewModel
    {
        [Required(ErrorMessage = "Comapny is required")] public int CompanyId { get; set; }
        [ValidateNever] public IEnumerable<SelectListItem> CompaniesList { get; set; }
    }
}
