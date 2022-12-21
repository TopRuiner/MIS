using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Polyclinic.Migrations
{
    public partial class InspecitonChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Appointment",
                table: "Inspections");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "Inspections",
                newName: "Recipe");

            migrationBuilder.CreateTable(
                name: "Receptionist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: true),
                    PolyclinicUserID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receptionist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receptionist_AspNetUsers_PolyclinicUserID",
                        column: x => x.PolyclinicUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Receptionist_PolyclinicUserID",
                table: "Receptionist",
                column: "PolyclinicUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Receptionist");

            migrationBuilder.RenameColumn(
                name: "Recipe",
                table: "Inspections",
                newName: "Data");

            migrationBuilder.AddColumn<string>(
                name: "Appointment",
                table: "Inspections",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
