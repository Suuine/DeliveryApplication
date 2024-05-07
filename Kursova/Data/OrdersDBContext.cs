using Kursova.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace DeliveryApp.Data
{
    public class OrdersDBContext : IdentityDbContext<ApplicationUser>
    {
        public OrdersDBContext(DbContextOptions<OrdersDBContext> options) : base(options)
        {
        }       
        public DbSet<Asortimet> Asortiment { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Goods> Goods { get; set; }
        public DbSet<GoodsDetail> GoodsDetail { get; set; }
        public DbSet<GoodsStatus> GoodsStatus { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<CartDetail> CartDetail { get; set; }
        public DbSet<DeliverOrders> DeliverOrders { get; set; }
    }
}
//2.7