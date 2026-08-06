using Attendance.Application.Features.Attendances.Commands.DeleteAttendance;
using Attendance.Application.Features.Attendances.Commands.MarkAttendance;
using Attendance.Application.Features.Attendances.Commands.UpdateAttendance;
using Attendance.Application.Features.Attendances.Queries.GetAttendanceById;
using Attendance.Application.Features.Attendances.Queries.GetAttendances;
using Attendance.Contracts.Attendances;
using MediatR;

namespace Attendance.Api.Endpoints.Attendances;

public static class AttendanceEndpoints
{
    public static IEndpointRouteBuilder MapAttendanceEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/attendances")
            .WithTags("Attendances");

        // MARK ATTENDANCE
        group.MapPost("/mark",
            async (
                MarkAttendanceRequest request,
                ISender sender) =>
            {
                var response = await sender.Send(
                    new MarkAttendanceCommand(request));

                return Results.Created(
                    $"/api/attendances/{response.Id}",
                    response);
            });

        // GET ALL
        group.MapGet("/",
            async (ISender sender) =>
            {
                var response = await sender.Send(
                    new GetAttendancesQuery());

                return Results.Ok(response);
            });

        // GET BY ID
        group.MapGet("/{id:guid}",
            async (
                Guid id,
                ISender sender) =>
            {
                var response = await sender.Send(
                    new GetAttendanceByIdQuery(id));

                return Results.Ok(response);
            });

        // UPDATE
        group.MapPut("/{id:guid}",
            async (
                Guid id,
                UpdateAttendanceRequest request,
                ISender sender) =>
            {
                var response = await sender.Send(
                    new UpdateAttendanceCommand(
                        id,
                        request.StudentId,
                        request.CourseId,
                        request.SemesterId,
                        request.AcademicSessionId,
                        request.Status));

                return Results.Ok(response);
            });

        // DELETE
        group.MapDelete("/{id:guid}",
            async (
                Guid id,
                ISender sender) =>
            {
                await sender.Send(
                    new DeleteAttendanceCommand(id));

                return Results.NoContent();
            });

        return app;
    }
}
