using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPDotNetWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class changed_DishInCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DishInCarts",
                table: "DishInCarts");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "DishInCarts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_DishInCarts",
                table: "DishInCarts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DishInCarts_UserId",
                table: "DishInCarts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DishInCarts",
                table: "DishInCarts");

            migrationBuilder.DropIndex(
                name: "IX_DishInCarts_UserId",
                table: "DishInCarts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DishInCarts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DishInCarts",
                table: "DishInCarts",
                columns: new[] { "UserId", "DishId" });
        }
    }
}
