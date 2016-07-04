using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WSFrameworkFrontend.Helpers;
using WSFrameworkFrontend.Models;

namespace WSFrameworkFrontend.Services
{
    public class ShopRESTService
    {
        private class ShopOut
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string DescriptionFull { get; set; }
        }

        private class ShopUpdate
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string DescriptionFull { get; set; }
            public int? IsActive { get; set; }
        }

        readonly string uri = UriHelper.getUri();

        public async Task<ShopModel> CreateShop(ShopModel shop)
        {
            ShopOut shopOut = new ShopOut();
            shopOut.Title = shop.Title;
            shopOut.Description = shop.Description;
            shopOut.DescriptionFull = shop.DescriptionFull;

            using (HttpClient httpClient = new HttpClient())
            {
                string content = JsonConvert.SerializeObject(shopOut).ToString();
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.PostAsync(uri + "Shops", stringContent);
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var shopResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(shopResponse);

                ShopModel Shop = new ShopModel();
                Shop.Id = Convert.ToInt64(json["Id"]);
                Shop.UserId = json["UserId"].ToString();
                Shop.Title = json["Title"].ToString();
                Shop.Description = json["Description"].ToString();
                Shop.DescriptionFull = json["DescriptionFull"].ToString();
                Shop.Views = Convert.ToInt64(json["Views"]);
                Shop.IsActive = Convert.ToInt32(json["IsActive"]);
                Shop.CreatedAt = DateTime.Parse(json["CreatedAt"].ToString());
                Shop.UpdatedAt = DateTime.Parse(json["UpdatedAt"].ToString());

                return Shop;
            }
        }

        public async Task<HttpResponseMessage> EditShop(ShopModel shop)
        {
            ShopUpdate shopOut = new ShopUpdate();
            shopOut.Title = shop.Title;
            shopOut.Description = shop.Description;
            shopOut.DescriptionFull = shop.DescriptionFull;
            shopOut.IsActive = shop.IsActive;

            using (HttpClient httpClient = new HttpClient())
            {
                string content = JsonConvert.SerializeObject(shopOut).ToString();
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.PutAsync(uri + "Shops/"+shop.Id, stringContent);
                return response;
            }
        }

        public async Task<HttpResponseMessage> DeleteShop(ShopModel shop)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.DeleteAsync(uri + "Shops/" + shop.Id);
                return response;
            }
        }

        public async Task<ShopModel> GetOwnShop()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.GetAsync(uri + "Shops/Own");
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var shopResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(shopResponse);

                ShopModel Shop = new ShopModel();
                Shop.Id = Convert.ToInt64(json["Id"]);
                Shop.UserId = json["UserId"].ToString();
                Shop.Title = json["Title"].ToString();
                Shop.Description = json["Description"].ToString();
                Shop.DescriptionFull = json["DescriptionFull"].ToString();
                Shop.Views = Convert.ToInt64(json["Views"]);
                Shop.IsActive = Convert.ToInt32(json["IsActive"]);
                Shop.CreatedAt = DateTime.Parse(json["CreatedAt"].ToString());
                Shop.UpdatedAt = DateTime.Parse(json["UpdatedAt"].ToString());

                return Shop;
            }
        }

        public async Task<IList<ShopModel>> GetAllShops()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.GetAsync(uri + "Shops");
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var shopResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(shopResponse);
                IList<ShopModel> shops = new List<ShopModel>();
                foreach (var shop in json)
                {
                    ShopModel Shop = new ShopModel();
                    Shop.Id = Convert.ToInt64(shop["Id"]);
                    Shop.UserId = shop["UserId"].ToString();
                    Shop.Title = shop["Title"].ToString();
                    Shop.Description = shop["Description"].ToString();
                    Shop.DescriptionFull = shop["DescriptionFull"].ToString();
                    Shop.Views = Convert.ToInt64(shop["Views"]);
                    Shop.IsActive = Convert.ToInt32(shop["IsActive"]);
                    Shop.CreatedAt = DateTime.Parse(shop["CreatedAt"].ToString());
                    Shop.UpdatedAt = DateTime.Parse(shop["UpdatedAt"].ToString());
                    shops.Add(Shop);
                }

                return shops;
            }
        }
    }
}