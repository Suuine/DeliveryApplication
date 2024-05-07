using Kursova.Data;
using Kursova.Repository;
using Microsoft.AspNetCore.Mvc;
namespace DeliveryApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly OrdersDBContext _context;
        private readonly IAdminRepository _adminRepository;

        public AdminController(OrdersDBContext context, IAdminRepository adminRepository)
        {
            _context = context;
            _adminRepository = adminRepository;
        }

        public async Task<IActionResult> AllOrders()
        {
            var allOrders = await _adminRepository.GetGoodsAsync();
            return View(allOrders);
        }

        public async Task<IActionResult> AllUsers()
        {
            var result = await _adminRepository.DisplayUsers();
            return View("AllUsers", result);
        }
        [HttpPost]
        public async Task<IActionResult> Find(DateTime selectedDateTime1, DateTime selectedDateTime2)
        {
            var allOrders = await _adminRepository.GetGoodsInDateRange(selectedDateTime1, selectedDateTime2);
            return RedirectToAction("Statistic", new { dateTime1 = selectedDateTime1, dateTime2 = selectedDateTime2 });
        }
        public async Task<IActionResult> Statistic(DateTime dateTime1, DateTime dateTime2)
        {
            var allOrders = await _adminRepository.GetGoodsInDateRange(dateTime1, dateTime2);
            return View(allOrders);
        }


    }
}
