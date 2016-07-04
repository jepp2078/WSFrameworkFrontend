using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WSFrameworkFrontend.Models;
using WSFrameworkFrontend.Services;

namespace WSFrameworkFrontend.Controllers
{
    public class ProductController : Controller
    {
        private CategoryRESTService categoryService = new CategoryRESTService();
        private ProductRESTService productService = new ProductRESTService();
        private ShopRESTService shopService = new ShopRESTService();

        public class category
        {
            public long Id { get; set; }
            public string name { get; set; }
        }

        public class CreateProductModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string DescriptionFull { get; set; }
            public int Stock { get; set; }
            public Nullable<double> Price { get; set; }
            public string ImageUrl { get; set; }
            public List<long> CategoryId { get; set; }
        }
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            List<category> categories = new List<category>();
            IList<CategoryModel> test = await categoryService.GetAllCategories();
            foreach (var category in test)
            {
                category categoryToAdd = new category();
                categoryToAdd.Id = category.Id;
                categoryToAdd.name = category.Title;
                categories.Add(categoryToAdd);
            }

            ViewBag.CategoryList = new SelectList(categories, "ID", "Name");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateProductModel product)
        {
            ProductModel productOut = new ProductModel();
            productOut.Title = product.Title;
            productOut.Description = product.Description;
            productOut.DescriptionFull = product.DescriptionFull;
            productOut.Stock = product.Stock;
            productOut.Price = product.Price;
            long shopId = (await shopService.GetOwnShop()).Id;
            var response = await productService.CreateProduct(productOut, product.ImageUrl, product.CategoryId, shopId);
            if (response != null)
            {
                return View("Success", response);
            }
            else
            {
                List<category> categories = new List<category>();
                IList<CategoryModel> test = await categoryService.GetAllCategories();
                foreach (var category in test)
                {
                    category categoryToAdd = new category();
                    categoryToAdd.Id = category.Id;
                    categoryToAdd.name = category.Title;
                    categories.Add(categoryToAdd);
                }

                ViewBag.CategoryList = new SelectList(categories, "ID", "Name");
                productOut.Title = null;
                return View("Create", productOut);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Browse()
        {
            return View(await productService.GetOwnProducts());
        }
    }
}