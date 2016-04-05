using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSTF_RD_Bot.Models
{
    public class TFSResponse
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string AreaPath { get; set; }
        public string State { get; set; }
        public string IterationPath { get; set; }
        public string LinkType { get; set; }
        public int LinkId { get; set; }

        public int id { get; set; }
    }
}