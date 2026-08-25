using Attendance.Domain.Entities;
using Attendance.Domain.Enums;
using Attendance.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Apply pending migrations
        await context.Database.MigrateAsync();

        var passwordHasher = new PasswordHasher();

        // Find existing admin
        var admin = await context.Users
            .FirstOrDefaultAsync(u => u.Role == UserRole.Admin);

        if (admin is not null)
        {
            // Reset the existing admin password
            admin.PasswordHash = passwordHasher.HashPassword("Admin@123");
            admin.IsEmailConfirmed = true;

            await context.SaveChangesAsync();

            return;
        }

        // Create admin if one does not exist
        admin = new ApplicationUser
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