using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication2MVCAuthO.Data.Migrations
{
    public partial class mig14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 256, nullable: false),
                    ClientRequestId = table.Column<string>(nullable: true),
                    CreatDate = table.Column<DateTime>(nullable: false),
                    DriverLocationId = table.Column<string>(nullable: true),
                    Status = table.Column<string>(maxLength: 40, nullable: true),
                    UpdStatusDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_ClientRequests_ClientRequestId",
                        column: x => x.ClientRequestId,
                        principalTable: "ClientRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_DriverLocations_DriverLocationId",
                        column: x => x.DriverLocationId,
                        principalTable: "DriverLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientRequestId",
                table: "Orders",
                column: "ClientRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DriverLocationId",
                table: "Orders",
                column: "DriverLocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
