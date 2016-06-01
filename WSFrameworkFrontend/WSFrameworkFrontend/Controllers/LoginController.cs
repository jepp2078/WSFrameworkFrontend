using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WSFrameworkFrontend.Models;
using WSFrameworkFrontend.Services;

namespace WSFrameworkFrontend.Controllers
{
    public class LoginController : Controller
    {
        private LoginRESTService service = new LoginRESTService();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(LoginUserModel user)
        {
            IList<string> tokenIn = await service.GenerateToken(user);
            if(tokenIn == null)
            {
                return View("Failure");
            }
            System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("AccessToken")
            {
                Value = tokenIn[0],
                HttpOnly = true,
                Expires = DateTime.Now.AddSeconds(Convert.ToDouble(tokenIn[1])) //TODO: Tokens now expire in UTC time
            });

            System.Web.HttpContext.Current.Session["AccessToken"] = tokenIn[0];
            return Redirect("/home/index");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            if (Request.Cookies["AccessToken"] != null)
            {
                var c = new HttpCookie("AccessToken");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
                System.Web.HttpContext.Current.Session["AccessToken"] = null;
            }
            return Redirect("/login/index");
        }
    }
}