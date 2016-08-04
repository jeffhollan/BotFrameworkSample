using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSTS_API.Models
{

    public class Fields
    {
        [JsonProperty(PropertyName = "System.AreaPath")]
        public string AreaPath { get; set; }
        [JsonProperty(PropertyName = "System.TeamProject")]
        public string TeamProject { get; set; }
        [JsonProperty(PropertyName = "System.IterationPath")]
        public string IterationPath { get; set; }
        [JsonProperty(PropertyName = "System.WorkItemType")]
        public string WorkItemType { get; set; }
        [JsonProperty(PropertyName = "System.State")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "System.Reason")]
        public string Reason { get; set; }
        [JsonProperty(PropertyName = "System.AssignedTo")]
        public string AssignedTo { get; set; }
        [JsonProperty(PropertyName = "System.CreatedDate")]
        public string CreatedDate { get; set; }
        [JsonProperty(PropertyName = "System.CreatedBy")]
        public string CreatedBy { get; set; }
        [JsonProperty(PropertyName = "System.ChangedDate")]
        public string ChangedDate { get; set; }
        [JsonProperty(PropertyName = "System.ChangedBy")]
        public string ChangedBy { get; set; }
        [JsonProperty(PropertyName = "System.Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "System.BoardColumn")]
        public string BoardColumn { get; set; }
        [JsonProperty(PropertyName = "System.BoardColumnDone")]
        public bool BoardColumnDone { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.Severity")]
        public string Severity { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.StateChangeDate")]
        public string StateChangeDate { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.Priority")]
        public int Priority { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.ValueArea")]
        public string ValueArea { get; set; }
        [JsonProperty(PropertyName = "WEF_B44AD53AA0424B6E8B2FF5ED7F937CF7_Kanban.Column")]
        public string Column { get; set; }
        [JsonProperty(PropertyName = "WEF_B44AD53AA0424B6E8B2FF5ED7F937CF7_Kanban.Column.Done")]
        public bool Done { get; set; }
        [JsonProperty(PropertyName = "WEF_0642C41BE1F54B67BAED74C800549DA3_Kanban.Column")]
        public string Column2 { get; set; }
        [JsonProperty(PropertyName = "WEF_0642C41BE1F54B67BAED74C800549DA3_Kanban.Column.Done")]
        public bool Done2 { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.TCM.SystemInfo")]
        public string SystemInfo { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class WorkItemUpdates
    {
        public string href { get; set; }
    }

    public class WorkItemRevisions
    {
        public string href { get; set; }
    }

    public class WorkItemHistory
    {
        public string href { get; set; }
    }

    public class Html
    {
        public string href { get; set; }
    }

    public class WorkItemType
    {
        public string href { get; set; }
    }

    public class Fields2
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
        public WorkItemUpdates workItemUpdates { get; set; }
        public WorkItemRevisions workItemRevisions { get; set; }
        public WorkItemHistory workItemHistory { get; set; }
        public Html html { get; set; }
        public WorkItemType workItemType { get; set; }
        public Fields2 fields { get; set; }
    }

    public class WorkItemResponse
    {
        public int id { get; set; }
        public int rev { get; set; }
        public Fields fields { get; set; }
        public Links _links { get; set; }
        public string url { get; set; }
    }

}