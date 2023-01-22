using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment1_NotMVC.Migrations
{
    public partial class Create8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastOTPGeneratedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OTP",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastOTPGeneratedDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OTP",
                table: "AspNetUsers");
        }
    }
}
