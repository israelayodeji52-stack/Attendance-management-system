using Attendance.Application.Interfaces;
using Attendance.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Attendance.Infrastructure.Identity;

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly Microsoft.AspNetCore.Identity.PasswordHasher<ApplicationUser>
        _passwordHasher = new();

    public string HashPassword(string password)
    {
        var user = new ApplicationUser();

        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(
        string password,
        string passwordHash)
    {
        var user = new ApplicationUser();

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            passwordHash,
            password);

        return result == PasswordVerificationResult.Success ||
               result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}