using Authentication.Library;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string emailAddress)
        {
            //todo: email address validation
            _authenticationService.RegisterUser(emailAddress);

            return RedirectToAction("Register");
        }


    }
}