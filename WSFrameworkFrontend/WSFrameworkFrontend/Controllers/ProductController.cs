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
            if (IsUserLoggedIn())
            {
                List<category> categories = new List<category>();
                IList<CategoryModel> currentCategories = await categoryService.GetAllCategories();
                foreach (var category in currentCategories)
                {
                    category categoryToAdd = new category();
                    categoryToAdd.Id = category.Id;
                    categoryToAdd.name = category.Title;
                    categories.Add(categoryToAdd);
                }

                ViewBag.CategoryList = new SelectList(categories, "ID", "Name");
                return View();
            }
            else
            {
                return Redirect("/login/index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProductCreateViewModel product)
        {
            ShopModel shop = new ShopModel();
            shop = (await shopService.GetOwnShop());
            if(shop == null)
            {
                HttpResponseModel resp = new HttpResponseModel();
                resp.ReasonMessage = "No webshop on account. Create a webshop before creating products!";
                return View("Failure", resp);
            }
            long shopId = shop.Id;
            var response = await productService.CreateProduct(product, shopId);
            if (response != null)
            {
                var responseAfterCreate = await productService.getProduct(response.Id);
                return View("Success", responseAfterCreate);
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
                product.Title = null;
                return View("Create", product);
            }

        }

        [HttpGet]
        public async Task<ActionResult> Edit(long Id)
        {
            if (IsUserLoggedIn())
            {
                var response = await productService.getProduct(Id);
                if (response == null)
                {
                    HttpResponseModel resp = new HttpResponseModel();
                    resp.ReasonMessage = "Product not found.";
                    return View("Failure", resp);
                }
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
                return View("Edit", response);
            }
            else
            {
                return Redirect("/login/index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ProductCreateViewModel productForEdit)
        {
            var response = await productService.EditProduct(productForEdit);
            if (response.IsSuccessStatusCode)
            {
                var responseAfterPut = await productService.getProduct(productForEdit.Id);
                return View("Success", responseAfterPut);
            }
            HttpResponseModel resp = new HttpResponseModel();
            resp.ReasonMessage = response.ReasonPhrase;
            return View("Failure", resp);
        }

        [HttpPost]
        public async Task<ActionResult> Activate(ProductCreateViewModel productForActivation)
        {
            if (productForActivation.IsActive == 0)
            {
                productForActivation.IsActive = 1;
            }
            else
            {
                productForActivation.IsActive = 0;

            }

            var response = await productService.ActivateProduct(productForActivation);
            if (response.IsSuccessStatusCode)
            {
                var responseAfterPut = await productService.getProduct(productForActivation.Id);
                return View("Success", responseAfterPut);
            }
            HttpResponseModel resp = new HttpResponseModel();
            resp.ReasonMessage = response.ReasonPhrase;
            return View("Failure", resp);
        }

        [HttpGet]
        public async Task<ActionResult> Browse()
        {
            if (IsUserLoggedIn())
                return View(await productService.GetOwnProducts());
            else
                return Redirect("/login/index");
        }

        [HttpGet]
        public async Task<ActionResult> Details(long Id)
        {
            if (IsUserLoggedIn())
                return View(await productService.getProduct(Id));
            else
                return Redirect("/login/index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(ProductCreateViewModel product)
        {
            var response = await productService.DeleteProduct(product);
            if (response.IsSuccessStatusCode)
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
                return View("Create");
            }
            else
            {
                HttpResponseModel resp = new HttpResponseModel();
                resp.ReasonMessage = response.ReasonPhrase;
                return View("Failure", resp);
            }

        }

        public bool IsUserLoggedIn()
        {
            if (System.Web.HttpContext.Current.Session["AccessToken"] == null)
            {
                return false;
            }
            return true;
        }
    }
}