using Attendance.Domain.Common;

namespace Attendance.Domain.Entities;

public class AcademicSession : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    // Navigation Properties
    public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}
