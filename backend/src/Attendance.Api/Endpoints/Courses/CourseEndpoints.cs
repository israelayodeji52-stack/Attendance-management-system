using Attendance.Application.Features.Courses.Commands.CreateCourse;
using Attendance.Application.Features.Courses.Commands.DeleteCourse;
using Attendance.Application.Features.Courses.Commands.UpdateCourse;
using Attendance.Application.Features.Courses.Queries.GetCourseById;
using Attendance.Application.Features.Courses.Queries.GetCourses;
using Attendance.Contracts.Courses;
using MediatR;

namespace Attendance.Api.Endpoints.Courses;

public static class CourseEndpoints
{
    public static IEndpointRouteBuilder MapCourseEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/courses")
            .WithTags("Courses");

        // ============================================
        // TEMP TEST ENDPOINT
        // ============================================
        group.MapPost("/test", (CreateCourseRequest request) =>
        {
            return Results.Ok(new
            {
                Message = "JSON Binding Successful",
                Request = request
            });
        });

        // ============================================
        // CREATE
        // ============================================
        group.MapPost("/", async (
            CreateCourseRequest request,
            ISender sender) =>
        {
            var command = new CreateCourseCommand(request);

            var result = await sender.Send(command);

            return Results.Created(
                $"/api/courses/{result.Id}",
                result);
        });

        // ============================================
        // GET ALL
        // ============================================
        group.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(new GetCoursesQuery());

            return Results.Ok(result);
        });

        // ============================================
        // GET BY ID
        // ============================================
        group.MapGet("/{id:guid}", async (
            Guid id,
            ISender sender) =>
        {
            var result = await sender.Send(new GetCourseByIdQuery(id));

            return Results.Ok(result);
        });

        // ============================================
        // UPDATE
        // ============================================
        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateCourseRequest request,
            ISender sender) =>
        {
            var command = new UpdateCourseCommand(
                id,
                request.Code,
                request.Title,
                request.Unit,
                request.SemesterId);

            var result = await sender.Send(command);

            return Results.Ok(result);
        });

        // ============================================
        // DELETE
        // ============================================
        group.MapDelete("/{id:guid}", async (
            Guid id,
            ISender sender) =>
        {
            await sender.Send(new DeleteCourseCommand(id));

            return Results.NoContent();
        });

        return app;
    }
}
