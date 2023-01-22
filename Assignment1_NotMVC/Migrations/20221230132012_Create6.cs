using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment1_NotMVC.Migrations
{
    public partial class Create6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PastPassword1",
                table: "AspNetUsers",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PastPassword2",
                table: "AspNetUsers",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PastPassword1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PastPassword2",
                table: "AspNetUsers");
        }
    }
}
