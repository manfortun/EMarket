using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnNullability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2024, 1, 26, 8, 6, 32, 278, DateTimeKind.Local).AddTicks(7494));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2024, 1, 26, 8, 6, 32, 278, DateTimeKind.Local).AddTicks(7502));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2024, 1, 26, 8, 6, 32, 278, DateTimeKind.Local).AddTicks(7504));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2024, 1, 26, 8, 6, 32, 278, DateTimeKind.Local).AddTicks(7506));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2024, 1, 26, 8, 6, 32, 278, DateTimeKind.Local).AddTicks(7508));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2024, 1, 26, 8, 6, 32, 278, DateTimeKind.Local).AddTicks(7510));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2024, 1, 26, 8, 6, 32, 278, DateTimeKind.Local).AddTicks(7512));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateCreated",
                value: new DateTime(2024, 1, 26, 8, 6, 32, 278, DateTimeKind.Local).AddTicks(7513));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "DateCreated",
                value: new DateTime(2024, 1, 26, 8, 6, 32, 278, DateTimeKind.Local).AddTicks(7515));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "DateCreated",
                value: new DateTime(2024, 1, 26, 8, 6, 32, 278, DateTimeKind.Local).AddTicks(7517));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2024, 1, 25, 20, 3, 8, 437, DateTimeKind.Local).AddTicks(9323));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2024, 1, 25, 20, 3, 8, 437, DateTimeKind.Local).AddTicks(9330));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2024, 1, 25, 20, 3, 8, 437, DateTimeKind.Local).AddTicks(9332));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2024, 1, 25, 20, 3, 8, 437, DateTimeKind.Local).AddTicks(9333));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2024, 1, 25, 20, 3, 8, 437, DateTimeKind.Local).AddTicks(9336));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2024, 1, 25, 20, 3, 8, 437, DateTimeKind.Local).AddTicks(9338));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2024, 1, 25, 20, 3, 8, 437, DateTimeKind.Local).AddTicks(9339));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateCreated",
                value: new DateTime(2024, 1, 25, 20, 3, 8, 437, DateTimeKind.Local).AddTicks(9341));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "DateCreated",
                value: new DateTime(2024, 1, 25, 20, 3, 8, 437, DateTimeKind.Local).AddTicks(9342));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "DateCreated",
                value: new DateTime(2024, 1, 25, 20, 3, 8, 437, DateTimeKind.Local).AddTicks(9344));
        }
    }
}
