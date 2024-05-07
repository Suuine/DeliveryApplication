using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApp.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly ICartRepository _cartRepo;

        public ShoppingCartController(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }
        public async Task<IActionResult> AddItem(int asortimentId, int qty=1, int redirect=0)
        {
            var cartCount = await _cartRepo.AddItem(asortimentId, qty);

            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");        
        }
        public async Task<IActionResult> RemoveItem(int asortimentId)
        {
            var cartCount = await _cartRepo.RemoveItem(asortimentId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepo.GetUserCart();
            return View(cart);
        }
        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartRepo.GetCartItemCount();
            return Ok(cartItem);
        }
        public async Task<IActionResult> Checkout()
        {
            bool isCheckedOut = await _cartRepo.DoCheckout();
            if (!isCheckedOut)
                throw new Exception("Something happen in server side");
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
