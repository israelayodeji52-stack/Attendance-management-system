using Attendance.Contracts.Attendances;
using MediatR;

namespace Attendance.Application.Features.Attendances.Commands.UpdateAttendance;

public sealed record UpdateAttendanceCommand(
    Guid Id,
    Guid StudentId,
    Guid CourseId,
    Guid SemesterId,
    Guid AcademicSessionId,
    bool Status)
    : IRequest<AttendanceResponse>;
