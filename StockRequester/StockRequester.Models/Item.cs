using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StockRequester.Models
{
    public class Item
    {
        public static Item BlobToItem(string blob)
        {
            if(blob is null) return new Item();
            return JsonConvert.DeserializeObject<Item>(blob);
        }

        public static string ItemToBlob(Item item)
        {
            return JsonConvert.SerializeObject(item);
        }

        public string ToBlob()
        {
            return ItemToBlob(this);
        }

        [DisplayName("Item Name")]
        public string Name { get; set; }

        [DisplayName("Item Description")]
        public string Description { get; set; }
    }
}
