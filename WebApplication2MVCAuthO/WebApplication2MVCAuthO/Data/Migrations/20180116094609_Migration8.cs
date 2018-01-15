using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication2MVCAuthO.Data.Migrations
{
    public partial class Migration8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DriverLocationModel",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 256, nullable: false),
                    Latitude = table.Column<string>(maxLength: 256, nullable: true),
                    Longitude = table.Column<string>(maxLength: 256, nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverLocationModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverLocationModel_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverLocationModel_UserId",
                table: "DriverLocationModel",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverLocationModel");
        }
    }
}
