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
        
        
        // The following, probably not contained as collection
        // rather, to be associated by foreign key relation:
        //
        // locations
        // transfer requests, or, rather, transfer request ids
        // reason tags
    }
}
