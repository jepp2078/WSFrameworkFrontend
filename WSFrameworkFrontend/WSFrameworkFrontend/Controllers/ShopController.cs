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
    public class ShopController : Controller
    {
        private ShopRESTService service = new ShopRESTService();
        private ProductRESTService productService = new ProductRESTService();

        [HttpGet]
        public ActionResult Failure(HttpResponseModel input)
        {
            return View(input);
        }

        class CartProduct
        {
            public long id { get; set; }
            public int amount { get; set; }
        }

        [HttpPost]
        public int AddProductToCart(long id, int amount)
        {
            CartProduct productToAdd = new CartProduct();
            productToAdd.id = id;
            productToAdd.amount = amount;

            if (Session["cart"] == null)
            {
                List<CartProduct> cartList = new List<CartProduct>();
                cartList.Add(productToAdd);
                Session.Add("cart", cartList);
            }
            else
            {
                List<CartProduct> cartList = new List<CartProduct>();
                cartList = (List<CartProduct>) Session["cart"];
                if(cartList.Find(p => p.id == id) != null)
                {
                    int amountOld = cartList.Find(p => p.id == id).amount;
                    productToAdd.amount = amountOld + amount;
                    cartList.RemoveAt(cartList.FindIndex(p => p.id == id));
                }
                cartList.Add(productToAdd);
                Session["cart"] = cartList;
            }
            return ((List<CartProduct>)Session["cart"]).Count;
        }

        [HttpPost]
        public int RemoveProductFromCart(long id, int amount)
        {
            CartProduct productToUpdate = new CartProduct();
            productToUpdate.id = id;
            productToUpdate.amount = amount;

            List<CartProduct> cartList = new List<CartProduct>();
            cartList = (List<CartProduct>)Session["cart"];
            if (cartList.Find(p => p.id == id) != null)
            {
                int amountOld = cartList.Find(p => p.id == id).amount;
                productToUpdate.amount = amountOld - amount;
                cartList.RemoveAt(cartList.FindIndex(p => p.id == id));
            }
            if(productToUpdate.amount > 0)
            {
                cartList.Add(productToUpdate);
            }
            if(cartList.Count == 0)
            {
                Session["cart"] = null;
                return 0;
            }
            else
            {
                Session["cart"] = cartList;
            }

            ViewBag.ProductsInCart = Session["cart"] != null ? ((List<CartProduct>)Session["cart"]).Count : 0;
            return 1;
        }

        [HttpGet]
        public async Task<ActionResult> ViewShop(long id)
        {
            var response = await service.GetFullShop(id);
            if (response == null)
            {
                HttpResponseModel resp = new HttpResponseModel();
                resp.ReasonMessage = "Webshop doesn't exsist or isn't configured properly.";
                return View("Failure", resp);
            }
            ViewBag.ProductsInCart = Session["cart"] != null ? ((List<CartProduct>)Session["cart"]).Count : 0;
            return View(response);
        }

        [HttpGet]
        public async Task<ActionResult> ViewCart()
        {
            List<CartProduct> cart = ((List<CartProduct>)Session["cart"]);
            if(cart == null)
            {
                HttpResponseModel resp = new HttpResponseModel();
                resp.ReasonMessage = "No products in cart.";
                return View("Failure", resp);
            }
            List<ProductViewModel> productsInCart = new List<ProductViewModel>();
            double? total = 0;
            foreach (var product in cart)
            {
                ProductViewModel productTemp = await productService.getProduct(product.id);
                productTemp.Stock = product.amount;
                total += productTemp.Price * productTemp.Stock;
                productsInCart.Add(productTemp);
            }

            if (productsInCart.Count > 0)
            {
                var Configuration = await service.GetShopConfig(productsInCart[0].ShopId);
                ViewBag.BgColor = Configuration.BgColor;
                ViewBag.MenuColor = Configuration.MenuColor;
                ViewBag.MenuTextColor = Configuration.MenuTextColor;
                ViewBag.ProductsInCart = Session["cart"] != null ? ((List<CartProduct>)Session["cart"]).Count : 0;
                ViewBag.TotalPrice = total;
                return View(productsInCart);
            }
            return null;
        }

        [HttpGet]
        public async Task<ActionResult> ViewOwnShop()
        {
            if (IsUserLoggedIn())
            {
                var response = await service.GetOwnFullShop();
                if (response == null)
                {
                    HttpResponseModel resp = new HttpResponseModel();
                    resp.ReasonMessage = "Webshop doesn't exsist or isn't configured properly.";
                    return View("Failure", resp);
                }
                ViewBag.ProductsInCart = Session["cart"] != null ? ((List<CartProduct>)Session["cart"]).Count : 0;
                return View("ViewShop", response);
            }
            else
            {
                return Redirect("/login/index");
            }
        }

        [HttpGet]
        public async Task<ActionResult> ViewShopProduct(long id)
        {
            ShopProductViewModel model = new ShopProductViewModel();
            ProductViewModel product = await productService.getProduct(id);
            if(product == null)
            {
                HttpResponseModel resp = new HttpResponseModel();
                resp.ReasonMessage = "Product ID not present in database.";
                return View("Failure", resp);
            }
            model.Id = product.Id;
            model.Title = product.Title;
            model.Description = product.Description;
            model.DescriptionFull = product.DescriptionFull;
            model.Views = product.Views;
            model.IsActive = product.IsActive;
            model.CreatedAt = product.CreatedAt;
            model.UpdatedAt = product.UpdatedAt;
            model.ShopId = product.ShopId;
            model.Stock = product.Stock;
            model.Price = product.Price;
            model.Image = product.Image;
            model.Category = product.Category;
            model.CategoryName = product.CategoryName;
            model.Configuration = await service.GetShopConfig(product.ShopId);

            ViewBag.ProductsInCart = Session["cart"] != null ? ((List<CartProduct>)Session["cart"]).Count : 0;
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            if (IsUserLoggedIn())
            {
                var response = await service.GetOwnShop();
                if (response == null)
                {
                    return View();
                }
                HttpResponseModel resp = new HttpResponseModel();
                resp.ReasonMessage = "Webshop already present on account. Delete old webshop to create a new one.";
                return View("Failure", resp);
            }
            else
            {
                return Redirect("/login/index");
            }
        }
        public class layout
        {
            public long Id { get; set; }
            public string name { get; set; }
        }
        [HttpGet]
        public async Task<ActionResult> CreateConfig()
        {
            if (IsUserLoggedIn())
            {
                var response = await service.GetOwnShopConfig();
                if (response == null)
                {
                    List<layout> layouts = new List<layout>();
                    layouts.Add(new layout { Id = 0, name = "Layout 1" });
                    layouts.Add(new layout { Id = 1, name = "Layout 2" });
                    ViewBag.LayoutList = new SelectList(layouts, "ID", "Name");

                    return View();
                }
                else
                {
                    return Redirect("EditConfig");
                }
            }
            else
            {
                return Redirect("/login/index");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Browse()
        {
            if (IsUserLoggedIn())
                return View(await service.GetAllShops());
            else
                return Redirect("/login/index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            if (IsUserLoggedIn())
            {
                var response = await service.GetOwnShop();
                if (response == null)
                {
                    HttpResponseModel resp = new HttpResponseModel();
                    resp.ReasonMessage = "No webshop present on account.";
                    return View("Failure", resp);
                }
                return View("Edit", response);
            }
            else
            {
                return Redirect("/login/index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ShopModel shopForEdit)
        {
            var response = await service.EditShop(shopForEdit);
            if (response.IsSuccessStatusCode)
            {
                var responseAfterPut = await service.GetOwnShop();
                return View("Success", responseAfterPut);
            }
            HttpResponseModel resp = new HttpResponseModel();
            resp.ReasonMessage = response.ReasonPhrase;
            return View("Failure", resp);
        }

        [HttpGet]
        public async Task<ActionResult> EditConfig()
        {
            if (IsUserLoggedIn())
            {
                var response = await service.GetOwnShopConfig();
                if (response == null)
                {
                    HttpResponseModel resp = new HttpResponseModel();
                    resp.ReasonMessage = "No webshop present on account.";
                    return View("Failure", resp);
                }

                List<layout> layouts = new List<layout>();
                layouts.Add(new layout { Id = 0, name = "Layout 1" });
                layouts.Add(new layout { Id = 1, name = "Layout 2" });
                layouts.Add(new layout { Id = 2, name = "Layout 3" });
                ViewBag.LayoutList = new SelectList(layouts, "ID", "Name");
                return View("EditConfig", response);
            }
            else
            {
                return Redirect("/login/index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditConfig(ShopConfigurationModel shopConfigForEdit)
        {
            var response = await service.EditShopConfig(shopConfigForEdit);
            if (response.IsSuccessStatusCode)
            {
                var responseAfterPut = await service.GetOwnShopConfig();
                return View("ConfigSuccess", responseAfterPut);
            }
            HttpResponseModel resp = new HttpResponseModel();
            resp.ReasonMessage = response.ReasonPhrase;
            return View("Failure", resp);
        }

        [HttpPost]
        public async Task<ActionResult> Activate(ShopModel shopForEdit)
        {
            if(shopForEdit.IsActive == 0)
            {
                shopForEdit.IsActive = 1;
            }
            else
            {
                shopForEdit.IsActive = 0;

            }

            var response = await service.EditShop(shopForEdit);
            if (response.IsSuccessStatusCode)
            {
                var responseAfterPut = await service.GetOwnShop();
                return View("Success", responseAfterPut);
            }
            HttpResponseModel resp = new HttpResponseModel();
            resp.ReasonMessage = response.ReasonPhrase;
            return View("Failure", resp);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ShopModel shop)
        {
            var response = await service.CreateShop(shop);
            if (response != null)
            {
                return View("Success", response);
            }
            else
            {
                shop.Title = null;
                return View("Create", shop);
            }

        }

        [HttpPost]
        public async Task<ActionResult> CreateConfig(ShopConfigurationModel shopConfig)
        {
            ShopModel shop = await service.GetOwnShop();
            if (shop == null)
            {
                HttpResponseModel resp = new HttpResponseModel();
                resp.ReasonMessage = "No webshop present on account.";
                return View("Failure", resp);
            }
            shopConfig.ShopId = shop.Id;
            var response = await service.CreateShopConfig(shopConfig);
            if (response != null)
            {
                return View("ConfigSuccess", response);
            }
            else
            {
                HttpResponseModel resp = new HttpResponseModel();
                resp.ReasonMessage = "Something went wrong!";
                return View("Failure", resp);
            }

        }

        [HttpPost]
        public async Task<ActionResult> Delete(ShopModel shop)
        {
            var response = await service.DeleteShop(shop);
            if (response.IsSuccessStatusCode)
            {
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