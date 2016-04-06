using Microsoft.Bot.Connector;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VSTF_RD_Bot.Controllers
{
    public class LoginController : ApiController
    {
        [HttpGet, Route("api/{userid}/login")]
        public RedirectResult Login(string userid)
        {
            return Redirect(String.Format("https://login.windows.net/common/oauth2/authorize?response_type=code&client_id={0}&redirect_uri={1}&resource={2}", 
                Constants.ADClientId,  HttpUtility.UrlEncode(Constants.apiBasePath + userid + "/authorize"), HttpUtility.UrlEncode("https://graph.windows.net/")));
        }

        [HttpGet, Route("api/{userid}/authorize")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> Authorize(string userid, string code)
        {
            AuthenticationContext ac = new AuthenticationContext("https://login.windows.net/common/oauth2/authorize/");
            ClientCredential cc = new ClientCredential(Constants.ADClientId, Constants.ADClientSecrent);
            AuthenticationResult ar = await ac.AcquireTokenByAuthorizationCodeAsync(code, new Uri(Constants.apiBasePath + userid + "/authorize"), cc);
            if (!String.IsNullOrEmpty(ar.AccessToken))
            {
                var client = new ConnectorClient();
                var getData = await client.Bots.GetUserDataAsync(Constants.botId, userid);
                getData.Data = ar.Serialize();
                var foo = await client.Bots.SetUserDataAsync(Constants.botId, userid, getData);

                //return Request.CreateResponse(foo);
                var response = Request.CreateResponse(HttpStatusCode.Moved);
                response.Headers.Location = new Uri("/loggedin.htm", UriKind.Relative);
                return response;


            }
            else
                return Request.CreateResponse(HttpStatusCode.Unauthorized);

        }
    }
}
