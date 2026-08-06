using Attendance.Contracts.Attendances;
using MediatR;

namespace Attendance.Application.Features.Attendances.Commands.MarkAttendance;

public sealed record MarkAttendanceCommand(
    MarkAttendanceRequest Request)
    : IRequest<AttendanceResponse>;
