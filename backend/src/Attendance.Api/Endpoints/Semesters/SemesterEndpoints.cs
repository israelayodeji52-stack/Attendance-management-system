using Attendance.Application.Features.Semesters.Commands.CreateSemester;
using Attendance.Application.Features.Semesters.Commands.DeleteSemester;
using Attendance.Application.Features.Semesters.Commands.UpdateSemester;
using Attendance.Application.Features.Semesters.Queries.GetSemesterById;
using Attendance.Application.Features.Semesters.Queries.GetSemesters;
using Attendance.Contracts.Semesters;
using MediatR;

namespace Attendance.Api.Endpoints.Semesters;

public static class SemesterEndpoints
{
    public static IEndpointRouteBuilder MapSemesterEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/semesters")
            .WithTags("Semesters");

        // CREATE
        group.MapPost("/", async (
            CreateSemesterRequest request,
            ISender sender) =>
        {
            var command = new CreateSemesterCommand(request);

            var result = await sender.Send(command);

            return Results.Created(
                $"/api/semesters/{result.Id}",
                result);
        });

        // GET ALL
        group.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(
                new GetSemestersQuery());

            return Results.Ok(result);
        });

        // GET BY ID
        group.MapGet("/{id:guid}", async (
            Guid id,
            ISender sender) =>
        {
            var result = await sender.Send(
                new GetSemesterByIdQuery(id));

            return Results.Ok(result);
        });

        // UPDATE
        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateSemesterRequest request,
            ISender sender) =>
        {
            var command = new UpdateSemesterCommand(
                id,
                request.Name,
                request.StartDate,
                request.EndDate,
                request.IsActive);

            var result = await sender.Send(command);

            return Results.Ok(result);
        });

        // DELETE
        group.MapDelete("/{id:guid}", async (
            Guid id,
            ISender sender) =>
        {
            await sender.Send(
                new DeleteSemesterCommand(id));

            return Results.NoContent();
        });

        return app;
    }
}
