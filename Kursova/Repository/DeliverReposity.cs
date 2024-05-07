using DeliveryApp.Data;
using DeliveryApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Repository
{
    public class DeliverRepository : IDeliverReposity
    {
        private readonly OrdersDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeliverRepository(OrdersDBContext context, IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> TakeOrder(int orderId)
        {
            using var transaction = _context.Database.BeginTransaction();
            int waitingStatusId = 1, takenStatusId = 2;
            try
            {
                var order = await _context.Goods
                    .Include(o => o.DeliverOrders)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    throw new InvalidOperationException($"Order with ID {orderId} not found");
                }

                if (order.GoodsStatusId == waitingStatusId)
                {
                    var takenStatus = await _context.GoodsStatus.FirstOrDefaultAsync(s => s.StatusId == takenStatusId);
                    if (takenStatus == null)
                    {
                        throw new InvalidOperationException("Taken status not found");
                    }

                    order.GoodsStatus = takenStatus;

                    if (order.DeliverOrders == null)
                    {
                        order.DeliverOrders = new DeliverOrders();
                    }
                    order.DeliverOrders.DelivererId = GetUserId();
                    order.DeliverOrders.NameDeliverer = await GetEmail();

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                else
                {
                    throw new InvalidOperationException("Order cannot be taken. Invalid status.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                transaction.Rollback();
                return false; 
            }
        }
        public async Task<DeliverOrders> GetDeliverOrders()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            var user = await _userManager.GetUserAsync(principal);

            if (user == null)
                throw new InvalidOperationException("User not found");

            var deliverOrders = await _context.DeliverOrders.FirstOrDefaultAsync(d => d.DelivererId == user.Id);

            return deliverOrders ?? new DeliverOrders { DelivererId = user.Id, NameDeliverer = user.UserName };
        }
        public async Task<bool> ApplyStatus(int orderId, int status)
        {
            using var transaction = _context.Database.BeginTransaction();
            int waitingStatusId = 1;
            try
            {
                var order = await _context.Goods
                    .Include(o => o.DeliverOrders)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    throw new InvalidOperationException($"Order with ID {orderId} not found");
                }
                if (order.GoodsStatusId != waitingStatusId)
                {
                    var takenStatus = await _context.GoodsStatus.FirstOrDefaultAsync(s => s.StatusId == status);
                    if (takenStatus == null)
                    {
                        throw new InvalidOperationException("Some problem with changing");
                    }

                    order.GoodsStatus = takenStatus;
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                else
                {
                    throw new InvalidOperationException("Order cannot be taken. Invalid status.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                transaction.Rollback();
                return false; 
            }
        }
        public async Task<string> GetEmail()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            var user = await _userManager.GetUserAsync(principal);

            if (user == null)
                throw new InvalidOperationException("User not found");

            return user?.Email ?? throw new InvalidOperationException("User email is null");
        }

        public string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}

