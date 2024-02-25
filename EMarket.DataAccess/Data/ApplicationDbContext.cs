using EMarket.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EMarket.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Receiver> Receivers { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        Category[] seedCategories =
        [
            new() { Id = 1, Name = "Clothing & Apparel" },
            new() { Id = 2, Name = "Electronics" },
            new() { Id = 3, Name = "Home & Kitchen" },
            new() { Id = 4, Name = "Health & Beauty" },
            new() { Id = 5, Name = "Sports & Outdoors" },
            new() { Id = 6, Name = "Books & Media" },
            new() { Id = 7, Name = "Toys & Games" },
            new() { Id = 8, Name = "Automotive" },
            new() { Id = 9, Name = "Pets" },
            new() { Id = 10, Name = "Jewelry & Accessories" }
        ];

        Product[] seedProducts =
        [
            new() { Id = 1, Name = "T-Shirt", UnitPrice = 299.00, ImageSource="~/images/OIP.jpg", DateCreated = DateTime.Now.AddMonths(-1) },
            new() { Id = 2, Name = "Cellphone", UnitPrice = 13999.00, ImageSource="~/images/cellphone.jpg", DateCreated = DateTime.Now.AddMonths(-1) },
            new() { Id = 3, Name = "Knife", UnitPrice = 240.00, ImageSource="~/images/ec3596459302e2e8e4d586517816a69a.jpg", DateCreated = DateTime.Now.AddMonths(-1) },
            new() { Id = 4, Name = "Lotion", UnitPrice = 250.00, ImageSource="~/images/lotion.jpg", DateCreated = DateTime.Now.AddMonths(-1) },
            new() { Id = 5, Name = "Rubber Shoes", UnitPrice = 5500.00, ImageSource="~/images/rubbershoes.jpg", DateCreated = DateTime.Now.AddMonths(-1) },
            new() { Id = 6, Name = "Clean Code", UnitPrice = 2890.00, ImageSource="~/images/cleancode.jpg", DateCreated = DateTime.Now.AddMonths(-1) },
            new() { Id = 7, Name = "Minecraft", UnitPrice = 150.00, ImageSource="~/images/Minecraft.jpg", DateCreated = DateTime.Now.AddMonths(-1) },
            new() { Id = 8, Name = "Fibre Cloth", UnitPrice = 40.00, ImageSource="~/images/fibrecloth.jpg", DateCreated = DateTime.Now.AddMonths(-1) },
            new() { Id = 9, Name = "Goat's Milk", UnitPrice = 380.00, ImageSource="~/images/goatsmilk.jpg", DateCreated = DateTime.Now.AddMonths(-1) },
            new() { Id = 10, Name = "14K Gold Necklace", UnitPrice = 21500.00, ImageSource="~/images/necklace.jpg", DateCreated = DateTime.Now.AddMonths(-1) }
        ];

        ProductCategory[] seedProductCategories =
        [
            new() { ProductId = 1, CategoryId = 1 },
            new() { ProductId = 2, CategoryId = 2 },
            new() { ProductId = 3, CategoryId = 3 },
            new() { ProductId = 4, CategoryId = 4 },
            new() { ProductId = 5, CategoryId = 5 },
            new() { ProductId = 6, CategoryId = 6 },
            new() { ProductId = 7, CategoryId = 7 },
            new() { ProductId = 7, CategoryId = 6 },
            new() { ProductId = 8, CategoryId = 8 },
            new() { ProductId = 9, CategoryId = 9 },
            new() { ProductId = 10, CategoryId = 10 },
        ];

        modelBuilder.Entity<ProductCategory>()
            .HasKey(pc => new { pc.ProductId, pc.CategoryId });

        modelBuilder.Entity<ProductCategory>()
            .HasIndex(pc => new { pc.ProductId, pc.CategoryId })
            .IsUnique();

        modelBuilder.Entity<Category>().HasData(seedCategories);

        modelBuilder.Entity<Product>().HasData(seedProducts);

        modelBuilder.Entity<ProductCategory>().HasData(seedProductCategories);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseLazyLoadingProxies();
    }
}
