using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeatingGateway.Application.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HeatPumpRecords",
                columns: table => new
                {
                    Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TankLimits_LowerTankMinimum = table.Column<int>(type: "integer", nullable: false),
                    TankLimits_LowerTankMinimumAdjusted = table.Column<int>(type: "integer", nullable: false),
                    TankLimits_LowerTankMaximum = table.Column<int>(type: "integer", nullable: false),
                    TankLimits_LowerTankMaximumAdjusted = table.Column<int>(type: "integer", nullable: false),
                    TankLimits_UpperTankMinimum = table.Column<int>(type: "integer", nullable: false),
                    TankLimits_UpperTankMinimumAdjusted = table.Column<int>(type: "integer", nullable: false),
                    TankLimits_UpperTankMaximum = table.Column<int>(type: "integer", nullable: false),
                    TankLimits_UpperTankMaximumAdjusted = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_HeatPumpRecords", x => x.Time);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeatPumpRecords_Time",
                table: "HeatPumpRecords",
                column: "Time",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeatPumpRecords");
        }
    }
}
