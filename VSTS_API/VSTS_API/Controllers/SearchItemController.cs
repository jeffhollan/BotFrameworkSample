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
using VSTS_API.Models;

namespace VSTS_API.Controllers
{
    public class SearchItemController : ApiController
    {
        private string username = ConfigurationManager.AppSettings["uname"];
        private string password = ConfigurationManager.AppSettings["pword"];
        private string domain = ConfigurationManager.AppSettings["domain"];

        [HttpPost, Route("api/tfs/search")]
        [Metadata("Search TFS")]
        public HttpResponseMessage SearchItem([FromUri] string requestedCollectionUri, [FromUri] string requestedProject, [FromUri] int count, [FromBody] BasicQuery query)
        {
            string type = Utils.Mappings.GetTFSType(query.type);
            string filter = query.query;

            Uri collectionUri = new Uri(requestedCollectionUri);
            TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(collectionUri, new NetworkCredential(username, password, domain));
            WorkItemStore workItemStore = tpc.GetService<WorkItemStore>();
            WorkItemCollection queryResults = workItemStore.Query(
                "Select [Title], [State], [Id], [Stack Rank] " +
                "From Workitems " +
                "Where [Work Item Type] = '" + type + "' and [State] != 'Removed' and [State] != 'Closed' and " + 
                "[State] != 'Resolved' and [Title] contains '" + filter + "' " +
                "Order By [Stack Rank] Asc"
                );
            var response = (from WorkItem r in queryResults select new SimpleWorkItem {
                Title = r.Title,
                State = r.State,
                id = r.Id,
                AreaPath = r.AreaPath,
                IterationPath = r.IterationPath,
                Type = r.Type.Name
            }).Take(count);
            
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        

    }
    
}
