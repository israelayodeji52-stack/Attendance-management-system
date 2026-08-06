namespace Attendance.Contracts.Courses;

public class UpdateCourseRequest
{
    public string Code { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public int Unit { get; set; }

    public Guid SemesterId { get; set; }
}
