using DeliveryApp.Models;

namespace DeliveryApp
{
    public interface ICartRepository
    {
        Task<int> AddItem(int asortimentId, int qty);
        Task<int> RemoveItem(int asortimentId);
        Task<ShoppingCart> GetUserCart();
        Task<int> GetCartItemCount(string userId = "");
        Task<ShoppingCart> GetCart(string userId);
        Task<bool> DoCheckout();
    }
}