using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPDotNetWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class update_house : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Addnum1",
                table: "Houses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Addnum2",
                table: "Houses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Addtype1",
                table: "Houses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Addtype2",
                table: "Houses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Housetype",
                table: "Houses",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Addnum1",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "Addnum2",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "Addtype1",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "Addtype2",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "Housetype",
                table: "Houses");
        }
    }
}
