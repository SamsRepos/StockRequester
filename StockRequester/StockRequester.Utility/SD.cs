using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
