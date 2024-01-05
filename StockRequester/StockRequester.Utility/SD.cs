using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace StockRequester.Utility
{
    public static class SD
    {
        public const string Role_SiteAdmin     = "SiteAdmin";
        public const string Role_CompanyAdmin  = "CompanyAdmin";
        public const string Role_CompanyUser   = "CompanyUser";

        public const string RegisterRoleText_CompanyAdmin = "Register a new company profile";
        public const string RegisterRoleText_CompanyUser  = "Join an existing company profile";

        public const string RequestStatus_Pending   = "Pending";
        public const string RequestStatus_OnShelf   = "On Transfer Shelf";
        public const string RequestStatus_Sent      = "Sent";
        public const string RequestStatus_Fulfilled = "Fulfilled";
        public const string RequestStatus_Cancelled = "Cancelled";

        public static List<SelectListItem> StatusesList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text  = RequestStatus_Pending,
                    Value = RequestStatus_Pending
                },
                new SelectListItem()
                {
                    Text  = RequestStatus_OnShelf,
                    Value = RequestStatus_OnShelf
                },
                new SelectListItem()
                {
                    Text  = RequestStatus_Sent,
                    Value = RequestStatus_Sent
                },
                new SelectListItem()
                {
                    Text  = RequestStatus_Fulfilled,
                    Value = RequestStatus_Fulfilled
                },
                new SelectListItem()
                {
                    Text  = RequestStatus_Cancelled,
                    Value = RequestStatus_Cancelled
                }
            };
        }
    }
}

