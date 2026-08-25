using System.ComponentModel.DataAnnotations;
using Attendance.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Students.Commands.SetupPassword;

public sealed class SetupPasswordCommandHandler
    : IRequestHandler<SetupPasswordCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public SetupPasswordCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task Handle(
        SetupPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (string.IsNullOrWhiteSpace(request.Token))
            throw new ValidationException(
                "Setup token is required.");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ValidationException(
                "Password is required.");

        if (request.Password != request.ConfirmPassword)
            throw new ValidationException(
                "Passwords do not match.");

        if (request.Password.Length < 8)
            throw new ValidationException(
                "Password must be at least 8 characters long.");

        // Hash the setup token so we can find the stored token.
        var tokenHash = HashToken(request.Token);

        var setupToken = await _context.PasswordSetupTokens
            .Include(x => x.Student)
            .FirstOrDefaultAsync(
                x => x.TokenHash == tokenHash,
                cancellationToken);

        if (setupToken is null)
            throw new ValidationException(
                "Invalid setup token.");

        if (setupToken.IsUsed)
            throw new ValidationException(
                "This setup link has already been used.");

        if (setupToken.ExpiresAt <= DateTime.UtcNow)
            throw new ValidationException(
                "This setup link has expired.");

        var student = setupToken.Student;

        if (student is null)
            throw new ValidationException(
                "Student account could not be found.");

        // IMPORTANT:
        // Use the same ASP.NET Identity password hasher
        // that the login process uses.
        student.PasswordHash =
            _passwordHasher.HashPassword(request.Password);

        // Setting the password confirms the student's email.
        student.IsEmailConfirmed = true;

        // Prevent the setup link from being reused.
        setupToken.IsUsed = true;

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static string HashToken(string token)
    {
        var bytes = System.Security.Cryptography.SHA256.HashData(
            System.Text.Encoding.UTF8.GetBytes(token));

        return Convert.ToHexString(bytes);
    }
}