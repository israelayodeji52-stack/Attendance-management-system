namespace Attendance.Contracts.Courses;

public class CourseResponse
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public int Unit { get; set; }

    public Guid SemesterId { get; set; }
}
