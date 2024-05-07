using Microsoft.AspNetCore.Mvc;

namespace DeliveryApp.Controllers
{
    public class UserGoodsController : Controller
    {
        private readonly IUserGoodsPerository _userGoodsRepo;

        public UserGoodsController(IUserGoodsPerository userGoodsRepo)
        {
            _userGoodsRepo = userGoodsRepo;
        }
        public async Task<IActionResult> UserGoods()
        {
            var orders = await _userGoodsRepo.UserGoods();
            return View(orders);
        }
    }
}
