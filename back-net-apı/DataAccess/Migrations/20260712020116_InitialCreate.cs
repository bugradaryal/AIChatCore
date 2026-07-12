using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "userMessages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_message = table.Column<string>(type: "text", nullable: false),
                    user_message_date = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    SessionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userMessages", x => x.id);
                    table.ForeignKey(
                        name: "FK_userMessages_sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "aiMessages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ai_message = table.Column<string>(type: "text", nullable: true, defaultValue: "No Response!"),
                    ai_message_date = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UserMessageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aiMessages", x => x.id);
                    table.ForeignKey(
                        name: "FK_aiMessages_userMessages_UserMessageId",
                        column: x => x.UserMessageId,
                        principalTable: "userMessages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ip_adress = table.Column<IPAddress>(type: "inet", nullable: true),
                    UserMessageId = table.Column<int>(type: "integer", nullable: true),
                    AiMessageId = table.Column<int>(type: "integer", nullable: true),
                    date = table.Column<DateTime>(type: "timestamptz", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    prop = table.Column<string>(type: "text", nullable: true, defaultValue: "Unknown")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_logs_aiMessages_AiMessageId",
                        column: x => x.AiMessageId,
                        principalTable: "aiMessages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_logs_userMessages_UserMessageId",
                        column: x => x.UserMessageId,
                        principalTable: "userMessages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_aiMessages_UserMessageId",
                table: "aiMessages",
                column: "UserMessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_logs_AiMessageId",
                table: "logs",
                column: "AiMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_logs_UserMessageId",
                table: "logs",
                column: "UserMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_userMessages_SessionId",
                table: "userMessages",
                column: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropTable(
                name: "aiMessages");

            migrationBuilder.DropTable(
                name: "userMessages");

            migrationBuilder.DropTable(
                name: "sessions");
        }
    }
}
