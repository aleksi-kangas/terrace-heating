using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeatingGateway.Application.Migrations
{
    /// <inheritdoc />
    public partial class CompressorToHeatPumpRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompressorRecords");

            migrationBuilder.AddColumn<bool>(
                name: "Compressor_Active",
                table: "HeatPumpRecords",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Compressor_Usage",
                table: "HeatPumpRecords",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Compressor_Active",
                table: "HeatPumpRecords");

            migrationBuilder.DropColumn(
                name: "Compressor_Usage",
                table: "HeatPumpRecords");

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

            migrationBuilder.CreateIndex(
                name: "IX_CompressorRecords_Time",
                table: "CompressorRecords",
                column: "Time",
                unique: true);
        }
    }
}
