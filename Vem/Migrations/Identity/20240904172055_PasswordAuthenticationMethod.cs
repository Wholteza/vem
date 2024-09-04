using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vem.Migrations.Identity
{
    /// <inheritdoc />
    public partial class PasswordAuthenticationMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "AuthenticationMethodSequence");

            migrationBuilder.CreateTable(
                name: "PasswordAuthentications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('\"AuthenticationMethodSequence\"')"),
                    IdentityId = table.Column<int>(type: "integer", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordAuthentications", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasswordAuthentications");

            migrationBuilder.DropSequence(
                name: "AuthenticationMethodSequence");
        }
    }
}
