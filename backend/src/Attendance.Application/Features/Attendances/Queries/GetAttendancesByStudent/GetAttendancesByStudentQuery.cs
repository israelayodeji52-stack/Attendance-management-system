using Attendance.Contracts.Attendances;
using MediatR;

namespace Attendance.Application.Features.Attendances.Queries.GetAttendancesByStudent;

public sealed record GetAttendancesByStudentQuery(Guid StudentId)
    : IRequest<IEnumerable<AttendanceSummaryResponse>>;