using Attendance.Domain.Entities;
using Attendance.Domain.Enums;
using Attendance.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Apply any pending migrations
        await context.Database.MigrateAsync();

        // Check if an admin already exists
        if (await context.Users.AnyAsync(u => u.Role == UserRole.Admin))
            return;

        var passwordHasher = new PasswordHasher();

        var admin = new ApplicationUser
        {
            StudentNumber = "ADMIN001",
            MatricNumber = "ADMIN001",
            FirstName = "System",
            LastName = "Administrator",
            Email = "admin@attendance.com",
            PasswordHash = passwordHasher.HashPassword("Admin@123"),
            Role = UserRole.Admin,
            IsEmailConfirmed = true
        };

        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}
