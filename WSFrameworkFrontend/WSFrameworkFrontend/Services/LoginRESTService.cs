using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WSFrameworkFrontend.Helpers;
using WSFrameworkFrontend.Models;

namespace WSFrameworkFrontend.Services
{
    public class LoginRESTService
    {
        readonly string uri = UriHelper.getUri();
        private class UserOut
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string grant_type { get; set; }
        }
        public async Task<IList<string>> GenerateToken(LoginUserModel user)
        {
            var baseAddress = uri;
            using (HttpClient httpClient = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("userName", user.UserName),
                    new KeyValuePair<string, string>("password", user.Password),
                    new KeyValuePair<string, string>("grant_type", "password")
                });
                
                var response = await httpClient.PostAsync(uri + "token", content);

                if(response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var tokenResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(tokenResponse);

                var token = json["access_token"].ToString();
                var expires = json["expires_in"].ToString();
                List<string> outToken = new List<string>();
                outToken.Add(token);
                outToken.Add(expires);
                return outToken;
            }
        }
    }
}