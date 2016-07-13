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

        public async Task<ShopConfigurationModel> CreateShopConfig(ShopConfigurationModel shop)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string content = JsonConvert.SerializeObject(shop).ToString();
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.PostAsync(uri + "ShopConfigurations", stringContent);
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var shopResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(shopResponse);

                ShopConfigurationModel ShopConfig = new ShopConfigurationModel();
                ShopConfig.ShopId = Convert.ToInt64(json["ShopId"]);
                ShopConfig.BgColor = json["BgColor"].ToString();
                ShopConfig.MenuColor = json["MenuColor"].ToString();
                ShopConfig.MenuTextColor = json["MenuTextColor"].ToString();
                ShopConfig.LayoutId = Convert.ToInt32(json["LayoutId"]);

                return ShopConfig;
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

        public async Task<HttpResponseMessage> EditShopConfig(ShopConfigurationModel shopConfig)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string content = JsonConvert.SerializeObject(shopConfig).ToString();
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.PutAsync(uri + "ShopConfigurations/" + shopConfig.ShopId, stringContent);
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

        public async Task<ShopConfigurationModel> GetOwnShopConfig()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.GetAsync(uri + "ShopConfigurations/Own/");
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var shopResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(shopResponse);

                ShopConfigurationModel ShopConfig = new ShopConfigurationModel();
                ShopConfig.ShopId = Convert.ToInt64(json["ShopId"]);
                ShopConfig.BgColor = json["BgColor"].ToString();
                ShopConfig.MenuColor = json["MenuColor"].ToString();
                ShopConfig.MenuTextColor = json["MenuTextColor"].ToString();
                ShopConfig.LayoutId = Convert.ToInt32(json["LayoutId"]);

                return ShopConfig;
            }
        }

        public async Task<ShopConfigurationModel> GetShopConfig(long id)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.GetAsync(uri + "ShopConfigurations/"+id);
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var shopResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(shopResponse);

                ShopConfigurationModel ShopConfig = new ShopConfigurationModel();
                ShopConfig.ShopId = Convert.ToInt64(json["ShopId"]);
                ShopConfig.BgColor = json["BgColor"].ToString();
                ShopConfig.MenuColor = json["MenuColor"].ToString();
                ShopConfig.MenuTextColor = json["MenuTextColor"].ToString();
                ShopConfig.LayoutId = Convert.ToInt32(json["LayoutId"]);

                return ShopConfig;
            }
        }

        public async Task<ShopViewModel> GetOwnFullShop()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.GetAsync(uri + "Shops/Own/Products");
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var shopResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(shopResponse);

                ShopViewModel Shop = new ShopViewModel();
                Shop.Id = Convert.ToInt64(json["Id"]);
                Shop.UserId = json["UserId"].ToString();
                Shop.Title = json["Title"].ToString();
                Shop.Description = json["Description"].ToString();
                Shop.DescriptionFull = json["DescriptionFull"].ToString();
                Shop.Views = Convert.ToInt64(json["Views"]);
                Shop.IsActive = Convert.ToInt32(json["IsActive"]);
                Shop.CreatedAt = DateTime.Parse(json["CreatedAt"].ToString());
                Shop.UpdatedAt = DateTime.Parse(json["UpdatedAt"].ToString());

                //Get list of products
                var Products = ((ProductFullModel[])Newtonsoft.Json.JsonConvert.DeserializeObject(json["Products"].ToString(), typeof(ProductFullModel[])));
                List<ProductViewModel> productList = new List<ProductViewModel>();

                foreach (var product in Products)
                {
                    ProductViewModel Product = new ProductViewModel();
                    Product.Id = product.Id;
                    Product.Title = product.Title;
                    Product.Description = product.Description;
                    Product.DescriptionFull = product.DescriptionFull;
                    Product.Views = product.Views;
                    Product.IsActive = product.IsActive;
                    Product.CreatedAt = product.CreatedAt;
                    Product.UpdatedAt = product.UpdatedAt;
                    Product.ShopId = product.ShopId;
                    Product.Stock = product.Stock;
                    Product.Price = product.Price;

                    Product.Image = product.Images[0];
                    
                    List<long> categoryList = new List<long>();
                    List<string> categoryNameList = new List<string>();

                    var categories = product.Categories;
                    foreach (var category in categories)
                    {
                        categoryList.Add(category.Id);
                        categoryNameList.Add(category.Title);
                    }
                    Product.Category = categoryList;
                    Product.CategoryName = categoryNameList;

                    productList.Add(Product);
                }
                Shop.Products = productList;

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                response = await httpClient.GetAsync(uri + "ShopConfigurations/Own");
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                shopResponse = await response.Content.ReadAsStringAsync();

                Shop.Configuration = ((ShopConfigurationModel)Newtonsoft.Json.JsonConvert.DeserializeObject(shopResponse, typeof(ShopConfigurationModel)));

                return Shop;
            }
        }

        public async Task<ShopViewModel> GetFullShop(long id)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(uri + "Shops/" + id + "/Products");
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var shopResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(shopResponse);

                ShopViewModel Shop = new ShopViewModel();
                Shop.Id = Convert.ToInt64(json["Id"]);
                Shop.UserId = json["UserId"].ToString();
                Shop.Title = json["Title"].ToString();
                Shop.Description = json["Description"].ToString();
                Shop.DescriptionFull = json["DescriptionFull"].ToString();
                Shop.Views = Convert.ToInt64(json["Views"]);
                Shop.IsActive = Convert.ToInt32(json["IsActive"]);
                Shop.CreatedAt = DateTime.Parse(json["CreatedAt"].ToString());
                Shop.UpdatedAt = DateTime.Parse(json["UpdatedAt"].ToString());

                //Get list of products
                var Products = ((ProductFullModel[])Newtonsoft.Json.JsonConvert.DeserializeObject(json["Products"].ToString(), typeof(ProductFullModel[])));
                List<ProductViewModel> productList = new List<ProductViewModel>();

                foreach (var product in Products)
                {
                    ProductViewModel Product = new ProductViewModel();
                    Product.Id = product.Id;
                    Product.Title = product.Title;
                    Product.Description = product.Description;
                    Product.DescriptionFull = product.DescriptionFull;
                    Product.Views = product.Views;
                    Product.IsActive = product.IsActive;
                    Product.CreatedAt = product.CreatedAt;
                    Product.UpdatedAt = product.UpdatedAt;
                    Product.ShopId = product.ShopId;
                    Product.Stock = product.Stock;
                    Product.Price = product.Price;

                    Product.Image = product.Images[0];

                    List<long> categoryList = new List<long>();
                    List<string> categoryNameList = new List<string>();

                    var categories = product.Categories;
                    foreach (var category in categories)
                    {
                        categoryList.Add(category.Id);
                        categoryNameList.Add(category.Title);
                    }
                    Product.Category = categoryList;
                    Product.CategoryName = categoryNameList;

                    productList.Add(Product);
                }
                Shop.Products = productList;

                response = await httpClient.GetAsync(uri + "ShopConfigurations/" + id);
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                shopResponse = await response.Content.ReadAsStringAsync();

                Shop.Configuration = ((ShopConfigurationModel)Newtonsoft.Json.JsonConvert.DeserializeObject(shopResponse, typeof(ShopConfigurationModel)));

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