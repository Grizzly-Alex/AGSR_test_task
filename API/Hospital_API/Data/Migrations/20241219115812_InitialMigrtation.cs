using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_API.Data.Migrations
{
    public partial class InitialMigrtation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Active = table.Column<byte>(type: "TINYINT", maxLength: 1, nullable: false, defaultValue: (byte)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientNames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    Use = table.Column<string>(type: "NVARCHAR(max)", nullable: false),
                    Family = table.Column<string>(type: "NVARCHAR(max)", nullable: false),
                    Given = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientNames_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientNames_PatientId",
                table: "PatientNames",
                column: "PatientId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientNames");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
