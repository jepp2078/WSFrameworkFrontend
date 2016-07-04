using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using WSFrameworkFrontend.Helpers;
using WSFrameworkFrontend.Models;

namespace WSFrameworkFrontend.Services
{
    public class CategoryRESTService
    {
        readonly string uri = UriHelper.getUri();

        public async Task<IList<CategoryModel>> GetAllCategories()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Current.Session["AccessToken"].ToString());
                var response = await httpClient.GetAsync(uri + "Categories");
                if (response.IsSuccessStatusCode != true)
                {
                    return null;
                }

                var categoriesResponse = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(categoriesResponse);
                IList<CategoryModel> categories = new List<CategoryModel>();
                foreach (var category in json)
                {
                    CategoryModel Category = new CategoryModel();
                    Category.Id = Convert.ToInt64(category["Id"]);
                    Category.Title = category["Title"].ToString();
                    Category.Description = category["Description"].ToString();
                    Category.Image = category["Image"].ToString();
                    Category.IsActive = Convert.ToInt32(category["IsActive"]);
                    Category.CreatedAt = DateTime.Parse(category["CreatedAt"].ToString());
                    Category.UpdatedAt = DateTime.Parse(category["UpdatedAt"].ToString());
                    categories.Add(Category);
                }

                return categories;
            }
        }
    }
}