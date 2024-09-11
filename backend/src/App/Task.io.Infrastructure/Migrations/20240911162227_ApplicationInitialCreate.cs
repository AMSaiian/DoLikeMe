using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task.io.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationInitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "task.io-app");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "users",
                schema: "task.io-app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    auth_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.CheckConstraint("CHK_user_created_at_datetime_not_in_future", "created_at <= CURRENT_TIMESTAMP");
                    table.CheckConstraint("CHK_user_created_at_datetime_smaller_than_update_at", "created_at < updated_at");
                    table.CheckConstraint("CHK_user_update_at_datetime_not_in_future", "updated_at <= CURRENT_TIMESTAMP");
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                schema: "task.io-app",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    priority = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasks", x => x.id);
                    table.CheckConstraint("CHK_task_created_at_datetime_not_in_future", "created_at <= CURRENT_TIMESTAMP");
                    table.CheckConstraint("CHK_task_created_at_datetime_smaller_than_update_at", "created_at < updated_at");
                    table.CheckConstraint("CHK_task_update_at_datetime_not_in_future", "updated_at <= CURRENT_TIMESTAMP");
                    table.ForeignKey(
                        name: "FK_tasks_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "task.io-app",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tasks_user_id",
                schema: "task.io-app",
                table: "tasks",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tasks",
                schema: "task.io-app");

            migrationBuilder.DropTable(
                name: "users",
                schema: "task.io-app");
        }
    }
}
