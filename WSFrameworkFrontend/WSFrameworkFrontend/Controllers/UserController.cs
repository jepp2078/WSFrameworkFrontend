using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WSFrameworkFrontend.Models;
using WSFrameworkFrontend.Services;

namespace WSFrameworkFrontend.Controllers
{
    public class UserController : Controller
    {
        private RegUserRESTService service = new RegUserRESTService();
        private LoginRESTService loginService = new LoginRESTService();

        [HttpPost]
        public async Task<ActionResult> Create(RegUserModel user)
        {
            var response = await service.RegisterUser(user);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                UserModel userCreated = new UserModel();
                userCreated.UserName = user.UserName;
                userCreated.Email = user.Email;
                userCreated.PhoneNumber = user.PhoneNumber;

                //Log user in
                LoginUserModel login = new LoginUserModel();
                login.UserName = user.UserName;
                login.Password = user.Password;
                IList<string> tokenIn = await loginService.GenerateToken(login);
                if (tokenIn == null)
                {
                    return View("Success", userCreated);
                }
                System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("AccessToken")
                {
                    Value = tokenIn[0],
                    HttpOnly = true,
                    Expires = DateTime.Now.AddSeconds(Convert.ToDouble(tokenIn[1])) //TODO: Tokens now expire in UTC time
                });

                System.Web.HttpContext.Current.Session["AccessToken"] = tokenIn[0];

                System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("UserName")
                {
                    Value = user.UserName,
                    HttpOnly = true,
                    Expires = DateTime.Now.AddSeconds(Convert.ToDouble(tokenIn[1])) //TODO: Tokens now expire in UTC time
                });

                System.Web.HttpContext.Current.Session["UserName"] = user.UserName;

                return View("Success",userCreated);
            }
            else
            {
                user.Password = null;
                user.ConfirmPassword = null;
                return View("~/Views/Home/Splash.cshtml", user);
            }
            
        }
    }
}