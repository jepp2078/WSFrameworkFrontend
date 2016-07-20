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
using WSFrameworkFrontend.Controllers;

namespace WSFrameworkFrontend.Services
{
    public class OrderRESTService
    {
        readonly string uri = UriHelper.getUri();

        public async Task<OrderModel> GetOrder(long id)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.GetAsync(uri + "Orders/"+id);
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var orderResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(orderResponse);

                OrderModel Order = new OrderModel();
                Order.Id = Convert.ToInt64(json["Id"]);
                Order.CustomerId = Convert.ToInt64(json["CustomerId"]);
                Order.ShippingAddress = json["ShippingAddress"].ToString();
                Order.BillingAddress = json["BillingAddress"].ToString();
                Order.CreatedAt = DateTime.Parse(json["CreatedAt"].ToString());
                Order.UpdatedAt = DateTime.Parse(json["UpdatedAt"].ToString());
                Order.Status = Convert.ToInt32(json["Status"]);
                Order.ShopId = Convert.ToInt64(json["ShopId"]);
                Order.Amount = Convert.ToDouble(json["Amount"]);

                return Order;
            }
        }

        public async Task<List<OrderModel>> GetOrdersForCustomer(long id)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.GetAsync(uri + "Customers/" + id + "/Orders");
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var orderResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(orderResponse);

                List<OrderModel> orders = new List<OrderModel>();
                foreach (var order in json)
                {
                    OrderModel Order = new OrderModel();
                    Order.Id = Convert.ToInt64(order["Id"]);
                    Order.CustomerId = Convert.ToInt64(order["CustomerId"]);
                    Order.ShippingAddress = order["ShippingAddress"].ToString();
                    Order.BillingAddress = order["BillingAddress"].ToString();
                    Order.CreatedAt = DateTime.Parse(order["CreatedAt"].ToString());
                    Order.UpdatedAt = DateTime.Parse(order["UpdatedAt"].ToString());
                    Order.Status = Convert.ToInt32(order["Status"]);
                    Order.ShopId = Convert.ToInt64(order["ShopId"]);
                    Order.Amount = Convert.ToDouble(order["Amount"]);
                    orders.Add(Order);
                }

                return orders;
            }
        }

        public async Task<OrderProductViewModel> GetOrderFull(long id)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.GetAsync(uri + "Orders/" + id + "/Products");
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var orderResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(orderResponse);

                OrderProductViewModel Order = new OrderProductViewModel();
                Order.Id = Convert.ToInt64(json["Id"]);
                Order.CustomerId = Convert.ToInt64(json["CustomerId"]);
                Order.ShippingAddress = json["ShippingAddress"].ToString();
                Order.BillingAddress = json["BillingAddress"].ToString();
                Order.CreatedAt = DateTime.Parse(json["CreatedAt"].ToString());
                Order.UpdatedAt = DateTime.Parse(json["UpdatedAt"].ToString());
                Order.Status = Convert.ToInt32(json["Status"]);
                Order.ShopId = Convert.ToInt64(json["ShopId"]);
                Order.Amount = Convert.ToDouble(json["Amount"]);


                //Get list of products
                var Products = ((OrderProductsFull[])Newtonsoft.Json.JsonConvert.DeserializeObject(json["Products"].ToString(), typeof(OrderProductsFull[])));
                List<OrderProducts> productList = new List<OrderProducts>();

                foreach (var product in Products)
                {
                    OrderProducts Product = new OrderProducts();
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
                    Product.Quantity = product.Quantity;

                    productList.Add(Product);
                }

                Order.Products = productList;

                return Order;
            }
        }

        public async Task<List<OrderModel>> GetOwnOrders()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.GetAsync(uri + "Orders/Own/");
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var orderResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(orderResponse);
                List<OrderModel> orders = new List<OrderModel>();
                foreach (var order in json)
                {
                    OrderModel Order = new OrderModel();
                    Order.Id = Convert.ToInt64(order["Id"]);
                    Order.CustomerId = Convert.ToInt64(order["CustomerId"]);
                    Order.ShippingAddress = order["ShippingAddress"].ToString();
                    Order.BillingAddress = order["BillingAddress"].ToString();
                    Order.CreatedAt = DateTime.Parse(order["CreatedAt"].ToString());
                    Order.UpdatedAt = DateTime.Parse(order["UpdatedAt"].ToString());
                    Order.Status = Convert.ToInt32(order["Status"]);
                    Order.ShopId = Convert.ToInt64(order["ShopId"]);
                    Order.Amount = Convert.ToDouble(order["Amount"]);
                    orders.Add(Order);
                }

                return orders;
            }
        }

        public async Task<OrderModel> CreateOrder(ShopController.OrderOut order)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string content = JsonConvert.SerializeObject(order).ToString();
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(uri + "Orders", stringContent);
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var customerResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(customerResponse);

                OrderModel Order = new OrderModel();
                Order.Id = Convert.ToInt64(json["Id"]);
                Order.CustomerId = Convert.ToInt64(json["CustomerId"]);
                Order.ShippingAddress = json["ShippingAddress"].ToString();
                Order.BillingAddress = json["BillingAddress"].ToString();
                Order.CreatedAt = DateTime.Parse(json["CreatedAt"].ToString());
                Order.UpdatedAt = DateTime.Parse(json["UpdatedAt"].ToString());
                Order.Status = Convert.ToInt32(json["Status"]);
                Order.ShopId = Convert.ToInt64(json["ShopId"]);
                Order.Amount = Convert.ToDouble(json["Amount"]);

                return Order;
            }
        }

        class OrderUpdate
        {
            public string ShippingAddress { get; set; }
            public string BillingAddress { get; set; }
            public int Status { get; set; }
            public double Amount { get; set; }
        }

        public async Task<HttpResponseMessage> EditOrder(OrderModel order)
        {
            OrderUpdate orderOut = new OrderUpdate();
            orderOut.ShippingAddress = order.ShippingAddress;
            orderOut.BillingAddress = order.BillingAddress;
            orderOut.Status = order.Status;
            orderOut.Amount = order.Amount;

            using (HttpClient httpClient = new HttpClient())
            {
                string content = JsonConvert.SerializeObject(orderOut).ToString();
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.PutAsync(uri + "Orders/" + order.Id, stringContent);
                return response;
            }
        }
    }
}