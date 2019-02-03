using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ranko.Migrations
{
    public partial class SqliteMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GuildConfig",
                columns: table => new
                {
                    GuildId = table.Column<ulong>(nullable: false),
                    CommandChannelId = table.Column<ulong>(nullable: false),
                    AlertChannelId = table.Column<ulong>(nullable: false),
                    DeleteAlertMessageTimespan = table.Column<uint>(nullable: false),
                    DateFormat = table.Column<ushort>(nullable: false),
                    Language = table.Column<ushort>(nullable: false),
                    DefaultTimezone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildConfig", x => x.GuildId);
                });

            migrationBuilder.CreateTable(
                name: "AdminRoleEntity",
                columns: table => new
                {
                    RoleId = table.Column<ulong>(nullable: false),
                    GuildId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminRoleEntity", x => x.RoleId);
                    table.ForeignKey(
                        name: "ForeignKey_AdminRoleEntity_GuildConfigEntity",
                        column: x => x.GuildId,
                        principalTable: "GuildConfig",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    GuildId = table.Column<ulong>(nullable: false),
                    AssignedUserId = table.Column<ulong>(nullable: false),
                    LastAlertDate = table.Column<DateTime>(nullable: false),
                    DeadlineDate = table.Column<DateTime>(nullable: false),
                    TimeZone = table.Column<string>(nullable: true),
                    CompletionStatus = table.Column<uint>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskId);
                    table.ForeignKey(
                        name: "ForeignKey_TaskEntity_GuildConfigEntity",
                        column: x => x.GuildId,
                        principalTable: "GuildConfig",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminRoleEntity_GuildId",
                table: "AdminRoleEntity",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_GuildId",
                table: "Tasks",
                column: "GuildId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminRoleEntity");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "GuildConfig");
        }
    }
}
