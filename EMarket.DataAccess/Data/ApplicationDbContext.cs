using EMarket.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EMarket.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        Category[] seedCategories =
        {
            new Category { Id = 1, Name = "Clothing & Apparel", DisplayOrder = 1 },
            new Category { Id = 2, Name = "Electronics", DisplayOrder = 2 },
            new Category { Id = 3, Name = "Home & Kitchen", DisplayOrder = 3 },
            new Category { Id = 4, Name = "Health & Beauty", DisplayOrder = 4 },
            new Category { Id = 5, Name = "Sports & Outdoors", DisplayOrder = 5 },
            new Category { Id = 6, Name = "Books & Media", DisplayOrder = 6 },
            new Category { Id = 7, Name = "Toys & Games", DisplayOrder = 7 },
            new Category { Id = 8, Name = "Automotive", DisplayOrder = 8 },
            new Category { Id = 9, Name = "Pets", DisplayOrder = 9 },
            new Category { Id = 10, Name = "Jewelry & Accessories", DisplayOrder = 10 }
        };

        Product[] seedProducts =
        {
            new Product { Id = 1, Name = "T-Shirt", UnitPrice = 299.00, CategoryId = 1, ImageSource=string.Empty },
            new Product { Id = 2, Name = "Cellphone", UnitPrice = 13999.00, CategoryId = 2, ImageSource=string.Empty },
            new Product { Id = 3, Name = "Knife", UnitPrice = 240.00, CategoryId = 3, ImageSource=string.Empty },
            new Product { Id = 4, Name = "Lotion", UnitPrice = 250.00, CategoryId = 4, ImageSource=string.Empty },
            new Product { Id = 5, Name = "Rubber Shoes", UnitPrice = 5500.00, CategoryId = 5, ImageSource=string.Empty },
            new Product { Id = 6, Name = "Clean Code", UnitPrice = 2890.00, CategoryId = 6, ImageSource=string.Empty },
            new Product { Id = 7, Name = "Minecraft", UnitPrice = 150.00, CategoryId = 7, ImageSource=string.Empty },
            new Product { Id = 8, Name = "Fiber Cloth", UnitPrice = 40.00, CategoryId = 8, ImageSource=string.Empty },
            new Product { Id = 9, Name = "Goat's Milk", UnitPrice = 380.00, CategoryId = 9, ImageSource=string.Empty },
            new Product { Id = 10, Name = "14K Gold Necklace", UnitPrice = 21500.00, CategoryId = 10, ImageSource=string.Empty }
        };

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);

        modelBuilder.Entity<Category>().HasData(seedCategories);

        modelBuilder.Entity<Product>().HasData(seedProducts);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseLazyLoadingProxies();
    }
}
