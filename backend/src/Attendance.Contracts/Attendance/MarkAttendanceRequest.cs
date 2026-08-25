namespace Attendance.Contracts.Attendances;

public sealed class MarkAttendanceRequest
{
    public Guid StudentId { get; set; }

    public Guid CourseId { get; set; }

    public Guid SemesterId { get; set; }

    public Guid AcademicSessionId { get; set; }

    public string MatricNumber { get; set; } = string.Empty;
}