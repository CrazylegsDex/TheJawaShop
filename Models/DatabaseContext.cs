/*
    This class contains the code for a Database connection
    The connection type will not be defined here.

    Date: 05-03-2023
    Author: Dexter Downey
*/

using Microsoft.EntityFrameworkCore;

namespace TheJawaShop.Models
{
    public class DatabaseContext : DbContext
    {
        // Empty connection configuration to the database
        public DatabaseContext (DbContextOptions<DatabaseContext> options) : base(options) { }

        // Configuration for EFCore to understand the Many-Many relationship
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductOrder>().HasKey(p => new { p.ProductId, p.OrderId });
        }

        // Each Entity class gets its own DbSet
        public DbSet<User> User { get; set; } = default!;
        public DbSet<Product> Product { get; set; } = default!;
        public DbSet<Order> Order { get; set; } = default!;
        public DbSet<ProductOrder> ProductOrder { get; set; } = default!;
    }
}