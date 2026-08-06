using Attendance.Contracts.Authentication;
using MediatR;

namespace Attendance.Application.Features.Authentication.Commands;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<LoginResponse>;
