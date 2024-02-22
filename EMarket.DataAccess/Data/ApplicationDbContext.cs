using EMarket.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EMarket.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        Category[] seedCategories =
        [
            new() { Id = 1, Name = "Uncategorized", DisplayOrder = 0, DisplayFlag = false },
            new() { Id = 2, Name = "Clothing & Apparel", DisplayOrder = 1, DisplayFlag = true },
            new() { Id = 3, Name = "Electronics", DisplayOrder = 2, DisplayFlag = true },
            new() { Id = 4, Name = "Home & Kitchen", DisplayOrder = 3, DisplayFlag = true },
            new() { Id = 5, Name = "Health & Beauty", DisplayOrder = 4, DisplayFlag = true },
            new() { Id = 6, Name = "Sports & Outdoors", DisplayOrder = 5, DisplayFlag = true },
            new() { Id = 7, Name = "Books & Media", DisplayOrder = 6, DisplayFlag = true },
            new() { Id = 8, Name = "Toys & Games", DisplayOrder = 7, DisplayFlag = true },
            new() { Id = 9, Name = "Automotive", DisplayOrder = 8, DisplayFlag = true },
            new() { Id = 10, Name = "Pets", DisplayOrder = 9, DisplayFlag = true },
            new() { Id = 11, Name = "Jewelry & Accessories", DisplayOrder = 10, DisplayFlag = true }
        ];

        Product[] seedProducts =
        [
            new() { Id = 1, Name = "T-Shirt", UnitPrice = 299.00, CategoryId = 2, ImageSource="~/images/OIP.jpg" },
            new() { Id = 2, Name = "Cellphone", UnitPrice = 13999.00, CategoryId = 3, ImageSource="~/images/cellphone.jpg" },
            new() { Id = 3, Name = "Knife", UnitPrice = 240.00, CategoryId = 4, ImageSource="~/images/ec3596459302e2e8e4d586517816a69a.jpg" },
            new() { Id = 4, Name = "Lotion", UnitPrice = 250.00, CategoryId = 5, ImageSource="~/images/lotion.jpg" },
            new() { Id = 5, Name = "Rubber Shoes", UnitPrice = 5500.00, CategoryId = 6, ImageSource="~/images/rubbershoes.jpg" },
            new() { Id = 6, Name = "Clean Code", UnitPrice = 2890.00, CategoryId = 7, ImageSource="~/images/cleancode.jpg" },
            new() { Id = 7, Name = "Minecraft", UnitPrice = 150.00, CategoryId = 8, ImageSource="~/images/Minecraft.jpg" },
            new() { Id = 8, Name = "Fibre Cloth", UnitPrice = 40.00, CategoryId = 9, ImageSource="~/images/fibrecloth.jpg" },
            new() { Id = 9, Name = "Goat's Milk", UnitPrice = 380.00, CategoryId = 10, ImageSource="~/images/goatsmilk.jpg" },
            new() { Id = 10, Name = "14K Gold Necklace", UnitPrice = 21500.00, CategoryId = 11, ImageSource="~/images/necklace.jpg" }
        ];

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Category>().HasData(seedCategories);

        modelBuilder.Entity<Product>().HasData(seedProducts);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseLazyLoadingProxies();
    }

    public override int SaveChanges()
    {
        var deletedCategories = ChangeTracker.Entries<Category>()
            .Where(e => e.State == EntityState.Deleted)
            .Select(e => e.Entity.Id)
            .ToList();

        if (deletedCategories.Any())
        {
            var affectedProducts = Products.Where(p => deletedCategories.Contains(p.CategoryId)).ToList();
            foreach (var product in affectedProducts)
            {
                product.CategoryId = 1;
            }
        }
        return base.SaveChanges();
    }
}
