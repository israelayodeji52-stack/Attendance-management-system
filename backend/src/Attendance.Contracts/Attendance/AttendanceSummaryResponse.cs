namespace Attendance.Contracts.Attendances;

public class AttendanceSummaryResponse
{
    public Guid Id { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public string CourseCode { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime AttendanceDate { get; set; }
}