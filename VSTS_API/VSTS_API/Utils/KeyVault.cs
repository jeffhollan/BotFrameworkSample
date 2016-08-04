using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace VSTS_API.Utils
{
    public static class KeyVault
    {
        public async static Task<string> retrieveSecret(string secretUri)
        {
            var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(retrieveAuthToken));

            var sec = await kv.GetSecretAsync(secretUri);

            return sec.Value;
        }

        private static async Task<string> retrieveAuthToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(Constants.AAD_Client_Id,
                        Constants.AAD_Client_Secret);
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }
    }
}