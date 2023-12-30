using System;
using System.Collections.Generic;
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
        [Required] public string Name { get; set; }

        public int CompanyId {  get; set; }
        
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
    }
}
