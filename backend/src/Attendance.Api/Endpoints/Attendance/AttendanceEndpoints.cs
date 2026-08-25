using Attendance.Application.Features.Attendance.Commands.RecordAttendance;

using Attendance.Application.Features.Attendances.Commands.DeleteAttendance;
using Attendance.Application.Features.Attendances.Commands.MarkAttendance;
using Attendance.Application.Features.Attendances.Commands.UpdateAttendance;

using Attendance.Application.Features.Attendances.Queries.GetAttendanceById;
using Attendance.Application.Features.Attendances.Queries.GetAttendances;
using Attendance.Application.Features.Attendances.Queries.GetAttendancesByStudent;
using Attendance.Application.Features.Attendances.Queries.GetStudentAttendanceSummary;

using Attendance.Contracts.Attendance;
using Attendance.Contracts.Attendances;

using MediatR;

namespace Attendance.Api.Endpoints.Attendance;

public static class AttendanceEndpoints
{
    public static IEndpointRouteBuilder MapAttendanceEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/attendance")
            .WithTags("Attendance");

        // =====================================================
        // GET ALL ATTENDANCE
        // GET /api/attendance
        // =====================================================

        group.MapGet(
            "/",
            GetAttendances)
            .WithName("GetAttendances");

        // =====================================================
        // GET ATTENDANCE BY ID
        // GET /api/attendance/{id}
        // =====================================================

        group.MapGet(
            "/{id:guid}",
            GetAttendanceById)
            .WithName("GetAttendanceById");

        // =====================================================
        // GET ATTENDANCE BY STUDENT
        // GET /api/attendance/student/{studentId}
        // =====================================================

        group.MapGet(
            "/student/{studentId:guid}",
            GetAttendancesByStudent)
            .WithName("GetAttendancesByStudent");

        // =====================================================
        // GET STUDENT ATTENDANCE SUMMARY
        // GET /api/attendance/student/{studentId}/summary
        // =====================================================

        group.MapGet(
            "/student/{studentId:guid}/summary",
            GetStudentAttendanceSummary)
            .WithName("GetStudentAttendanceSummary");

        // =====================================================
        // RECORD ATTENDANCE / QR SCAN
        // POST /api/attendance/scan
        // =====================================================

        group.MapPost(
            "/scan",
            RecordAttendance)
            .WithName("RecordAttendance");

        // =====================================================
        // MARK ATTENDANCE MANUALLY
        // POST /api/attendance/mark
        // =====================================================

        group.MapPost(
            "/mark",
            MarkAttendance)
            .WithName("MarkAttendance");

        // =====================================================
        // UPDATE ATTENDANCE
        // PUT /api/attendance/{id}
        // =====================================================

        group.MapPut(
            "/{id:guid}",
            UpdateAttendance)
            .WithName("UpdateAttendance");

        // =====================================================
        // DELETE ATTENDANCE
        // DELETE /api/attendance/{id}
        // =====================================================

        group.MapDelete(
            "/{id:guid}",
            DeleteAttendance)
            .WithName("DeleteAttendance");

        return app;
    }

    // =========================================================
    // GET ALL ATTENDANCE
    // =========================================================

    private static async Task<IResult> GetAttendances(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(
            new GetAttendancesQuery(),
            cancellationToken);

        return Results.Ok(response);
    }

    // =========================================================
    // GET ATTENDANCE BY ID
    // =========================================================

    private static async Task<IResult> GetAttendanceById(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(
            new GetAttendanceByIdQuery(id),
            cancellationToken);

        return Results.Ok(response);
    }

    // =========================================================
    // GET ATTENDANCE BY STUDENT
    // =========================================================

    private static async Task<IResult> GetAttendancesByStudent(
        Guid studentId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(
            new GetAttendancesByStudentQuery(studentId),
            cancellationToken);

        return Results.Ok(response);
    }

    // =========================================================
    // GET STUDENT ATTENDANCE SUMMARY
    // =========================================================

    private static async Task<IResult> GetStudentAttendanceSummary(
        Guid studentId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(
            new GetStudentAttendanceSummaryQuery(studentId),
            cancellationToken);

        return Results.Ok(response);
    }

    // =========================================================
    // RECORD ATTENDANCE / QR SCAN
    // =========================================================

    private static async Task<IResult> RecordAttendance(
        RecordAttendanceRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(
            new RecordAttendanceCommand(request),
            cancellationToken);

        return Results.Ok(response);
    }

    // =========================================================
    // MARK ATTENDANCE MANUALLY
    // =========================================================

    private static async Task<IResult> MarkAttendance(
        MarkAttendanceRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var response = await sender.Send(
            new MarkAttendanceCommand(request),
            cancellationToken);

        return Results.Ok(response);
    }

    // =========================================================
    // UPDATE ATTENDANCE
    // =========================================================

    private static async Task<IResult> UpdateAttendance(
        Guid id,
        UpdateAttendanceRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new UpdateAttendanceCommand(
            id,
            request.StudentId,
            request.CourseId,
            request.SemesterId,
            request.AcademicSessionId,
            request.Status);

        var response = await sender.Send(
            command,
            cancellationToken);

        return Results.Ok(response);
    }

    // =========================================================
    // DELETE ATTENDANCE
    // =========================================================

    private static async Task<IResult> DeleteAttendance(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new DeleteAttendanceCommand(id),
            cancellationToken);

        return Results.NoContent();
    }
}