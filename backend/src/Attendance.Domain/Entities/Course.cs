using Attendance.Domain.Common;

namespace Attendance.Domain.Entities;

public class Course : BaseEntity
{
    public string CourseCode { get; set; } = string.Empty;

    public string CourseTitle { get; set; } = string.Empty;

    public int Units { get; set; }

    public Guid SemesterId { get; set; }

    public Semester Semester { get; set; } = null!;

    public ICollection<StudentCourse> StudentCourses { get; set; }
        = new List<StudentCourse>();

    public ICollection<Attendance> Attendances { get; set; }
        = new List<Attendance>();
}
