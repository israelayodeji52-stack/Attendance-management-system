using Attendance.Domain.Common;
using Attendance.Domain.Enums;

namespace Attendance.Domain.Entities;

public class ApplicationUser : BaseEntity
{
    public string StudentNumber { get; set; } = string.Empty;

    public string MatricNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public bool IsEmailConfirmed { get; set; }

    public string? QrCode { get; set; }

    // Navigation Properties
    public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}
