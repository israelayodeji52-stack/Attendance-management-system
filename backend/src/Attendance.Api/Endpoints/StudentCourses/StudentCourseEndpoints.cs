using Attendance.Application.Features.StudentCourses.Commands.CreateStudentCourse;
using Attendance.Application.Features.StudentCourses.Commands.DeleteStudentCourse;
using Attendance.Application.Features.StudentCourses.Commands.UpdateStudentCourse;
using Attendance.Application.Features.StudentCourses.Queries.GetStudentCourseById;
using Attendance.Application.Features.StudentCourses.Queries.GetStudentCourses;
using Attendance.Contracts.StudentCourses;
using MediatR;

namespace Attendance.Api.Endpoints.StudentCourses;

public static class StudentCourseEndpoints
{
    public static IEndpointRouteBuilder MapStudentCourseEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/student-courses")
            .WithTags("Student Courses");

        // CREATE
        group.MapPost("/", async (
            CreateStudentCourseRequest request,
            ISender sender) =>
        {
            var result = await sender.Send(
                new CreateStudentCourseCommand(request));

            return Results.Created(
                $"/api/student-courses/{result.Id}",
                result);
        });

        // GET ALL
        group.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(
                new GetStudentsCoursesQuery());

            return Results.Ok(result);
        });

        // GET BY ID
        group.MapGet("/{id:guid}", async (
            Guid id,
            ISender sender) =>
        {
            var result = await sender.Send(
                new GetStudentCourseByIdQuery(id));

            return Results.Ok(result);
        });

        // UPDATE
        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateStudentCourseCommand request,
            ISender sender) =>
        {
            var command = new UpdateStudentCourseCommand(
                id,
                request.StudentId,
                request.CourseId);

            var result = await sender.Send(command);

            return Results.Ok(result);
        });

        // DELETE
        group.MapDelete("/{id:guid}", async (
            Guid id,
            ISender sender) =>
        {
            await sender.Send(
                new DeleteStudentCourseCommand(id));

            return Results.NoContent();
        });

        return app;
    }
}
