using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations.TicketingDb
{
    /// <inheritdoc />
    public partial class UpdateOutboxEventEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastError",
                table: "OutboxEvents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MaxRetries",
                table: "OutboxEvents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextRetryAt",
                table: "OutboxEvents",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                table: "OutboxEvents",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastError",
                table: "OutboxEvents");

            migrationBuilder.DropColumn(
                name: "MaxRetries",
                table: "OutboxEvents");

            migrationBuilder.DropColumn(
                name: "NextRetryAt",
                table: "OutboxEvents");

            migrationBuilder.DropColumn(
                name: "RetryCount",
                table: "OutboxEvents");
        }
    }
}
