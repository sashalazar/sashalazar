using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication2MVCAuthO.Data.Migrations
{
    public partial class Migration11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverLocationModel_AspNetUsers_UserId",
                table: "DriverLocationModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverLocationModel",
                table: "DriverLocationModel");

            migrationBuilder.RenameTable(
                name: "DriverLocationModel",
                newName: "DriverLocations");

            migrationBuilder.RenameIndex(
                name: "IX_DriverLocationModel_UserId",
                table: "DriverLocations",
                newName: "IX_DriverLocations_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverLocations",
                table: "DriverLocations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverLocations_AspNetUsers_UserId",
                table: "DriverLocations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverLocations_AspNetUsers_UserId",
                table: "DriverLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DriverLocations",
                table: "DriverLocations");

            migrationBuilder.RenameTable(
                name: "DriverLocations",
                newName: "DriverLocationModel");

            migrationBuilder.RenameIndex(
                name: "IX_DriverLocations_UserId",
                table: "DriverLocationModel",
                newName: "IX_DriverLocationModel_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DriverLocationModel",
                table: "DriverLocationModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverLocationModel_AspNetUsers_UserId",
                table: "DriverLocationModel",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
