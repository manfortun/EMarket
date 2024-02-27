﻿// <auto-generated />
using System;
using EMarket.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EMarket.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EMarket.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Clothing & Apparel"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Electronics"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Home & Kitchen"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Health & Beauty"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Sports & Outdoors"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Books & Media"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Toys & Games"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Automotive"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Pets"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Jewelry & Accessories"
                        });
                });

            modelBuilder.Entity("EMarket.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ImageSource")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(1974),
                            Description = "Sleek black tee: Style redefined. Elevate your look effortlessly! 🔥 #FashionEssential",
                            ImageSource = "~/images/OIP.jpg",
                            Name = "T-Shirt",
                            UnitPrice = 299.0
                        },
                        new
                        {
                            Id = 2,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(1981),
                            Description = "Unleash limitless power with our latest cellphone innovation!",
                            ImageSource = "~/images/cellphone.jpg",
                            Name = "Cellphone",
                            UnitPrice = 13999.0
                        },
                        new
                        {
                            Id = 3,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(1983),
                            Description = "Unleash precision in the palm of your hand. Elevate your tools with our sleek knife.",
                            ImageSource = "~/images/ec3596459302e2e8e4d586517816a69a.jpg",
                            Name = "Knife",
                            UnitPrice = 240.0
                        },
                        new
                        {
                            Id = 4,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(1985),
                            Description = "Indulge in luxury with our hydrating lotion. Elevate your skincare routine effortlessly.",
                            ImageSource = "~/images/lotion.jpg",
                            Name = "Lotion",
                            UnitPrice = 250.0
                        },
                        new
                        {
                            Id = 5,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(1987),
                            Description = "Step up your game with our stylish rubber shoes. Elevate your look with every stride.",
                            ImageSource = "~/images/rubbershoes.jpg",
                            Name = "Rubber Shoes",
                            UnitPrice = 5500.0
                        },
                        new
                        {
                            Id = 6,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(1989),
                            Description = "Master clean code principles. Robert Martin's essential guide.",
                            ImageSource = "~/images/cleancode.jpg",
                            Name = "Clean Code",
                            UnitPrice = 2890.0
                        },
                        new
                        {
                            Id = 7,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(1990),
                            Description = "Immerse in endless adventures. Explore, create, survive. Minecraft awaits!",
                            ImageSource = "~/images/Minecraft.jpg",
                            Name = "Minecraft",
                            UnitPrice = 150.0
                        },
                        new
                        {
                            Id = 8,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(1992),
                            Description = "Upgrade your cleaning game with our durable fiber cloth.",
                            ImageSource = "~/images/fibrecloth.jpg",
                            Name = "Fibre Cloth",
                            UnitPrice = 40.0
                        },
                        new
                        {
                            Id = 9,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(1994),
                            Description = "Pure nourishment for your pet. Goat's milk: natural goodness.",
                            ImageSource = "~/images/goatsmilk.jpg",
                            Name = "Goat's Milk",
                            UnitPrice = 380.0
                        },
                        new
                        {
                            Id = 10,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(1996),
                            Description = "Elegant luxury, timeless beauty. Elevate your style with 14k gold.",
                            ImageSource = "~/images/necklace.jpg",
                            Name = "14K Gold Necklace",
                            UnitPrice = 21500.0
                        },
                        new
                        {
                            Id = 11,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(2025),
                            Description = "Powerful laptop with high-speed performance. Perfect for work or entertainment on the go.",
                            ImageSource = "~/images/laptop.jpg",
                            Name = "Laptop",
                            UnitPrice = 50000.0
                        },
                        new
                        {
                            Id = 12,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(2027),
                            Description = "Track your fitness, receive notifications, and more, all from your wrist.",
                            ImageSource = "~/images/smartwatch.jpg",
                            Name = "Smartwatch",
                            UnitPrice = 9999.9500000000007
                        },
                        new
                        {
                            Id = 13,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(2029),
                            Description = "Enjoy crisp sound quality and freedom from wires with these wireless earbuds.",
                            ImageSource = "~/images/wirelessearbuds.jpg",
                            Name = "Wireless Earbuds",
                            UnitPrice = 3999.9499999999998
                        },
                        new
                        {
                            Id = 14,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(2031),
                            Description = "Take your music anywhere with this portable Bluetooth speaker.",
                            ImageSource = "~/images/bluetoothspeaker.jpg",
                            Name = "Portable Bluetooth Speaker",
                            UnitPrice = 2499.9499999999998
                        },
                        new
                        {
                            Id = 15,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(2032),
                            Description = "Monitor your health and track your fitness goals with this sleek fitness tracker.",
                            ImageSource = "~/images/fitnesstracker.jpg",
                            Name = "Fitness Tracker",
                            UnitPrice = 2995.0
                        },
                        new
                        {
                            Id = 16,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(2034),
                            Description = "Brew your favorite coffee just the way you like it.",
                            ImageSource = "~/images/coffeemaker.jpg",
                            Name = "Coffee Maker",
                            UnitPrice = 6850.5
                        },
                        new
                        {
                            Id = 17,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(2036),
                            Description = "Gentle on gums, powerful on plaque.",
                            ImageSource = "~/images/electrictoothbrush.jpg",
                            Name = "Electric Toothbrush",
                            UnitPrice = 1999.0
                        },
                        new
                        {
                            Id = 18,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(2038),
                            Description = "Capture every moment with stunning clarity using this digital camera.",
                            ImageSource = "~/images/digitalcamera.jpg",
                            Name = "Digital Camera",
                            UnitPrice = 14560.6
                        },
                        new
                        {
                            Id = 19,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(2039),
                            Description = "Enjoy healthier cooking without sacrificing flavor with this air fryer.",
                            ImageSource = "~/images/airfryer.jpg",
                            Name = "Air Fryer",
                            UnitPrice = 4499.0
                        },
                        new
                        {
                            Id = 20,
                            DateCreated = new DateTime(2024, 1, 27, 23, 16, 8, 530, DateTimeKind.Local).AddTicks(2041),
                            Description = "Never run out of battery again with this portable power bank.",
                            ImageSource = "~/images/powerbank.jpg",
                            Name = "Portable Power Bank",
                            UnitPrice = 1499.0
                        });
                });

            modelBuilder.Entity("EMarket.Models.ProductCategory", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("ProductId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ProductId", "CategoryId")
                        .IsUnique();

                    b.ToTable("ProductCategories");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            CategoryId = 1
                        },
                        new
                        {
                            ProductId = 2,
                            CategoryId = 2
                        },
                        new
                        {
                            ProductId = 3,
                            CategoryId = 3
                        },
                        new
                        {
                            ProductId = 4,
                            CategoryId = 4
                        },
                        new
                        {
                            ProductId = 5,
                            CategoryId = 5
                        },
                        new
                        {
                            ProductId = 6,
                            CategoryId = 6
                        },
                        new
                        {
                            ProductId = 7,
                            CategoryId = 7
                        },
                        new
                        {
                            ProductId = 7,
                            CategoryId = 6
                        },
                        new
                        {
                            ProductId = 8,
                            CategoryId = 8
                        },
                        new
                        {
                            ProductId = 9,
                            CategoryId = 9
                        },
                        new
                        {
                            ProductId = 10,
                            CategoryId = 10
                        },
                        new
                        {
                            ProductId = 11,
                            CategoryId = 2
                        },
                        new
                        {
                            ProductId = 12,
                            CategoryId = 2
                        },
                        new
                        {
                            ProductId = 12,
                            CategoryId = 5
                        },
                        new
                        {
                            ProductId = 12,
                            CategoryId = 10
                        },
                        new
                        {
                            ProductId = 13,
                            CategoryId = 2
                        },
                        new
                        {
                            ProductId = 14,
                            CategoryId = 2
                        },
                        new
                        {
                            ProductId = 15,
                            CategoryId = 2
                        },
                        new
                        {
                            ProductId = 15,
                            CategoryId = 5
                        },
                        new
                        {
                            ProductId = 15,
                            CategoryId = 10
                        },
                        new
                        {
                            ProductId = 16,
                            CategoryId = 2
                        },
                        new
                        {
                            ProductId = 17,
                            CategoryId = 2
                        },
                        new
                        {
                            ProductId = 18,
                            CategoryId = 2
                        },
                        new
                        {
                            ProductId = 19,
                            CategoryId = 2
                        },
                        new
                        {
                            ProductId = 19,
                            CategoryId = 3
                        },
                        new
                        {
                            ProductId = 20,
                            CategoryId = 2
                        });
                });

            modelBuilder.Entity("EMarket.Models.Purchase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ProductId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("EMarket.Models.Receiver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNo")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Receivers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("EMarket.Models.ProductCategory", b =>
                {
                    b.HasOne("EMarket.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EMarket.Models.Product", "Product")
                        .WithMany("Category")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("EMarket.Models.Purchase", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EMarket.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EMarket.Models.Product", b =>
                {
                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}
