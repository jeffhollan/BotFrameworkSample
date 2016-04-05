using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSTS_API.App_Start
{
    public static class Utils
    {
        internal static string GetTFSType(string type)
        {
            if (type.ToLower().Contains("backlog") || type.ToLower().Contains("feature"))
            {
                return "Product Backlog Item";
            }
            else
            {
                return "Bug";
            }
        }
    }
}