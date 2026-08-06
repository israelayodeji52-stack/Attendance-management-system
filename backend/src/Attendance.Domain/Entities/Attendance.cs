using Attendance.Domain.Common;
using Attendance.Domain.Enums;

namespace Attendance.Domain.Entities;

public class Attendance : BaseEntity
{
    public Guid StudentId { get; set; }
    public ApplicationUser Student { get; set; } = null!;

    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public Guid SemesterId { get; set; }
    public Semester Semester { get; set; } = null!;

    public Guid AcademicSessionId { get; set; }
    public AcademicSession AcademicSession { get; set; } = null!;

    public DateTime AttendanceDate { get; set; }

    public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;
}
