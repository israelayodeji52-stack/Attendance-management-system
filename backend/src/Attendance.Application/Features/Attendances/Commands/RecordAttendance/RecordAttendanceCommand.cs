using Attendance.Contracts.Attendance;
using MediatR;

namespace Attendance.Application.Features.Attendance.Commands.RecordAttendance;

public sealed record RecordAttendanceCommand(
    RecordAttendanceRequest Request)
    : IRequest<RecordAttendanceResponse>;