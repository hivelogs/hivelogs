using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HiveLogs.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetupAndWebLatestChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_organization_members_user_id",
                table: "organization_members",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_organization_members_user_id",
                table: "organization_members");
        }
    }
}
