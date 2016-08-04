using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using VSTS_API.Models;
using System.Threading.Tasks;

namespace VSTS_API.Controllers
{
    public class WorkItemController : ApiController
    {
        [HttpPatch, Route("api/vso/create")]
        public async Task<HttpResponseMessage> Create(WorkItemRequest item)
        {
            using (var client = new HttpClient())
            {
                return Request.CreateResponse(await Utils.KeyVault.retrieveSecret(Constants.SecretUri));
            }
        }
    }
}