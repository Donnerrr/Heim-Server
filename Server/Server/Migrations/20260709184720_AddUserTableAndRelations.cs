using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTableAndRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "schuldenbuch");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "public",
                table: "Persons",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "schuldenbuch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_UserId",
                schema: "public",
                table: "Persons",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Debts_PersonId",
                schema: "public",
                table: "Debts",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Debts_Persons_PersonId",
                schema: "public",
                table: "Debts",
                column: "PersonId",
                principalSchema: "public",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Users_UserId",
                schema: "public",
                table: "Persons",
                column: "UserId",
                principalSchema: "schuldenbuch",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Debts_Persons_PersonId",
                schema: "public",
                table: "Debts");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Users_UserId",
                schema: "public",
                table: "Persons");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "schuldenbuch");

            migrationBuilder.DropIndex(
                name: "IX_Persons_UserId",
                schema: "public",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Debts_PersonId",
                schema: "public",
                table: "Debts");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "public",
                table: "Persons");
        }
    }
}
