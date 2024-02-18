using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeatingGateway.Application.Migrations
{
    /// <inheritdoc />
    public partial class SeparateHeatPumpRecordIntoPartitions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeatPumpRecords");

            migrationBuilder.CreateTable(
                name: "CompressorRecords",
                columns: table => new
                {
                    Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Usage = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompressorRecords", x => x.Time);
                });

            migrationBuilder.CreateTable(
                name: "TankLimitRecords",
                columns: table => new
                {
                    Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LowerTankMinimum = table.Column<int>(type: "integer", nullable: false),
                    LowerTankMinimumAdjusted = table.Column<int>(type: "integer", nullable: false),
                    LowerTankMaximum = table.Column<int>(type: "integer", nullable: false),
                    LowerTankMaximumAdjusted = table.Column<int>(type: "integer", nullable: false),
                    UpperTankMinimum = table.Column<int>(type: "integer", nullable: false),
                    UpperTankMinimumAdjusted = table.Column<int>(type: "integer", nullable: false),
                    UpperTankMaximum = table.Column<int>(type: "integer", nullable: false),
                    UpperTankMaximumAdjusted = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TankLimitRecords", x => x.Time);
                });

            migrationBuilder.CreateTable(
                name: "TemperatureRecords",
                columns: table => new
                {
                    Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Circuit1 = table.Column<float>(type: "real", nullable: false),
                    Circuit2 = table.Column<float>(type: "real", nullable: false),
                    Circuit3 = table.Column<float>(type: "real", nullable: false),
                    GroundInput = table.Column<float>(type: "real", nullable: false),
                    GroundOutput = table.Column<float>(type: "real", nullable: false),
                    HotGas = table.Column<float>(type: "real", nullable: false),
                    Inside = table.Column<float>(type: "real", nullable: false),
                    LowerTank = table.Column<float>(type: "real", nullable: false),
                    Outside = table.Column<float>(type: "real", nullable: false),
                    UpperTank = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemperatureRecords", x => x.Time);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompressorRecords_Time",
                table: "CompressorRecords",
                column: "Time",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TankLimitRecords_Time",
                table: "TankLimitRecords",
                column: "Time",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TemperatureRecords_Time",
                table: "TemperatureRecords",
                column: "Time",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompressorRecords");

            migrationBuilder.DropTable(
                name: "TankLimitRecords");

            migrationBuilder.DropTable(
                name: "TemperatureRecords");

            migrationBuilder.CreateTable(
                name: "HeatPumpRecords",
                columns: table => new
                {
                    Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Compressor_Active = table.Column<bool>(type: "boolean", nullable: false),
                    Compressor_Usage = table.Column<double>(type: "double precision", nullable: true),
                    TankLimits_LowerTankMaximum = table.Column<int>(type: "integer", nullable: false),
                    TankLimits_LowerTankMaximumAdjusted = table.Column<int>(type: "integer", nullable: false),
                    TankLimits_LowerTankMinimum = table.Column<int>(type: "integer", nullable: false),
                    TankLimits_LowerTankMinimumAdjusted = table.Column<int>(type: "integer", nullable: false),
                    TankLimits_UpperTankMaximum = table.Column<int>(type: "integer", nullable: false),
                    TankLimits_UpperTankMaximumAdjusted = table.Column<int>(type: "integer", nullable: false),
                    TankLimits_UpperTankMinimum = table.Column<int>(type: "integer", nullable: false),
                    TankLimits_UpperTankMinimumAdjusted = table.Column<int>(type: "integer", nullable: false),
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
    }
}
