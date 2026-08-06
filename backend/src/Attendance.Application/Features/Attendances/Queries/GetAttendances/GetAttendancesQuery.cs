using Attendance.Contracts.Attendances;
using MediatR;

namespace Attendance.Application.Features.Attendances.Queries.GetAttendances;

public sealed record GetAttendancesQuery
    : IRequest<IEnumerable<AttendanceSummaryResponse>>;