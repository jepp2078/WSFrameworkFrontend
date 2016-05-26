using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WSFrameworkFrontend.Helpers;
using WSFrameworkFrontend.Models;

namespace WSFrameworkFrontend.Services
{
    public class RegUserRESTService
    {
        readonly string uri = UriHelper.getUri();
        
        public async Task<HttpResponseMessage> RegisterUser(RegUserModel user)
        {

            using (HttpClient httpClient = new HttpClient())
            {
                string content = JsonConvert.SerializeObject(user).ToString();
                var stringContent = new StringContent(content, Encoding.UTF8,"application/json");
                var response = await httpClient.PostAsync(uri + "/Users/Register", stringContent);
                return response;
            }
        }
    }
}