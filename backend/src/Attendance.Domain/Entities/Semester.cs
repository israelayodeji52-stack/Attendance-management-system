using Attendance.Domain.Common;

namespace Attendance.Domain.Entities;

public class Semester : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    // Foreign Key
    public Guid AcademicSessionId { get; set; }

    // Navigation Property
    public AcademicSession AcademicSession { get; set; } = null!;

    // One Semester → Many Courses
    public ICollection<Course> Courses { get; set; }
        = new List<Course>();
}
