using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

using Attendance.Api.Endpoints.AcademicSessions;
using Attendance.Api.Endpoints.Attendance;
using Attendance.Api.Endpoints.Authentication;
using Attendance.Api.Endpoints.Courses;
using Attendance.Api.Endpoints.Semesters;
using Attendance.Api.Endpoints.StudentCourses;
using Attendance.Api.Endpoints.Students;
using Attendance.Api.Middleware;
using Attendance.Application;
using Attendance.Infrastructure;
using Attendance.Infrastructure.Persistence; // Required to locate your static DatabaseSeeder class

var builder = WebApplication.CreateBuilder(args);

// ==========================================================
// SERVICES REGISTER
// ==========================================================
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Force Minimal APIs to output using camelCase to perfectly match Next.js layouts
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

// ==========================================================
// OPENAPI
// ==========================================================
builder.Services.AddOpenApi();

// ==========================================================
// JWT AUTHENTICATION
// ==========================================================
var jwtSecret = builder.Configuration["Jwt:Secret"];
if (string.IsNullOrWhiteSpace(jwtSecret))
{
    throw new InvalidOperationException("Jwt:Secret is not configured.");
}

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
if (string.IsNullOrWhiteSpace(jwtIssuer))
{
    throw new InvalidOperationException("Jwt:Issuer is not configured.");
}

var jwtAudience = builder.Configuration["Jwt:Audience"];
if (string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new InvalidOperationException("Jwt:Audience is not configured.");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// ==========================================================
// CORS SPECIFICATIONS
// ==========================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ==========================================================
// BUILD APPLICATION COMPONENT MATRIX
// ==========================================================
var app = builder.Build();

// ==========================================================
// 1. INCOMING NETWORK SECURITY FILTERS (CRITICAL ORDERING)
// ==========================================================
// CORS policy execution must happen FIRST to intercept browser preflight headers before redirection logic triggers!
app.UseCors("Frontend");

// Global exception handler monitors all downstream pipelines
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// ==========================================================
// OPENAPI & SCALAR DOCUMENTATION MAPPERS
// ==========================================================
app.MapOpenApi();
app.MapScalarApiReference();

// ==========================================================
// SYSTEM IDENTITY CONTROL MIDDLEWARES
// ==========================================================
app.UseAuthentication();
app.UseAuthorization();

// ==========================================================
// RE-MAP DYNAMIC MINIMAL API ENDPOINTS
// ==========================================================
app.MapStudentEndpoints();
app.MapAuthenticationEndpoints();
app.MapCourseEndpoints();
app.MapSemesterEndpoints();
app.MapAcademicSessionEndpoints();
app.MapAttendanceEndpoints();
app.MapStudentCourseEndpoints();

// ==========================================================
// ROOT ROOTING SYSTEM APP HEALTH PROBE
// ==========================================================
app.MapGet("/", () => Results.Ok(new
{
    message = "Attendance Management API is running.",
    status = "healthy",
    environment = app.Environment.EnvironmentName
}));

// ==========================================================
// AUTOMATIC DATA SCHEMAS GENERATION & SEEDING ROUTINES
// ==========================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // 1. Extract your application database entity framework context
        var context = services.GetRequiredService<ApplicationDbContext>();

        // 2. Trigger your static seeder thread execution task pattern
        Console.WriteLine("[SEED] Triggering structural schema migration and admin user creation sequence...");
        await DatabaseSeeder.SeedAsync(context);
        Console.WriteLine("[SEED] Database migration and administration profile deployment completed successfully.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An unhandled exception block interrupted the automated database provisioning seeder sequence.");
    }
}

// ==========================================================
// FLUSH EXECUTION PIPELINE / RUN
// ==========================================================
app.Run();
