using Attendance.Application.Features.StudentCourses.Commands.EnrollStudent;
using Attendance.Contracts.StudentCourses;
using MediatR;

namespace Attendance.Api.Endpoints.StudentCourses;

public static class StudentCourseEndpoints
{
    public static IEndpointRouteBuilder MapStudentCourseEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/student-courses")
            .WithTags("Student Courses");

        // ======================================================
        // ENROLL STUDENT
        // POST /api/student-courses
        // ======================================================

        group.MapPost(
            "/",
            EnrollStudent)
            .WithName("EnrollStudent");

        return app;
    }

    private static async Task<IResult> EnrollStudent(
        EnrollStudentRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(
            new EnrollStudentCommand(request),
            cancellationToken);

        return Results.Ok(response);
    }
}