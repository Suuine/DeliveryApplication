using Kursova.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Kursova.Data
{
    public class OrdersDBContext : IdentityDbContext
    {
        public OrdersDBContext(DbContextOptions options) : base(options)
        {
        } 
        public DbSet<OrdersEntity> Order { get; set; }

    }

}
