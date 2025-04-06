using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Study1CApi.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsNoManipulate = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "material_type",
                columns: table => new
                {
                    type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_material_type", x => x.type_id);
                });

            migrationBuilder.CreateTable(
                name: "study_states",
                columns: table => new
                {
                    state_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    state_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_state_type", x => x.state_id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_surname = table.Column<string>(type: "text", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: false),
                    user_patronymic = table.Column<string>(type: "text", nullable: true),
                    is_first = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_users_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "materials",
                columns: table => new
                {
                    material_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    material_name = table.Column<string>(type: "text", nullable: false),
                    material_date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    link = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_material", x => x.material_id);
                    table.ForeignKey(
                        name: "fk_materials_types",
                        column: x => x.type,
                        principalTable: "material_type",
                        principalColumn: "type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    course_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    course_name = table.Column<string>(type: "text", nullable: false),
                    course_data_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    description = table.Column<string>(type: "text", nullable: true),
                    link = table.Column<string>(type: "text", nullable: true),
                    author = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_course", x => x.course_id);
                    table.ForeignKey(
                        name: "fk_user_course",
                        column: x => x.author,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "courses_blocks",
                columns: table => new
                {
                    block_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    block_name = table.Column<string>(type: "text", nullable: false),
                    block_date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    course = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    block_number_of_course = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_block", x => x.block_id);
                    table.ForeignKey(
                        name: "fk_courses_blocks",
                        column: x => x.course,
                        principalTable: "courses",
                        principalColumn: "course_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "blocks_materials",
                columns: table => new
                {
                    bm_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    block = table.Column<Guid>(type: "uuid", nullable: false),
                    material = table.Column<Guid>(type: "uuid", nullable: false),
                    bm_date_create = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    note = table.Column<string>(type: "text", nullable: true),
                    duration = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_blocks_materials", x => x.bm_id);
                    table.ForeignKey(
                        name: "fk_bm_blocks",
                        column: x => x.block,
                        principalTable: "courses_blocks",
                        principalColumn: "block_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_bm_materials",
                        column: x => x.material,
                        principalTable: "materials",
                        principalColumn: "material_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "blocks_tasks",
                columns: table => new
                {
                    task_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    task_name = table.Column<string>(type: "text", nullable: false),
                    task_date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    duration = table.Column<int>(type: "integer", nullable: false),
                    block = table.Column<Guid>(type: "uuid", nullable: false),
                    link = table.Column<string>(type: "text", nullable: true),
                    task_number_of_block = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task", x => x.task_id);
                    table.ForeignKey(
                        name: "fk_bm_blocks",
                        column: x => x.block,
                        principalTable: "courses_blocks",
                        principalColumn: "block_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tasks_practice",
                columns: table => new
                {
                    practice_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    practice_name = table.Column<string>(type: "text", nullable: false),
                    practice_date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    duration = table.Column<int>(type: "integer", nullable: false),
                    link = table.Column<string>(type: "text", nullable: true),
                    task = table.Column<Guid>(type: "uuid", nullable: false),
                    number_practice_of_task = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_practice", x => x.practice_id);
                    table.ForeignKey(
                        name: "fk_practice_task",
                        column: x => x.task,
                        principalTable: "blocks_tasks",
                        principalColumn: "task_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users_tasks",
                columns: table => new
                {
                    ut_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    auth_user = table.Column<Guid>(type: "uuid", nullable: false),
                    task = table.Column<Guid>(type: "uuid", nullable: true),
                    practice = table.Column<Guid>(type: "uuid", nullable: true),
                    material = table.Column<Guid>(type: "uuid", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    date_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    duration_task = table.Column<int>(type: "integer", nullable: false),
                    duration_practice = table.Column<int>(type: "integer", nullable: false),
                    duration_material = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ut", x => x.ut_id);
                    table.ForeignKey(
                        name: "fk_users_tasks_material",
                        column: x => x.material,
                        principalTable: "blocks_materials",
                        principalColumn: "bm_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_users_tasks_practice",
                        column: x => x.practice,
                        principalTable: "tasks_practice",
                        principalColumn: "practice_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_users_tasks_status",
                        column: x => x.status,
                        principalTable: "study_states",
                        principalColumn: "state_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_users_tasks_task",
                        column: x => x.task,
                        principalTable: "blocks_tasks",
                        principalColumn: "task_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_users_tasks_user",
                        column: x => x.auth_user,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_blocks_materials_block",
                table: "blocks_materials",
                column: "block");

            migrationBuilder.CreateIndex(
                name: "IX_blocks_materials_material",
                table: "blocks_materials",
                column: "material");

            migrationBuilder.CreateIndex(
                name: "IX_blocks_tasks_block",
                table: "blocks_tasks",
                column: "block");

            migrationBuilder.CreateIndex(
                name: "IX_courses_author",
                table: "courses",
                column: "author");

            migrationBuilder.CreateIndex(
                name: "IX_courses_blocks_course",
                table: "courses_blocks",
                column: "course");

            migrationBuilder.CreateIndex(
                name: "IX_materials_type",
                table: "materials",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_practice_task",
                table: "tasks_practice",
                column: "task");

            migrationBuilder.CreateIndex(
                name: "IX_users_tasks_auth_user",
                table: "users_tasks",
                column: "auth_user");

            migrationBuilder.CreateIndex(
                name: "IX_users_tasks_material",
                table: "users_tasks",
                column: "material");

            migrationBuilder.CreateIndex(
                name: "IX_users_tasks_practice",
                table: "users_tasks",
                column: "practice");

            migrationBuilder.CreateIndex(
                name: "IX_users_tasks_status",
                table: "users_tasks",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_users_tasks_task",
                table: "users_tasks",
                column: "task");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "users_tasks");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "blocks_materials");

            migrationBuilder.DropTable(
                name: "tasks_practice");

            migrationBuilder.DropTable(
                name: "study_states");

            migrationBuilder.DropTable(
                name: "materials");

            migrationBuilder.DropTable(
                name: "blocks_tasks");

            migrationBuilder.DropTable(
                name: "material_type");

            migrationBuilder.DropTable(
                name: "courses_blocks");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
