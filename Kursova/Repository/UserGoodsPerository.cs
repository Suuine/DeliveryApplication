using DeliveryApp.Data;
using DeliveryApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Repository
{
    public class UserGoodsPerository: IUserGoodsPerository
    {
        private readonly OrdersDBContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserGoodsPerository(OrdersDBContext db,UserManager<ApplicationUser> userManager,
             IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<IEnumerable<Goods>> UserGoods()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");

            var goods = await _db.Goods
                .Include(x => x.GoodsStatus)
                .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Asortimet)
                        .ThenInclude(x => x.Category)  
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return goods;
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
