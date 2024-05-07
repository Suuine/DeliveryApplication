using Microsoft.AspNetCore.Mvc;

namespace Kursova.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
