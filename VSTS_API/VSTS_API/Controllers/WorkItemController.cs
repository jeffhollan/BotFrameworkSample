using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using VSTS_API.Models;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;
using TRex.Metadata;

namespace VSTS_API.Controllers
{
    public class WorkItemController : ApiController
    {
       
        [Metadata("Create VSO Item", "Creates an item in a vso instance")]
        [Swashbuckle.Swagger.Annotations.SwaggerResponse(HttpStatusCode.OK, "VSO WorkItem", typeof(WorkItemResponse))]
        [HttpPatch, Route("api/vso/create")]
        public async Task<HttpResponseMessage> Create(WorkItemRequest item)
        {
            //retrieve access token if not provided
            if (String.IsNullOrEmpty(item.access_token))
                item.access_token = await Utils.KeyVault.retrieveSecret(Constants.SecretUri);

            using (var client = new HttpClient())
            {
                JArray patchOperations = new JArray();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", item.access_token);

                var vso_request = new HttpRequestMessage(new HttpMethod("PATCH"),
                    $"https://{item.instance}.visualstudio.com/DefaultCollection/{item.project}/_apis/wit/workitems/{item.getWorkItemTypeURL()}?api-version={Constants.api_version}");

                foreach (var property in item.GetType().GetProperties())
                {
                    if (!WorkItemRequest.pathMappingDictionary.ContainsKey(property.Name) || property.GetValue(item) == null)
                        continue;
                    JObject patchItem = JObject.FromObject(new PatchParameter {
                        op = "add",
                        path = WorkItemRequest.pathMappingDictionary[property.Name],
                        value = property.GetValue(item)
                    });

                    patchOperations.Add(patchItem);
                }
                vso_request.Content = new StringContent(patchOperations.ToString(Newtonsoft.Json.Formatting.None), 
                    Encoding.UTF8, 
                    "application/json-patch+json");

                var res = await client.SendAsync(vso_request);
                var res_with_edit_url = appendEditUrl(JObject.Parse((await res.Content.ReadAsStringAsync())), item);

                return Request.CreateResponse(res_with_edit_url);
            }
        }

        private object appendEditUrl(JObject obj, WorkItemRequest item)
        {
            var api_url = (string)obj["url"];
            api_url = api_url.Replace("DefaultCollection", item.project).Replace("_apis/wit/w", "_w").Replace("Items/", "Items/edit/");
            obj["edit_url"] = api_url;
            return obj;
        }
    }
}