using Attendance.Application.Features.AcademicSessions.Commands.CreateAcademicSession;
using Attendance.Application.Features.AcademicSessions.Commands.DeleteAcademicSession;
using Attendance.Application.Features.AcademicSessions.Commands.UpdateAcademicSession;
using Attendance.Application.Features.AcademicSessions.Queries.GetAcademicSessionById;
using Attendance.Application.Features.AcademicSessions.Queries.GetAcademicSessions;
using Attendance.Contracts.AcademicSessions;
using MediatR;

namespace Attendance.Api.Endpoints.AcademicSessions;

public static class AcademicSessionEndpoints
{
    public static IEndpointRouteBuilder MapAcademicSessionEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/academic-sessions")
            .WithTags("Academic Sessions");

        // CREATE
        group.MapPost("/", async (
            CreateAcademicSessionRequest request,
            ISender sender) =>
        {
            var command = new CreateAcademicSessionCommand(request);

            var result = await sender.Send(command);

            return Results.Created(
                $"/api/academic-sessions/{result.Id}",
                result);
        });

        // GET ALL
        group.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(
                new GetAcademicSessionsQuery());

            return Results.Ok(result);
        });

        // GET BY ID
        group.MapGet("/{id:guid}", async (
            Guid id,
            ISender sender) =>
        {
            var result = await sender.Send(
                new GetAcademicSessionByIdQuery(id));

            return Results.Ok(result);
        });

        // UPDATE
        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateAcademicSessionRequest request,
            ISender sender) =>
        {
            var command = new UpdateAcademicSessionCommand(
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
                new DeleteAcademicSessionCommand(id));

            return Results.NoContent();
        });

        return app;
    }
}
