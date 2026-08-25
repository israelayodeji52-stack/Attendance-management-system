namespace Attendance.Contracts.Attendance;

public sealed class RecordAttendanceResponse
{
    public Guid AttendanceId { get; set; }

    public Guid StudentId { get; set; }

    public string StudentNumber { get; set; } = string.Empty;

    public string MatricNumber { get; set; } = string.Empty;

    public string StudentName { get; set; } = string.Empty;

    public string CourseCode { get; set; } = string.Empty;

    public string CourseTitle { get; set; } = string.Empty;

    public string Semester { get; set; } = string.Empty;

    public string AcademicSession { get; set; } = string.Empty;

    public DateTime AttendanceDate { get; set; }

    public string Status { get; set; } = string.Empty;
}