using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VSTS_API.App_Start;
using VSTS_API.Models;

namespace VSTS_API.Controllers
{
    public class ExecuteQueryController : ApiController
    {
        private string username = ConfigurationManager.AppSettings["uname"];
        private string password = ConfigurationManager.AppSettings["pword"];
        private string domain = ConfigurationManager.AppSettings["domain"];


        public HttpResponseMessage getTopItems([FromUri] string requestedCollectionUri, [FromUri] string requestedProject, [FromUri] int count, [FromUri] string query)
        {
            Uri collectionUri = new Uri(requestedCollectionUri);
            TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(collectionUri, new NetworkCredential(username, password, domain));
            WorkItemStore workItemStore = tpc.GetService<WorkItemStore>();
            WorkItemCollection queryResults = workItemStore.Query(query);
            var response = (from WorkItem r in queryResults
                            select new SimpleWorkItem
                            {
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

    public class ExecuteRequest
    {
        public string query { get; set; }
    }
}
