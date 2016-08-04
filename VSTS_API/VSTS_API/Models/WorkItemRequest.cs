using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using TRex.Metadata;

namespace VSTS_API.Models
{
    public class WorkItemRequest
    {
        [Metadata("Access Token", "Basic auth value for request header - base64 :accesstoken")]
        public string access_token { get; set; }
        [Required]
        public string instance { get; set; }
        [Required]
        public string project { get; set; }
        [Required]
        public workItemEnum workItemType { get; set; }
        
        public string area_path { get; set; }
        public string iteration_path { get; set; }
        [Required]
        public string title { get; set; }
        [Metadata("Assigned To", "Email or Full Name")]
        public string assigned_to { get; set; }
        public string description { get; set; }
        public int? priority { get; set; }
        public string system_info { get; set; }
        public string tags { get; set; }
        public int? parent_link_id { get; set; }


        public static Dictionary<string,string> pathMappingDictionary = new Dictionary<string, string>
            {
                { "area_path", "/fields/System.AreaPath" },
                { "iteration_path", "/fields/System.IterationPath" },
                { "title", "/fields/System.Title" },
                { "assigned_to", "/fields/System.AssignedTo" },
                { "description", "/fields/System.Description" },
                { "priority", "/fields/Microsoft.VSTS.Common.Priority" },
                { "system_info", "/fields/Microsoft.VSTS.TCM.SystemInfo" },
                { "tags", "/fields/System.Tags" },

            };
        /// <summary>
        /// Return the value to be placed in a VSO REST call based on the enum value
        /// </summary>
        /// <param name="selection">Work Item Type Enum</param>
        /// <returns>URL Path Parameter</returns>
        public string getWorkItemTypeURL()
        {
             var dict = new Dictionary<workItemEnum, string> {
                 {  workItemEnum.BUG, "$Bug"  },
                 {  workItemEnum.FEATURE, "$Feature"  },
                 {  workItemEnum.TASK, "$Task"  },
                 {  workItemEnum.EPIC, "$Epic"  },
                 {  workItemEnum.PBI, "$Product%20Backlog%20Item"  }
                };

            return dict[workItemType];
        }    
        public enum workItemEnum
        {
            BUG=0, FEATURE=1, TASK=2, EPIC=3, [Display(Name="Product Backlog Item")]PBI=4
        }
    }
}