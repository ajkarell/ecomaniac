using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecomaniac.Api.Migrations
{
    public partial class TransactionAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChangeInBalance",
                table: "Transactions",
                newName: "Amount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Transactions",
                newName: "ChangeInBalance");
        }
    }
}
