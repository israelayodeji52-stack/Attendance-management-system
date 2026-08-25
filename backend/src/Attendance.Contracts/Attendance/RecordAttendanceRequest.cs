namespace Attendance.Contracts.Attendance;

public sealed class RecordAttendanceRequest
{
    public string MatricNumber { get; set; } = string.Empty;

    public Guid CourseId { get; set; }

    public Guid SemesterId { get; set; }

    public Guid AcademicSessionId { get; set; }
}