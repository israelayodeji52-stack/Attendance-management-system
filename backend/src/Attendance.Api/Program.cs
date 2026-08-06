using System.Text;
using Attendance.Api.Endpoints.AcademicSessions;
using Attendance.Api.Endpoints.Attendances;
using Attendance.Api.Endpoints.Authentication;
using Attendance.Api.Endpoints.Courses;
using Attendance.Api.Endpoints.Semesters;
using Attendance.Api.Endpoints.StudentCourses;
using Attendance.Api.Endpoints.Students;
using Attendance.Api.Middleware;
using Attendance.Application;
using Attendance.Infrastructure;
using Attendance.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//
// Register Services
//
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

//
// Configure JWT Authentication
//
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });

//
// Authorization
//
builder.Services.AddAuthorization();

//
// OpenAPI / Scalar
//
builder.Services.AddOpenApi();

//
// Verify Aspire injected the connection string
//
Console.WriteLine("========== DATABASE CONNECTION ==========");
Console.WriteLine(builder.Configuration.GetConnectionString("AttendanceDb"));
Console.WriteLine("=========================================");

var app = builder.Build();

//
// Apply Migrations & Seed Database
//
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();

        await DatabaseSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine("=========================================");
        Console.WriteLine("Database initialization failed");
        Console.WriteLine(ex);
        Console.WriteLine("=========================================");
    }
}

//
// Development Middleware
//
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

//
// Global Exception Middleware
//
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

//
// Authentication & Authorization
//
app.UseAuthentication();
app.UseAuthorization();

//
// Register Endpoints
//
app.MapAuthenticationEndpoints();
app.MapStudentEndpoints();
app.MapAcademicSessionEndpoints();
app.MapSemesterEndpoints();
app.MapCourseEndpoints();
app.MapStudentCourseEndpoints();
app.MapAttendanceEndpoints();

app.Run();
