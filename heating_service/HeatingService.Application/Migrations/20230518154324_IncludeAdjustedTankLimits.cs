using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeatingService.Application.Migrations
{
    /// <inheritdoc />
    public partial class IncludeAdjustedTankLimits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TankLimits_UpperTankMinimum",
                table: "HeatPumpRecords",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "TankLimits_UpperTankMaximum",
                table: "HeatPumpRecords",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "TankLimits_LowerTankMinimum",
                table: "HeatPumpRecords",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "TankLimits_LowerTankMaximum",
                table: "HeatPumpRecords",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "TankLimits_LowerTankMaximumAdjusted",
                table: "HeatPumpRecords",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TankLimits_LowerTankMinimumAdjusted",
                table: "HeatPumpRecords",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TankLimits_UpperTankMaximumAdjusted",
                table: "HeatPumpRecords",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TankLimits_UpperTankMinimumAdjusted",
                table: "HeatPumpRecords",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TankLimits_LowerTankMaximumAdjusted",
                table: "HeatPumpRecords");

            migrationBuilder.DropColumn(
                name: "TankLimits_LowerTankMinimumAdjusted",
                table: "HeatPumpRecords");

            migrationBuilder.DropColumn(
                name: "TankLimits_UpperTankMaximumAdjusted",
                table: "HeatPumpRecords");

            migrationBuilder.DropColumn(
                name: "TankLimits_UpperTankMinimumAdjusted",
                table: "HeatPumpRecords");

            migrationBuilder.AlterColumn<long>(
                name: "TankLimits_UpperTankMinimum",
                table: "HeatPumpRecords",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "TankLimits_UpperTankMaximum",
                table: "HeatPumpRecords",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "TankLimits_LowerTankMinimum",
                table: "HeatPumpRecords",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "TankLimits_LowerTankMaximum",
                table: "HeatPumpRecords",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
