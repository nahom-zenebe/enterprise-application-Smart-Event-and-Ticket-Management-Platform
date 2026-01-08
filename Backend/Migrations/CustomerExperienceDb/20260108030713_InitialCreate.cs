using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations.CustomerExperienceDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventInteractions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    InteractionType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Metadata = table.Column<string>(type: "jsonb", nullable: true),
                    UserAgent = table.Column<string>(type: "text", nullable: true),
                    IpAddress = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventInteractions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventInteractions_EventId",
                table: "EventInteractions",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventInteractions_InteractionType",
                table: "EventInteractions",
                column: "InteractionType");

            migrationBuilder.CreateIndex(
                name: "IX_EventInteractions_Timestamp",
                table: "EventInteractions",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_EventInteractions_UserId",
                table: "EventInteractions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventInteractions_UserId_EventId_InteractionType",
                table: "EventInteractions",
                columns: new[] { "UserId", "EventId", "InteractionType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventInteractions");
        }
    }
}
