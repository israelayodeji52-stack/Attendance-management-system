using Attendance.Application.Features.Students.Commands.CreateStudent;
using Attendance.Application.Features.Students.Commands.DeleteStudent;
using Attendance.Application.Features.Students.Commands.UpdateStudent;
using Attendance.Application.Features.Students.Queries.GetStudentById;
using Attendance.Application.Features.Students.Queries.GetStudents;
using Attendance.Contracts.Students;
using MediatR;

namespace Attendance.Api.Endpoints.Students;

public static class StudentEndpoints
{
    public static IEndpointRouteBuilder MapStudentEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/students")
                       .WithTags("Students");

        // Create Student
        group.MapPost("/", CreateStudent);

        // Get All Students
        group.MapGet("/", GetStudents);

        // Get Student By Id
        group.MapGet("/{id:guid}", GetStudentById);

        // Update Student
        group.MapPut("/{id:guid}", UpdateStudent);

        // Delete Student
        group.MapDelete("/{id:guid}", DeleteStudent);

        return app;
    }

    private static async Task<IResult> CreateStudent(
        CreateStudentRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(
            new CreateStudentCommand(request),
            cancellationToken);

        return Results.Created(
            $"/api/students/{response.Id}",
            response);
    }

    private static async Task<IResult> GetStudents(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var students = await sender.Send(
            new GetStudentsQuery(),
            cancellationToken);

        return Results.Ok(students);
    }

    private static async Task<IResult> GetStudentById(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var student = await sender.Send(
            new GetStudentByIdQuery(id),
            cancellationToken);

        return Results.Ok(student);
    }

    private static async Task<IResult> UpdateStudent(
        Guid id,
        UpdateStudentRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(
            new UpdateStudentCommand(id, request),
            cancellationToken);

        return Results.Ok(response);
    }

    private static async Task<IResult> DeleteStudent(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new DeleteStudentCommand(id),
            cancellationToken);

        return Results.NoContent();
    }
}
