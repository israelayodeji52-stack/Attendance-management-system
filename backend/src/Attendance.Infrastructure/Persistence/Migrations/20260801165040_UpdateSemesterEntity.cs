using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSemesterEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AcademicSessionId",
                table: "Semesters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Semesters",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Semesters",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Semesters",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Semesters_AcademicSessionId",
                table: "Semesters",
                column: "AcademicSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Semesters_AcademicSessions_AcademicSessionId",
                table: "Semesters",
                column: "AcademicSessionId",
                principalTable: "AcademicSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Semesters_AcademicSessions_AcademicSessionId",
                table: "Semesters");

            migrationBuilder.DropIndex(
                name: "IX_Semesters_AcademicSessionId",
                table: "Semesters");

            migrationBuilder.DropColumn(
                name: "AcademicSessionId",
                table: "Semesters");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Semesters");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Semesters");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Semesters");
        }
    }
}
