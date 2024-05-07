using DeliveryApp.Data;
using Kursova.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly OrdersDBContext _context;
        public AdminRepository(OrdersDBContext context)
        {
            _context = context;
        }

        public async Task<List<Goods>> GetGoodsInDateRange(DateTime dateTime1, DateTime dateTime2)
        {
            return await _context.Goods
                .Where(o => o.CreatedDate.HasValue
                    && o.CreatedDate.Value >= dateTime1
                    && o.CreatedDate.Value <= dateTime2)
                .Include(g => g.GoodsStatus)
                .Include(g => g.DeliverOrders)
                .Include(g => g.OrderDetails)
                    .ThenInclude(x => x.Asortimet)
                    .ThenInclude(x => x.Category)
                .ToListAsync();
        }


        public async Task<List<Goods>> GetGoodsAsync()
        {
            return await _context.Goods
                              .Include(g => g.GoodsStatus)
                              .Include(g => g.DeliverOrders)
                              .Include(g => g.OrderDetails)
                                  .ThenInclude(x => x.Asortimet)
                                  .ThenInclude(x => x.Category)
                              .ToListAsync();
        }

        public async Task<List<UserViewModel>> DisplayUsers()
        {
            var users = await GetUsersAsync();

            var userViewModels = users.Select(u => new UserViewModel
            {
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName
            }).ToList();
            return userViewModels;
        }

        public async Task<List<ApplicationUser>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
