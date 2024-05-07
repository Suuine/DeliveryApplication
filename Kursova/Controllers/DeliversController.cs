using Kursova.Data;
using Kursova.Models;
using Kursova.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Controllers
{
    public class DeliversController : Controller
    {
        private readonly OrdersDBContext _context;
        private readonly DeliverRepository _deliverRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DeliversController(OrdersDBContext context, IHttpContextAccessor httpContextAccessor, DeliverRepository deliverRepository,
        UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _deliverRepository = deliverRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> OrdersDisplay()
        {
            var pendingOrders = await GetAllPendingOrdersAsync();
            return View(pendingOrders);
        }
        [HttpGet]
        public async Task<IActionResult> TakenOrdersDisplay()
        {
            var pendingOrders = await GetAllTakenOrdersAsync();
            return View(pendingOrders);
        }
        private async Task<List<Goods>> GetAllPendingOrdersAsync()
        {
            return await _context.Goods
                .Where(g => g.GoodsStatusId == 1)
                .Include(g => g.GoodsStatus)
                .Include(g => g.OrderDetails)
                .ThenInclude(x => x.Asortimet)
                        .ThenInclude(x => x.Category)
                .ToListAsync();
        }
        private async Task<List<Goods>> GetAllTakenOrdersAsync()
        {
            string deliverId = GetUserId();
            return await _context.Goods
                .Include(g => g.GoodsStatus)
                .Include(g => g.OrderDetails)
                .ThenInclude(x => x.Asortimet)
                        .ThenInclude(x => x.Category)
                .Where(g => g.DeliverOrders.DelivererId == deliverId)
                .ToListAsync();
        }
        [HttpPost]
        public async Task<IActionResult> Take(int goodsId)
        {
            await _deliverRepository.TakeOrder(goodsId);
            return RedirectToAction("OrdersDisplay");
        }
        public async Task<IActionResult> ApplyStatus(int goodsId, int status)
        {
            await _deliverRepository.ApplyStatus(goodsId, status);
            return RedirectToAction("TakenOrdersDisplay");
        }
        public string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
