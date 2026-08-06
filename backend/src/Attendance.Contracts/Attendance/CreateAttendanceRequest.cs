namespace Attendance.Contracts.Attendances;

public class CreateAttendanceRequest
{
    public Guid StudentId { get; set; }

    public Guid CourseId { get; set; }

    public Guid SemesterId { get; set; }

    public Guid AcademicSessionId { get; set; }
}
