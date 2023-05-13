using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeatingService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HeatPumpRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TankLimits_LowerTankMinimum = table.Column<long>(type: "bigint", nullable: false),
                    TankLimits_LowerTankMaximum = table.Column<long>(type: "bigint", nullable: false),
                    TankLimits_UpperTankMinimum = table.Column<long>(type: "bigint", nullable: false),
                    TankLimits_UpperTankMaximum = table.Column<long>(type: "bigint", nullable: false),
                    Temperatures_Circuit1 = table.Column<float>(type: "real", nullable: false),
                    Temperatures_Circuit2 = table.Column<float>(type: "real", nullable: false),
                    Temperatures_Circuit3 = table.Column<float>(type: "real", nullable: false),
                    Temperatures_GroundInput = table.Column<float>(type: "real", nullable: false),
                    Temperatures_GroundOutput = table.Column<float>(type: "real", nullable: false),
                    Temperatures_HotGas = table.Column<float>(type: "real", nullable: false),
                    Temperatures_Inside = table.Column<float>(type: "real", nullable: false),
                    Temperatures_LowerTank = table.Column<float>(type: "real", nullable: false),
                    Temperatures_Outside = table.Column<float>(type: "real", nullable: false),
                    Temperatures_UpperTank = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeatPumpRecords", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeatPumpRecords");
        }
    }
}
