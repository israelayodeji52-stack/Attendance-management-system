var builder = DistributedApplication.CreateBuilder(args);

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
       .WithEnvironment("Jwt__Secret", "THIS_IS_A_DEVELOPMENT_SECRET_KEY_1234567890_ABCDEFGHIJKLMNOPQRSTUVWXYZ")
       .WithEnvironment("Jwt__Issuer", "Attendance.Api")
       .WithEnvironment("Jwt__Audience", "Attendance.Client");

builder.Build().Run();
