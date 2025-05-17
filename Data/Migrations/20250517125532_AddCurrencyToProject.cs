using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyToProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CurrencyId",
                table: "Projects",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Currencies_CurrencyId",
                table: "Projects",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Currencies_CurrencyId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CurrencyId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Projects");
        }
    }
}
