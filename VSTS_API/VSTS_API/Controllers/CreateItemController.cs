using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TRex.Metadata;
using VSTS_API.App_Start;
using VSTS_API.Models;

namespace VSTF_API.Controllers
{
    public class CreateItemController : ApiController
    {
        private string username = ConfigurationManager.AppSettings["uname"];
        private string password = ConfigurationManager.AppSettings["pword"];
        private string domain = ConfigurationManager.AppSettings["domain"];
        [Metadata(friendlyName: "Create New Item", description: "Create New Item in VSTS")]
        [Swashbuckle.Swagger.Annotations.SwaggerResponse(HttpStatusCode.OK, "New Work Item", typeof(SimpleWorkItem))]
        [HttpPost, Route("api/tfs/create")]
        public HttpResponseMessage CreateItem([FromUri] string requestedCollectionUri, [FromUri] string requestedProject, [FromBody] SimpleWorkItem requestedWorkItem)
        {
             
            Uri collectionUri = new Uri(requestedCollectionUri);
            TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(collectionUri, new NetworkCredential(username, password, domain));
            WorkItemStore workItemStore = tpc.GetService<WorkItemStore>();
            Project teamProject = workItemStore.Projects[requestedProject];
            WorkItemType workItemType = teamProject.WorkItemTypes[Utils.GetTFSType(requestedWorkItem.Type)];

            // Create the work item. 
            WorkItem newWorkItem = new WorkItem(workItemType)
            {
                // The title is generally the only required field that doesn’t have a default value. 
                // You must set it, or you can’t save the work item. If you’re working with another
                // type of work item, there may be other fields that you’ll have to set.
                Title = requestedWorkItem.Title,
                AreaPath = requestedWorkItem.AreaPath,
                State = requestedWorkItem.State,
                IterationPath = requestedWorkItem.IterationPath,

            };
            RelatedLink parent = new RelatedLink(workItemStore.WorkItemLinkTypes.LinkTypeEnds[requestedWorkItem.LinkType], requestedWorkItem.LinkId);
            newWorkItem.Links.Add(parent);
            newWorkItem.Save();
            requestedWorkItem.id = newWorkItem.Id;
            return Request.CreateResponse<SimpleWorkItem>(HttpStatusCode.OK, requestedWorkItem);

        }

    }
}
