using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Study1CApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthUsers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserLogin = table.Column<string>(type: "text", nullable: false),
                    UserHashPassword = table.Column<string>(type: "text", nullable: false),
                    UserDataCreate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserRole = table.Column<int>(type: "integer", nullable: false),
                    UserRoleNavigationRoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUsers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_AuthUsers_roles_UserRoleNavigationRoleId",
                        column: x => x.UserRoleNavigationRoleId,
                        principalTable: "roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthUsers_UserRoleNavigationRoleId",
                table: "AuthUsers",
                column: "UserRoleNavigationRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthUsers");
        }
    }
}
