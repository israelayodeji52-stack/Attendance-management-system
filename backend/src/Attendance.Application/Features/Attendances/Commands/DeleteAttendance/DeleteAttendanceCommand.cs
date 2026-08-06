using MediatR;

namespace Attendance.Application.Features.Attendances.Commands.DeleteAttendance;

public sealed record DeleteAttendanceCommand(Guid Id) : IRequest;
