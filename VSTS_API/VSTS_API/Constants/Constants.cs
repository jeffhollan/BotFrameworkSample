using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace VSTS_API
{
    public static class Constants
    {
        public static string api_version = "1.0";
        public static string AAD_Client_Id = ConfigurationManager.AppSettings["ClientID"];
        public static string AAD_Client_Secret = ConfigurationManager.AppSettings["ClientSecret"];

        public static string SecretUri = ConfigurationManager.AppSettings["SecretUri"];
    }
}