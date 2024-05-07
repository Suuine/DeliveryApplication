using Microsoft.AspNetCore.Mvc;

namespace DeliveryApp.Controllers
{
    public class AboutController : Controller
    {
        
        public IActionResult About()
        {
            return View();
        }
    }
}
