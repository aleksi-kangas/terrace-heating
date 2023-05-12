using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeatingService.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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

            migrationBuilder.InsertData(
                table: "HeatPumpRecords",
                columns: new[] { "Id", "TimeStamp", "TankLimits_LowerTankMaximum", "TankLimits_LowerTankMinimum", "TankLimits_UpperTankMaximum", "TankLimits_UpperTankMinimum", "Temperatures_Circuit1", "Temperatures_Circuit2", "Temperatures_Circuit3", "Temperatures_GroundInput", "Temperatures_GroundOutput", "Temperatures_HotGas", "Temperatures_Inside", "Temperatures_LowerTank", "Temperatures_Outside", "Temperatures_UpperTank" },
                values: new object[] { new Guid("d743aa73-829f-4743-a699-f1ca7d578cc7"), new DateTime(2023, 5, 12, 15, 29, 52, 777, DateTimeKind.Utc).AddTicks(4947), 50L, 40L, 51L, 41L, 10f, 20f, 30f, 40f, 50f, 60f, 70f, 80f, 90f, 100f });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeatPumpRecords");
        }
    }
}
