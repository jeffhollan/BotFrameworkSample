using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSTS_API.Models
{
    public class PatchParameter
    {
        public string op { get; set; }
        public string path { get; set; }
        public object value { get; set; }
    }
}