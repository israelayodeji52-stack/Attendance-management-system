using Attendance.Contracts.Attendances;
using MediatR;

namespace Attendance.Application.Features.Attendances.Commands.UpdateAttendance;

public sealed record UpdateAttendanceCommand(
    Guid Id,
    Guid StudentId,
    Guid CourseId,
    Guid SemesterId,
    Guid AcademicSessionId,
    string Status
) : IRequest<AttendanceResponse>
{
    public UpdateAttendanceCommand(Guid id, Guid studentId, Guid courseId, Guid semesterId, Guid academicSessionId, bool status)
        : this(id, studentId, courseId, semesterId, academicSessionId, status.ToString())
    {
        Status1 = status;
    }

    public bool Status1 { get; }
}