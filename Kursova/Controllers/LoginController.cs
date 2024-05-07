using Microsoft.AspNetCore.Mvc;

namespace DeliveryApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
