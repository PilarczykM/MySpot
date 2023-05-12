﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySpot.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeeklyParkingSpots",
                columns: table =>
                    new
                    {
                        Id = table.Column<Guid>(type: "uuid", nullable: false),
                        Name = table.Column<string>(type: "text", nullable: true),
                        Week = table.Column<DateTimeOffset>(
                            type: "timestamp with time zone",
                            nullable: true
                        )
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyParkingSpots", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table =>
                    new
                    {
                        Id = table.Column<Guid>(type: "uuid", nullable: false),
                        ParkingSpotId = table.Column<Guid>(type: "uuid", nullable: true),
                        EmployeeName = table.Column<string>(type: "text", nullable: true),
                        LicensePLate = table.Column<string>(type: "text", nullable: true),
                        Date = table.Column<DateTimeOffset>(
                            type: "timestamp with time zone",
                            nullable: true
                        ),
                        WeeklyParkingSpotId = table.Column<Guid>(type: "uuid", nullable: true)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_WeeklyParkingSpots_WeeklyParkingSpotId",
                        column: x => x.WeeklyParkingSpotId,
                        principalTable: "WeeklyParkingSpots",
                        principalColumn: "Id"
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_WeeklyParkingSpotId",
                table: "Reservations",
                column: "WeeklyParkingSpotId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Reservations");

            migrationBuilder.DropTable(name: "WeeklyParkingSpots");
        }
    }
}
