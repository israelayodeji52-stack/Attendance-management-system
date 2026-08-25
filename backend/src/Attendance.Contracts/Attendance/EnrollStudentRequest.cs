namespace Attendance.Contracts.StudentCourses;

public sealed class EnrollStudentRequest
{
    public Guid StudentId { get; set; }

    public Guid CourseId { get; set; }
}