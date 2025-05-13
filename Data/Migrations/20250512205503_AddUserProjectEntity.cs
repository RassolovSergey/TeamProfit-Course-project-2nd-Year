using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProjectEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FixedPrice",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PercentPrice",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TypeCooperation",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserProjects",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    TypeCooperation = table.Column<int>(type: "integer", nullable: false),
                    FixedPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    PercentPrice = table.Column<byte>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProjects", x => new { x.UserId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_UserProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProjects_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProjects_ProjectId",
                table: "UserProjects",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProjects");

            migrationBuilder.AddColumn<decimal>(
                name: "FixedPrice",
                table: "Users",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "PercentPrice",
                table: "Users",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeCooperation",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
