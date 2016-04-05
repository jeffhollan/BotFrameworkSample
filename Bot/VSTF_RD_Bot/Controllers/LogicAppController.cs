using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VSTF_RD_Bot.Models;

namespace VSTF_RD_Bot.Controllers
{
    public class LogicAppController : ApiController
    {
        [HttpGet, Route("api/vstf/search")]
        public async Task<HttpResponseMessage> SearchVSTF(QueryItem query)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync<QueryItem>(Constants.LogicAppQueryUrl, query);
                return response;
            }
        }

        [HttpPost, Route("api/vstf")]
        public async Task<HttpResponseMessage> CreateVSTF(TFSItem item)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync<TFSItem>(Constants.LogicAppCreateUrl, item);
                return response;
            }
        }

        internal async Task<HttpResponseMessage> ExecuteCommand(Command command)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync<Command>(Constants.LogicAppCommandUrl, command);
                return response;
            }
        }
    }
}
