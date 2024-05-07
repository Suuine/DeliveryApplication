using Microsoft.AspNetCore.Mvc;

namespace Kursova.Controllers
{
    public class AboutController : Controller
    {
        
        public IActionResult About()
        {
            return View();
        }
    }
}
