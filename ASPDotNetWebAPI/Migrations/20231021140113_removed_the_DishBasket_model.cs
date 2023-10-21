using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPDotNetWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class removed_the_DishBasket_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishBaskets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DishInCarts",
                table: "DishInCarts");

            migrationBuilder.DropIndex(
                name: "IX_DishInCarts_UserId",
                table: "DishInCarts");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DishInCarts",
                newName: "DishId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DishInCarts",
                table: "DishInCarts",
                columns: new[] { "UserId", "DishId" });

            migrationBuilder.CreateIndex(
                name: "IX_DishInCarts_DishId",
                table: "DishInCarts",
                column: "DishId");

            migrationBuilder.AddForeignKey(
                name: "FK_DishInCarts_Dishes_DishId",
                table: "DishInCarts",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishInCarts_Dishes_DishId",
                table: "DishInCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DishInCarts",
                table: "DishInCarts");

            migrationBuilder.DropIndex(
                name: "IX_DishInCarts_DishId",
                table: "DishInCarts");

            migrationBuilder.RenameColumn(
                name: "DishId",
                table: "DishInCarts",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DishInCarts",
                table: "DishInCarts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DishBaskets",
                columns: table => new
                {
                    DishId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DishInCartId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishBaskets", x => new { x.DishId, x.UserId, x.DishInCartId });
                    table.ForeignKey(
                        name: "FK_DishBaskets_DishInCarts_DishInCartId",
                        column: x => x.DishInCartId,
                        principalTable: "DishInCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishBaskets_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishBaskets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishInCarts_UserId",
                table: "DishInCarts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DishBaskets_DishInCartId",
                table: "DishBaskets",
                column: "DishInCartId");

            migrationBuilder.CreateIndex(
                name: "IX_DishBaskets_UserId",
                table: "DishBaskets",
                column: "UserId");
        }
    }
}
