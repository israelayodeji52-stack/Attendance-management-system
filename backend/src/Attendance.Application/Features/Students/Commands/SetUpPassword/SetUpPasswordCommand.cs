using Attendance.Contracts.Students;
using MediatR;

namespace Attendance.Application.Features.Students.Commands.SetupPassword;

public sealed record SetupPasswordCommand(
    SetupPasswordRequest Request
) : IRequest;