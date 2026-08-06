var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL server with a fixed host port
var postgres = builder.AddPostgres("postgres")
                      .WithDataVolume()
                      .WithHostPort(5433);

// Create the Attendance database
var attendanceDb = postgres.AddDatabase("AttendanceDb");

// Register the API project
builder.AddProject<Projects.Attendance_Api>("attendance-api")
       .WithReference(attendanceDb)
       .WaitFor(attendanceDb);

builder.Build().Run();
