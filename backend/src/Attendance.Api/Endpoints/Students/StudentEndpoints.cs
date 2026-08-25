using Attendance.Application.Features.Students.Commands.CreateStudent;
using Attendance.Application.Features.Students.Commands.DeleteStudent;
using Attendance.Application.Features.Students.Commands.SetupPassword;
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

        // =========================
        // CREATE STUDENT
        // =========================

        group.MapPost("/", CreateStudent);

        // =========================
        // GET ALL STUDENTS
        // =========================

        group.MapGet("/", GetStudents);

        // =========================
        // GET STUDENT BY ID
        // =========================

        group.MapGet("/{id:guid}", GetStudentById);

        // =========================
        // UPDATE STUDENT
        // =========================

        group.MapPut("/{id:guid}", UpdateStudent);

        // =========================
        // DELETE STUDENT
        // =========================

        group.MapDelete("/{id:guid}", DeleteStudent);

        // =========================
        // SETUP PASSWORD
        // =========================
        //
        // Student receives a secure token
        // through email and uses it to create
        // their password.
        //

        group.MapPost("/setup-password", SetupPassword);

        return app;
    }

    // =========================================================
    // CREATE STUDENT
    // =========================================================

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

    // =========================================================
    // GET ALL STUDENTS
    // =========================================================

    private static async Task<IResult> GetStudents(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var students = await sender.Send(
            new GetStudentsQuery(),
            cancellationToken);

        return Results.Ok(students);
    }

    // =========================================================
    // GET STUDENT BY ID
    // =========================================================

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

    // =========================================================
    // UPDATE STUDENT
    // =========================================================

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

    // =========================================================
    // DELETE STUDENT
    // =========================================================

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

    // =========================================================
    // SETUP PASSWORD
    // =========================================================

    private static async Task<IResult> SetupPassword(
        SetupPasswordRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new SetupPasswordCommand(request),
            cancellationToken);

        return Results.Ok(new
        {
            message = "Password created successfully."
        });
    }
}