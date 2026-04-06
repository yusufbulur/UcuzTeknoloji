using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UcuzTeknoloji.Models;

namespace UcuzTeknoloji.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<ProductUpgrade> ProductUpgrades { get; set; }
    }
}