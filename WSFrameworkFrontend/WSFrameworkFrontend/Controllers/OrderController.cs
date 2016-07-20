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
    public class OrderController : Controller
    {
        private OrderRESTService service = new OrderRESTService();

        [HttpGet]
        public async Task<ActionResult> Browse()
        {
            if (IsUserLoggedIn())
                return View(await service.GetOwnOrders());
            else
                return Redirect("/login/index");
        }

        class OrderStatus
        {
            public int Status { get; set; }
            public string Name { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(long Id)
        {
            if (IsUserLoggedIn())
            {
                var response = await service.GetOrder(Id);
                if (response == null)
                {
                    HttpResponseModel resp = new HttpResponseModel();
                    resp.ReasonMessage = "Order not found.";
                    return View("Failure", resp);
                }
                IList<OrderStatus> statusOptions = new List<OrderStatus>();
                statusOptions.Add(new OrderStatus(){ Name = "Pending" , Status = 0});
                statusOptions.Add(new OrderStatus() { Name = "Completed", Status = 1 });

                ViewBag.OrderStatusList = new SelectList(statusOptions, "Status", "Name");
                return View("Edit", response);
            }
            else
            {
                return Redirect("/login/index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(OrderModel order)
        {
            var response = await service.EditOrder(order);
            if (response.IsSuccessStatusCode)
            {
                var responseAfterPut = await service.GetOrder(order.Id);
                return View("Success", responseAfterPut);
            }
            HttpResponseModel resp = new HttpResponseModel();
            resp.ReasonMessage = response.ReasonPhrase;
            return View("Failure", resp);
        }

        [HttpGet]
        public async Task<ActionResult> Details(long Id)
        {
            if (IsUserLoggedIn())
                return View(await service.GetOrderFull(Id));
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