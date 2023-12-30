using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.Models.ViewModels
{
    public class LocationsViewModel
    {
        public Company Company { get; set; }
        public List<Location> Locations { get; set; }
    }
}
