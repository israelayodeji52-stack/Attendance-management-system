namespace Attendance.Contracts.Attendances;

public class UpdateAttendanceRequest
{
    public Guid StudentId { get; set; }

    public Guid CourseId { get; set; }

    public Guid SemesterId { get; set; }

    public Guid AcademicSessionId { get; set; }

    public bool Status { get; set; }
}
