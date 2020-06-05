using FlourMill_1.Models;
using Microsoft.EntityFrameworkCore;

namespace FlourMill_1.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DataContext()
        {
        }

        public DbSet<Bakery> Bakery { get; set; }
        public DbSet<Administrator> Administrator { get; set; }
        public DbSet<TruckDriver> TruckDriver { get; set; }
        public DbSet<SuperVisor> SuperVisor { get; set; }

        public DbSet<Product> Photo { get; set; }
        public DbSet<Product> Product { get; set; }

        public DbSet<Report> Report { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<AdminRate> AdminRate { get; set; }
        public DbSet<OrderProducts> orderProducts { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }
        public DbSet<ProductRate> ProductRate { get; set; }
    }
}