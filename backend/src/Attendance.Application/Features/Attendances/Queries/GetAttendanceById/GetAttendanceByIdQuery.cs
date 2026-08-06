using Attendance.Contracts.Attendances;
using MediatR;

namespace Attendance.Application.Features.Attendances.Queries.GetAttendanceById;

public sealed record GetAttendanceByIdQuery(Guid Id)
    : IRequest<AttendanceResponse>;
