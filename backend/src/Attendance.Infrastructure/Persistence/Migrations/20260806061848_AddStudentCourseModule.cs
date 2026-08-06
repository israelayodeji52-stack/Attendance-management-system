using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentCourseModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_AcademicSessions_AcademicSessionId",
                table: "StudentCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_Semesters_SemesterId",
                table: "StudentCourses");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourses_SemesterId",
                table: "StudentCourses");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourses_StudentId",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "SemesterId",
                table: "StudentCourses");

            migrationBuilder.AlterColumn<Guid>(
                name: "AcademicSessionId",
                table: "StudentCourses",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_StudentId_CourseId",
                table: "StudentCourses",
                columns: new[] { "StudentId", "CourseId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_AcademicSessions_AcademicSessionId",
                table: "StudentCourses",
                column: "AcademicSessionId",
                principalTable: "AcademicSessions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_AcademicSessions_AcademicSessionId",
                table: "StudentCourses");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourses_StudentId_CourseId",
                table: "StudentCourses");

            migrationBuilder.AlterColumn<Guid>(
                name: "AcademicSessionId",
                table: "StudentCourses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SemesterId",
                table: "StudentCourses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_SemesterId",
                table: "StudentCourses",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_StudentId",
                table: "StudentCourses",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_AcademicSessions_AcademicSessionId",
                table: "StudentCourses",
                column: "AcademicSessionId",
                principalTable: "AcademicSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_Semesters_SemesterId",
                table: "StudentCourses",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
