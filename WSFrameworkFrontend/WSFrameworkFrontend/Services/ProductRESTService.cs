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
    public class ProductRESTService
    {
        readonly string uri = UriHelper.getUri();

        public class CreateProductModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string DescriptionFull { get; set; }
            public long ShopId { get; set; }
            public string Image { get; set; }
            public IList<long> CategoryId { get; set; }
            public int Stock { get; set; }
            public double? Price { get; set; }
        }

        public async Task<ProductModel> CreateProduct(ProductModel product, string ImageUrl, List<long> categoryId, long shopId)
        {
            CreateProductModel productOut = new CreateProductModel();
            productOut.Title = product.Title;
            productOut.Description = product.Description;
            productOut.DescriptionFull = product.DescriptionFull;
            productOut.ShopId = shopId;
            productOut.Image = ImageUrl;
            productOut.CategoryId = categoryId;
            productOut.Stock = product.Stock;
            productOut.Price = product.Price;

            using (HttpClient httpClient = new HttpClient())
            {
                string content = JsonConvert.SerializeObject(productOut).ToString();
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.PostAsync(uri + "Products", stringContent);
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var shopResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(shopResponse);

                ProductModel Product = new ProductModel();
                Product.Id = Convert.ToInt64(json["Id"]);
                Product.Title = json["Title"].ToString();
                Product.Description = json["Description"].ToString();
                Product.DescriptionFull = json["DescriptionFull"].ToString();
                Product.Views = Convert.ToInt64(json["Views"]);
                Product.IsActive = Convert.ToInt32(json["IsActive"]);
                Product.CreatedAt = DateTime.Parse(json["CreatedAt"].ToString());
                Product.UpdatedAt = DateTime.Parse(json["UpdatedAt"].ToString());
                Product.ShopId = Convert.ToInt64(json["ShopId"]);
                Product.Stock = Convert.ToInt32(json["Stock"]);
                Product.Price = Convert.ToDouble(json["Price"]);

                return Product;
            }
        }

        public async Task<IList<ProductModel>> GetOwnProducts()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.GetAsync(uri + "Products/Own");
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var productResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(productResponse);
                List<ProductModel> products = new List<ProductModel>();
                foreach (var product in json)
                {
                    ProductModel Product = new ProductModel();
                    Product.Id = Convert.ToInt64(product["Id"]);
                    Product.Title = product["Title"].ToString();
                    Product.Description = product["Description"].ToString();
                    Product.DescriptionFull = product["DescriptionFull"].ToString();
                    Product.Views = Convert.ToInt64(product["Views"]);
                    Product.IsActive = Convert.ToInt32(product["IsActive"]);
                    Product.CreatedAt = DateTime.Parse(product["CreatedAt"].ToString());
                    Product.UpdatedAt = DateTime.Parse(product["UpdatedAt"].ToString());
                    Product.ShopId = Convert.ToInt64(product["ShopId"]);
                    Product.Stock = Convert.ToInt32(product["Stock"]);
                    Product.Price = Convert.ToDouble(product["Price"]);
                    products.Add(Product);
                }

                return products;
            }
        }
    }
}