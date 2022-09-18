using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardStorageService.Data.Migrations
{
    public partial class RenameLastName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecondName",
                table: "Clients",
                newName: "LastName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Clients",
                newName: "SecondName");
        }
    }
}
