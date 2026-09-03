var builder = DistributedApplication.CreateBuilder(args);
var jwtSecret = Environment.GetEnvironmentVariable("Jwt__Secret");

if (string.IsNullOrWhiteSpace(jwtSecret))
{
       throw new InvalidOperationException("Jwt__Secret is not configured.");
}

// Add PostgreSQL server setup
var postgres = builder.AddPostgres("postgres")
                      .WithDataVolume("attendance-clean-v4")
                      .WithHostPort(5433);

// Create the Attendance database reference string
var attendanceDb = postgres.AddDatabase("AttendanceDb");

// Register the API project and inject required configurations explicitly!
builder.AddProject<Projects.Attendance_Api>("attendance-api")
       .WithReference(attendanceDb)
       .WaitFor(attendanceDb)
       // Inject JWT environment variables directly into the Aspire container sandbox
       .WithEnvironment("Jwt__Secret", jwtSecret)
       .WithEnvironment("Jwt__Issuer", "Attendance.Api")
       .WithEnvironment("Jwt__Audience", "Attendance.Client");

builder.Build().Run();
