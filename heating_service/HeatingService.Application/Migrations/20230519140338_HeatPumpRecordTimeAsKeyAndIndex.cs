using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeatingService.Application.Migrations
{
    /// <inheritdoc />
    public partial class HeatPumpRecordTimeAsKeyAndIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HeatPumpRecords",
                table: "HeatPumpRecords");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "HeatPumpRecords");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "HeatPumpRecords",
                newName: "Time");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeatPumpRecords",
                table: "HeatPumpRecords",
                column: "Time");

            migrationBuilder.CreateIndex(
                name: "IX_HeatPumpRecords_Time",
                table: "HeatPumpRecords",
                column: "Time",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HeatPumpRecords",
                table: "HeatPumpRecords");

            migrationBuilder.DropIndex(
                name: "IX_HeatPumpRecords_Time",
                table: "HeatPumpRecords");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "HeatPumpRecords",
                newName: "TimeStamp");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "HeatPumpRecords",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeatPumpRecords",
                table: "HeatPumpRecords",
                column: "Id");
        }
    }
}
