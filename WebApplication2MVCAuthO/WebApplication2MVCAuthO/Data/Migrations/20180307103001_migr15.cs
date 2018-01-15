using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication2MVCAuthO.Data.Migrations
{
    public partial class migr15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 256, nullable: false),
                    AddServices = table.Column<string>(maxLength: 456, nullable: true),
                    CarColor = table.Column<string>(maxLength: 50, nullable: true),
                    CarModel = table.Column<string>(maxLength: 100, nullable: true),
                    CarNum = table.Column<string>(maxLength: 50, nullable: true),
                    CarType = table.Column<string>(maxLength: 50, nullable: true),
                    CarYearProd = table.Column<string>(maxLength: 50, nullable: true),
                    City = table.Column<string>(maxLength: 100, nullable: true),
                    DrLFromDate = table.Column<string>(maxLength: 50, nullable: true),
                    DrLicense = table.Column<string>(maxLength: 50, nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drivers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_UserId",
                table: "Drivers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Drivers");
        }
    }
}
