using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureAttendanceRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AcademicSessions_AcademicSessionId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Courses_CourseId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Semesters_SemesterId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourses_StudentId",
                table: "StudentCourses");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_StudentId",
                table: "Attendances");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId1",
                table: "StudentCourses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AcademicSessionId1",
                table: "Attendances",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId1",
                table: "Attendances",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_CourseId1",
                table: "StudentCourses",
                column: "CourseId1");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_StudentId_CourseId",
                table: "StudentCourses",
                columns: new[] { "StudentId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_AcademicSessionId1",
                table: "Attendances",
                column: "AcademicSessionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_CourseId1",
                table: "Attendances",
                column: "CourseId1");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentId_CourseId_SemesterId_AcademicSessionId~",
                table: "Attendances",
                columns: new[] { "StudentId", "CourseId", "SemesterId", "AcademicSessionId", "AttendanceDate" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AcademicSessions_AcademicSessionId",
                table: "Attendances",
                column: "AcademicSessionId",
                principalTable: "AcademicSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AcademicSessions_AcademicSessionId1",
                table: "Attendances",
                column: "AcademicSessionId1",
                principalTable: "AcademicSessions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Courses_CourseId",
                table: "Attendances",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Courses_CourseId1",
                table: "Attendances",
                column: "CourseId1",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Semesters_SemesterId",
                table: "Attendances",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_Courses_CourseId1",
                table: "StudentCourses",
                column: "CourseId1",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AcademicSessions_AcademicSessionId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AcademicSessions_AcademicSessionId1",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Courses_CourseId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Courses_CourseId1",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Semesters_SemesterId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_Courses_CourseId1",
                table: "StudentCourses");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourses_CourseId1",
                table: "StudentCourses");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourses_StudentId_CourseId",
                table: "StudentCourses");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_AcademicSessionId1",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_CourseId1",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_StudentId_CourseId_SemesterId_AcademicSessionId~",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "CourseId1",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "AcademicSessionId1",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "CourseId1",
                table: "Attendances");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_StudentId",
                table: "StudentCourses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentId",
                table: "Attendances",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AcademicSessions_AcademicSessionId",
                table: "Attendances",
                column: "AcademicSessionId",
                principalTable: "AcademicSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Courses_CourseId",
                table: "Attendances",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Semesters_SemesterId",
                table: "Attendances",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
