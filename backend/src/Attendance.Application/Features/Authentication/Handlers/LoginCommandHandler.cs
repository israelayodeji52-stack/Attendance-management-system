using Attendance.Application.Features.Authentication.Commands;
using Attendance.Application.Interfaces;
using Attendance.Contracts.Authentication;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Attendance.Application.Features.Authentication.Handlers;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenProvider _jwtTokenProvider;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(
        IApplicationDbContext context,
        IJwtTokenProvider jwtTokenProvider,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _jwtTokenProvider = jwtTokenProvider;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Email == request.Email,
                cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var passwordValid = _passwordHasher.VerifyPassword(
            request.Password,
            user.PasswordHash);

        if (!passwordValid)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var token = _jwtTokenProvider.GenerateToken(user);

        return new LoginResponse
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(2),
            Role = user.Role.ToString(),
            FullName = $"{user.FirstName} {user.LastName}"
        };
    }
}
