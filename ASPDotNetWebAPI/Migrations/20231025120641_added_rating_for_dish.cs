using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPDotNetWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class added_rating_for_dish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Dishes",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Dishes");
        }
    }
}
