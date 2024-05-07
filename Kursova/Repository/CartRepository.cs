using DeliveryApp.Data;
using DeliveryApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;

namespace DeliveryApp.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly OrdersDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(OrdersDBContext context, IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost]
        public async Task<int> AddItem(int asortimentId, int qty)
        {
            string userId = GetUserId();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged in");

                var cart = await GetCart(userId);
                if (cart == null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _context.ShoppingCart.Add(cart);
                    _context.SaveChanges();
                }

                var cartItem = _context.CartDetail
                    .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.AsortimentId == asortimentId);
                if (cartItem != null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var asortimet = _context.Asortiment.Find(asortimentId);
                    if (asortimet == null)
                    {
                        Console.WriteLine($"Asortiment with ID {asortimentId} not found.");
                    }
                    else
                    {
                        cartItem = new CartDetail
                        {
                            AsortimentId = asortimentId,
                            ShoppingCartId = cart.Id,
                            Quantity = qty,
                            UnitPrice = asortimet.Cost,
                            Asortimet = asortimet,
                        };
                        _context.CartDetail.Add(cartItem);
                    }
                }
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                transaction.Rollback();
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<int> RemoveItem(int asortimentId)
        {
            using var transaction = _context.Database.BeginTransaction();
            string userId = GetUserId();

            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                // cart detail section
                var cartItem = _context.CartDetail
                    .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.AsortimentId == asortimentId);
                if (cartItem is null)
                    throw new Exception("Not items in cart");
                else if (cartItem.Quantity == 1)
                    _context.CartDetail.Remove(cartItem);
                else
                    cartItem.Quantity--;

                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                transaction.Rollback();
            }

            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid userid");
            var shoppingCart = await _context.ShoppingCart
                               .Include(a => a.CartDetails)
                               .ThenInclude(a => a.Asortimet)
                               .ThenInclude(a => a.Category)
                               .Where(a => a.UserId == userId)
                               .FirstOrDefaultAsync();
            return shoppingCart;
        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _context.ShoppingCart.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _context.ShoppingCart
                              join cartDetail in _context.CartDetail
                              on cart.Id equals cartDetail.ShoppingCartId
                              select new { cartDetail.Id }
                        ).ToListAsync();
            return data.Count;
        }
        public async Task<bool> DoCheckout()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {   
                var userId = GetUserId();
                var userEmail = await GetUserEmail(); 

                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartDetail = _context.CartDetail
                     .Include(cd => cd.Asortimet) 
                     .Where(a => a.ShoppingCartId == cart.Id)
                     .ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");

                int waitingStatusId = 1;
                var order = new Goods
                {
                    UserId = userId,
                    NameUser = userEmail,
                    CreatedDate = DateTime.UtcNow,
                    GoodsStatusId = waitingStatusId
                };
                _context.Goods.Add(order);
                _context.SaveChanges();
                foreach (var item in cartDetail)
                {
                    _context.Entry(item).Reference(cd => cd.Asortimet).Load();
                    var orderDetail = new GoodsDetail
                    {
                        Asortimet = item.Asortimet,                        
                        AsortimentId = item.AsortimentId,
                        GoodsId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        
                    };
                    orderDetail.Asortimet.CategoryName = item.Asortimet.Category?.NameCategory;
                    _context.GoodsDetail.Add(orderDetail);
                }
                _context.SaveChanges();
                _context.CartDetail.RemoveRange(cartDetail);
                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<string> GetUserEmail()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid user ID");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            return user.Email;
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }

    }
}
