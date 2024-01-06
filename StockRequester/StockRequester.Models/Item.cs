using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace StockRequester.Models
{
    public class Item
    {
        public static Item BlobToItem(string blob)
        {
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

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
