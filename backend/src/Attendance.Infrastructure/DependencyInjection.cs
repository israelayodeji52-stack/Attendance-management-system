using Attendance.Application.Interfaces;

using Attendance.Infrastructure.Authentication;
using Attendance.Infrastructure.Email;
using Attendance.Infrastructure.Identity;
using Attendance.Infrastructure.Persistence;
using Attendance.Infrastructure.QRCode;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Attendance.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ==========================================
        // DATABASE
        // ==========================================

        var connectionString =
            configuration.GetConnectionString(
                "AttendanceDb"
            )
            ?? configuration.GetConnectionString(
                "DefaultConnection"
            );

        if (string.IsNullOrWhiteSpace(
                connectionString))
        {
            throw new InvalidOperationException(
                "Database connection string was not found."
            );
        }

        services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                options.UseNpgsql(
                    connectionString
                );
            }
        );


        // ==========================================
        // APPLICATION DB CONTEXT
        // ==========================================

        services.AddScoped<IApplicationDbContext>(
            provider =>
                provider.GetRequiredService<
                    ApplicationDbContext
                >()
        );


        // ==========================================
        // JWT
        // ==========================================

        services.AddScoped<
            IJwtTokenProvider,
            JwtTokenProvider
        >();


        // ==========================================
        // EMAIL
        // ==========================================

        services.AddScoped<
            IEmailService,
            GmailEmailService
        >();


        // ==========================================
        // PASSWORD HASHING
        // ==========================================

        services.AddScoped<
            IPasswordHasher,
            PasswordHasher
        >();


        // ==========================================
        // QR CODE
        // ==========================================

        services.AddScoped<
            IQrCodeService,
            QrCodeService
        >();


        return services;
    }
}