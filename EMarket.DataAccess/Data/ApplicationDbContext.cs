using EMarket.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EMarket.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
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
            new() { Id = 1, Name = "T-Shirt", UnitPrice = 299.00, ImageSource="~/images/OIP.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Sleek black tee: Style redefined. Elevate your look effortlessly! 🔥 #FashionEssential" },
            new() { Id = 2, Name = "Cellphone", UnitPrice = 13999.00, ImageSource="~/images/cellphone.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Unleash limitless power with our latest cellphone innovation!" },
            new() { Id = 3, Name = "Knife", UnitPrice = 240.00, ImageSource="~/images/ec3596459302e2e8e4d586517816a69a.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Unleash precision in the palm of your hand. Elevate your tools with our sleek knife." },
            new() { Id = 4, Name = "Lotion", UnitPrice = 250.00, ImageSource="~/images/lotion.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Indulge in luxury with our hydrating lotion. Elevate your skincare routine effortlessly." },
            new() { Id = 5, Name = "Rubber Shoes", UnitPrice = 5500.00, ImageSource="~/images/rubbershoes.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Step up your game with our stylish rubber shoes. Elevate your look with every stride." },
            new() { Id = 6, Name = "Clean Code", UnitPrice = 2890.00, ImageSource="~/images/cleancode.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Master clean code principles. Robert Martin's essential guide." },
            new() { Id = 7, Name = "Minecraft", UnitPrice = 150.00, ImageSource="~/images/Minecraft.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Immerse in endless adventures. Explore, create, survive. Minecraft awaits!" },
            new() { Id = 8, Name = "Fibre Cloth", UnitPrice = 40.00, ImageSource="~/images/fibrecloth.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Upgrade your cleaning game with our durable fiber cloth." },
            new() { Id = 9, Name = "Goat's Milk", UnitPrice = 380.00, ImageSource="~/images/goatsmilk.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Pure nourishment for your pet. Goat's milk: natural goodness." },
            new() { Id = 10, Name = "14K Gold Necklace", UnitPrice = 21500.00, ImageSource="~/images/necklace.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Elegant luxury, timeless beauty. Elevate your style with 14k gold." },
            new() { Id = 11, Name = "Laptop", UnitPrice = 50000.0, ImageSource="~/images/laptop.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Powerful laptop with high-speed performance. Perfect for work or entertainment on the go." },
            new() { Id = 12, Name = "Smartwatch", UnitPrice = 9999.95, ImageSource="~/images/smartwatch.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Track your fitness, receive notifications, and more, all from your wrist." },
            new() { Id = 13, Name = "Wireless Earbuds", UnitPrice = 3999.95, ImageSource="~/images/wirelessearbuds.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Enjoy crisp sound quality and freedom from wires with these wireless earbuds." },
            new() { Id = 14, Name = "Portable Bluetooth Speaker", UnitPrice = 2499.95, ImageSource="~/images/bluetoothspeaker.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Take your music anywhere with this portable Bluetooth speaker." },
            new() { Id = 15, Name = "Fitness Tracker", UnitPrice = 2995.00, ImageSource="~/images/fitnesstracker.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Monitor your health and track your fitness goals with this sleek fitness tracker." },
            new() { Id = 16, Name = "Coffee Maker", UnitPrice = 6850.50, ImageSource="~/images/coffeemaker.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Brew your favorite coffee just the way you like it." },
            new() { Id = 17, Name = "Electric Toothbrush", UnitPrice = 1999.0, ImageSource="~/images/electrictoothbrush.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Gentle on gums, powerful on plaque." },
            new() { Id = 18, Name = "Digital Camera", UnitPrice = 14560.60, ImageSource="~/images/digitalcamera.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Capture every moment with stunning clarity using this digital camera." },
            new() { Id = 19, Name = "Air Fryer", UnitPrice = 4499.0, ImageSource="~/images/airfryer.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Enjoy healthier cooking without sacrificing flavor with this air fryer." },
            new() { Id = 20, Name = "Portable Power Bank", UnitPrice = 1499.0, ImageSource="~/images/powerbank.jpg", DateCreated = DateTime.Now.AddMonths(-1), Description = "Never run out of battery again with this portable power bank." },
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
            new() { ProductId = 11, CategoryId = 2 },
            new() { ProductId = 12, CategoryId = 2 },
            new() { ProductId = 12, CategoryId = 5 },
            new() { ProductId = 12, CategoryId = 10 },
            new() { ProductId = 13, CategoryId = 2 },
            new() { ProductId = 14, CategoryId = 2 },
            new() { ProductId = 15, CategoryId = 2 },
            new() { ProductId = 15, CategoryId = 5 },
            new() { ProductId = 15, CategoryId = 10 },
            new() { ProductId = 16, CategoryId = 2 },
            new() { ProductId = 17, CategoryId = 2 },
            new() { ProductId = 18, CategoryId = 2 },
            new() { ProductId = 19, CategoryId = 2 },
            new() { ProductId = 19, CategoryId = 3 },
            new() { ProductId = 20, CategoryId = 2 },
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
