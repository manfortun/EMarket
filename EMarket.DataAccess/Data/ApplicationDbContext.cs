using EMarket.Models;
using Microsoft.EntityFrameworkCore;

namespace EMarket.DataAccess.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
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
            );
    }

}
