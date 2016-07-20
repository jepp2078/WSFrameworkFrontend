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
    public class CustomerController : Controller
    {
        private CustomerRESTService service = new CustomerRESTService();
        private OrderRESTService orderService = new OrderRESTService();

        [HttpGet]
        public async Task<ActionResult> Browse()
        {
            if (IsUserLoggedIn())
                return View(await service.GetOwnCustomers());
            else
                return Redirect("/login/index");
        }

        [HttpGet]
        public async Task<ActionResult> Orders(long id)
        {
            if (IsUserLoggedIn())
                return View(await orderService.GetOrdersForCustomer(id));
            else
                return Redirect("/login/index");
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