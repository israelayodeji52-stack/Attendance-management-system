using Attendance.Domain.Common;

namespace Attendance.Domain.Entities;

public class StudentCourse : BaseEntity
{
    public Guid StudentId { get; set; }

    public ApplicationUser Student { get; set; } = null!;

    public Guid CourseId { get; set; }

    public Course Course { get; set; } = null!;
}
