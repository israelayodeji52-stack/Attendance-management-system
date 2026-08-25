namespace Attendance.Contracts.StudentCourses;

public sealed class EnrollStudentResponse
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public string StudentNumber { get; set; } = string.Empty;

    public string MatricNumber { get; set; } = string.Empty;

    public string StudentName { get; set; } = string.Empty;

    public Guid CourseId { get; set; }

    public string CourseCode { get; set; } = string.Empty;

    public string CourseTitle { get; set; } = string.Empty;

    public bool AlreadyEnrolled { get; set; }
}