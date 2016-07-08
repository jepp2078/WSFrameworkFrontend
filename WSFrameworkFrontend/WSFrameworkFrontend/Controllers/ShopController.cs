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

        [HttpGet]
        public ActionResult Failure(HttpResponseModel input)
        {
            return View(input);
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

        [HttpGet]
        public async  Task<ActionResult> Browse()
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