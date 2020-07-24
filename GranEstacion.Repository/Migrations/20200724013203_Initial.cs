using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GranEstacion.Repository.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cameras",
                columns: table => new
                {
                    CameraId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cameras", x => x.CameraId);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    LogId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    CameraId = table.Column<int>(nullable: false),
                    Entered = table.Column<int>(nullable: false),
                    Exited = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_Logs_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalTable: "Cameras",
                        principalColumn: "CameraId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cameras",
                columns: new[] { "CameraId", "Name" },
                values: new object[,]
                {
                    { 1, "Subida Rampa de Sotano 2 a Sotano 1" },
                    { 2, "Salida Ascensor Sotano 1" },
                    { 3, "Subida Escaleras Sotano 1 a Piso 1" },
                    { 4, "Rampa Subida Sotano 1 a Piso 1" },
                    { 5, "Rampa Bajada de Piso 1 a Sotano 1" },
                    { 6, "Tunel Granestacion 1-2" },
                    { 7, "Puerta 2 Costado Norte" },
                    { 8, "Puerta 2 Costado Sur" },
                    { 9, "Puerta 1 Costado Norte" },
                    { 10, "Puerta 1 Costado Sur" },
                    { 11, "Salida Ascensor Sotano 2" },
                    { 12, "Bajada a Rampa a Sotano 2" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_CameraId",
                table: "Logs",
                column: "CameraId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Cameras");
        }
    }
}
