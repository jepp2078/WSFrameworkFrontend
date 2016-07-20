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
    public class CustomerRESTService
    {
        readonly string uri = UriHelper.getUri();

        public async Task<List<CustomerModel>> GetOwnCustomers()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());

                var response = await httpClient.GetAsync(uri + "Customers/Own");
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var customerResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(customerResponse);

                List<CustomerModel> Customers = new List<CustomerModel>();
                foreach (var customer in json)
                {
                    CustomerModel Customer = new CustomerModel();
                    Customer.Id = Convert.ToInt64(customer["Id"]);
                    Customer.Name = customer["Name"].ToString();
                    Customer.Phone = customer["Phone"].ToString();
                    Customer.Email = customer["Email"].ToString();
                    Customer.Address = customer["Address"].ToString();
                    Customer.Zip = customer["Zip"].ToString();
                    Customer.City = customer["City"].ToString();
                    Customer.Country = customer["Country"].ToString();
                    Customer.ShopId = Convert.ToInt64(customer["ShopId"]);

                    Customers.Add(Customer);
                }
                return Customers;
            }
        }

        public async Task<CustomerModel> CreateCustomer(CustomerModel customer)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                customer.ShopId = customer.Id;
                string content = JsonConvert.SerializeObject(customer).ToString();
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(uri + "Customers", stringContent);
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var customerResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(customerResponse);

                CustomerModel Customer = new CustomerModel();
                Customer.Id = Convert.ToInt64(json["Id"]);
                Customer.Name = json["Name"].ToString();
                Customer.Phone = json["Phone"].ToString();
                Customer.Email = json["Email"].ToString();
                Customer.Address = json["Address"].ToString();
                Customer.Zip = json["Zip"].ToString();
                Customer.City = json["City"].ToString();
                Customer.Country = json["Country"].ToString();
                Customer.ShopId = Convert.ToInt64(json["ShopId"]);

                return Customer;
            }
        }
    }
}