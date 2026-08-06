namespace Attendance.Contracts.Attendances;

public class AttendanceResponse
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public Guid CourseId { get; set; }

    public string CourseCode { get; set; } = string.Empty;

    public string CourseTitle { get; set; } = string.Empty;

    public Guid SemesterId { get; set; }

    public string SemesterName { get; set; } = string.Empty;

    public Guid AcademicSessionId { get; set; }

    public string AcademicSessionName { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime AttendanceDate { get; set; }
}