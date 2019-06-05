using Microsoft.EntityFrameworkCore.Migrations;

namespace Consultations.Data.Migrations
{
    public partial class emailinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailRemaind",
                table: "Consultations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailRemaind",
                table: "Consultations");
        }
    }
}
