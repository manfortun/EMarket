using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: false),
                    ImageSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Receivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => new { x.ProductId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ProductCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Purchases_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Clothing & Apparel" },
                    { 2, "Electronics" },
                    { 3, "Home & Kitchen" },
                    { 4, "Health & Beauty" },
                    { 5, "Sports & Outdoors" },
                    { 6, "Books & Media" },
                    { 7, "Toys & Games" },
                    { 8, "Automotive" },
                    { 9, "Pets" },
                    { 10, "Jewelry & Accessories" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "DateCreated", "Description", "ImageSource", "Name", "UnitPrice" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7409), "Sleek black tee: Style redefined. Elevate your look effortlessly! 🔥 #FashionEssential", "~/images/OIP.jpg", "T-Shirt", 299.0 },
                    { 2, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7416), "Unleash limitless power with our latest cellphone innovation!", "~/images/cellphone.jpg", "Cellphone", 13999.0 },
                    { 3, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7418), "Unleash precision in the palm of your hand. Elevate your tools with our sleek knife.", "~/images/ec3596459302e2e8e4d586517816a69a.jpg", "Knife", 240.0 },
                    { 4, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7420), "Indulge in luxury with our hydrating lotion. Elevate your skincare routine effortlessly.", "~/images/lotion.jpg", "Lotion", 250.0 },
                    { 5, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7421), "Step up your game with our stylish rubber shoes. Elevate your look with every stride.", "~/images/rubbershoes.jpg", "Rubber Shoes", 5500.0 },
                    { 6, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7423), "Master clean code principles. Robert Martin's essential guide.", "~/images/cleancode.jpg", "Clean Code", 2890.0 },
                    { 7, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7424), "Immerse in endless adventures. Explore, create, survive. Minecraft awaits!", "~/images/Minecraft.jpg", "Minecraft", 150.0 },
                    { 8, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7426), "Upgrade your cleaning game with our durable fiber cloth.", "~/images/fibrecloth.jpg", "Fibre Cloth", 40.0 },
                    { 9, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7428), "Pure nourishment for your pet. Goat's milk: natural goodness.", "~/images/goatsmilk.jpg", "Goat's Milk", 380.0 },
                    { 10, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7430), "Elegant luxury, timeless beauty. Elevate your style with 14k gold.", "~/images/necklace.jpg", "14K Gold Necklace", 21500.0 },
                    { 11, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7431), "Powerful laptop with high-speed performance. Perfect for work or entertainment on the go.", "~/images/laptop.jpg", "Laptop", 50000.0 },
                    { 12, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7433), "Track your fitness, receive notifications, and more, all from your wrist.", "~/images/smartwatch.jpg", "Smartwatch", 9999.9500000000007 },
                    { 13, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7435), "Enjoy crisp sound quality and freedom from wires with these wireless earbuds.", "~/images/wirelessearbuds.jpg", "Wireless Earbuds", 3999.9499999999998 },
                    { 14, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7437), "Take your music anywhere with this portable Bluetooth speaker.", "~/images/bluetoothspeaker.jpg", "Portable Bluetooth Speaker", 2499.9499999999998 },
                    { 15, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7438), "Monitor your health and track your fitness goals with this sleek fitness tracker.", "~/images/fitnesstracker.jpg", "Fitness Tracker", 2995.0 },
                    { 16, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7440), "Brew your favorite coffee just the way you like it.", "~/images/coffeemaker.jpg", "Coffee Maker", 6850.5 },
                    { 17, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7442), "Gentle on gums, powerful on plaque.", "~/images/electrictoothbrush.jpg", "Electric Toothbrush", 1999.0 },
                    { 18, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7444), "Capture every moment with stunning clarity using this digital camera.", "~/images/digitalcamera.jpg", "Digital Camera", 14560.6 },
                    { 19, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7472), "Enjoy healthier cooking without sacrificing flavor with this air fryer.", "~/images/airfryer.jpg", "Air Fryer", 4499.0 },
                    { 20, new DateTime(2024, 1, 27, 23, 13, 39, 920, DateTimeKind.Local).AddTicks(7474), "Never run out of battery again with this portable power bank.", "~/images/powerbank.jpg", "Portable Power Bank", 1499.0 }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 },
                    { 5, 5 },
                    { 6, 6 },
                    { 6, 7 },
                    { 7, 7 },
                    { 8, 8 },
                    { 9, 9 },
                    { 10, 10 },
                    { 2, 11 },
                    { 2, 12 },
                    { 5, 12 },
                    { 10, 12 },
                    { 2, 13 },
                    { 2, 14 },
                    { 2, 15 },
                    { 5, 15 },
                    { 10, 15 },
                    { 2, 16 },
                    { 2, 17 },
                    { 2, 18 },
                    { 2, 19 },
                    { 3, 19 },
                    { 2, 20 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CategoryId",
                table: "ProductCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ProductId_CategoryId",
                table: "ProductCategories",
                columns: new[] { "ProductId", "CategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_OwnerId",
                table: "Purchases",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ProductId",
                table: "Purchases",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "Receivers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
