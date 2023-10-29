using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPDotNetWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class added_DishBasket_and_DishInCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DishInCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishInCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DishInCarts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DishBaskets",
                columns: table => new
                {
                    DishInCartId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DishId = table.Column<Guid>(type: "uuid", nullable: false)
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
                name: "IX_DishBaskets_DishInCartId",
                table: "DishBaskets",
                column: "DishInCartId");

            migrationBuilder.CreateIndex(
                name: "IX_DishBaskets_UserId",
                table: "DishBaskets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DishInCarts_UserId",
                table: "DishInCarts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishBaskets");

            migrationBuilder.DropTable(
                name: "DishInCarts");
        }
    }
}
