using System.ComponentModel.DataAnnotations;

namespace StockRequester.Models
{
    // stock/inventory item
    public class Item
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        public string Description { get; set; }
    }
}
