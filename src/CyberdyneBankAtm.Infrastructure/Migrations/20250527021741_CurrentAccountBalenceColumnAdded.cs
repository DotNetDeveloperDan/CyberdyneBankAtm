using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyberdyneBankAtm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CurrentAccountBalenceColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentAccountBalance",
                schema: "public",
                table: "Transactions",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentAccountBalance",
                schema: "public",
                table: "Transactions");
        }
    }
}
