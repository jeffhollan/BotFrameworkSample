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
        [Required]
        public string access_token { get; set; }
        [Required]
        public string instance { get; set; }
        [Required]
        public workItemEnum workItemType { get; set; }
        [Required]
        public string area { get; set; }
        [Required]
        public string iteration { get; set; }
        [Required]
        public string title { get; set; }
        [Metadata("Assigned To", "Email or Full Name")]
        public string assigned_to { get; set; }
        public string description { get; set; }
        public int? priority { get; set; }
        public string discussion { get; set; }
        public string system_info { get; set; }
        public string tags { get; set; }
        public int? parent_link_id { get; set; }
        
        /// <summary>
        /// Return the value to be placed in a VSO REST call based on the enum value
        /// </summary>
        /// <param name="selection">Work Item Type Enum</param>
        /// <returns>URL Path Parameter</returns>
        public string getWorkItemTypeURL(workItemEnum selection)
        {
             var dict = new Dictionary<workItemEnum, string> {
                 {  workItemEnum.BUG, "$Bug"  },
                 {  workItemEnum.FEATURE, "$Feature"  },
                 {  workItemEnum.TASK, "$Task"  },
                 {  workItemEnum.EPIC, "$Epic"  },
                 {  workItemEnum.PBI, "$Product%20Backlog%20Item"  }
                };

            return dict[selection];
        }    
        public enum workItemEnum
        {
            BUG=0, FEATURE=1, TASK=2, EPIC=3, [Display(Name="Product Backlog Item")]PBI=4
        }
    }
}