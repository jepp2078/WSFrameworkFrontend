using System.Threading.Tasks;
using System.Web.Mvc;
using WSFrameworkFrontend.Models;
using WSFrameworkFrontend.Services;

namespace WSFrameworkFrontend.Controllers
{
    public class RegUserController : Controller
    {
        private RegUserRESTService service = new RegUserRESTService();
        
        //GET: /RegUserPage/
        public ActionResult RegUserPage()
        {
            return View();
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
                
                return View("RegUserPage",user);
            }
            
        }
    }
}