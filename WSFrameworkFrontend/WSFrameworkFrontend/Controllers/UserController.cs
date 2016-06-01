using System.Threading.Tasks;
using System.Web.Mvc;
using WSFrameworkFrontend.Models;
using WSFrameworkFrontend.Services;

namespace WSFrameworkFrontend.Controllers
{
    public class UserController : Controller
    {
        private RegUserRESTService service = new RegUserRESTService();

        [HttpGet]
        public ActionResult Create()
        {
            return View("Create");
        }

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