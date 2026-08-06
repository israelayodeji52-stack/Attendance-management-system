namespace Attendance.Contracts.Students;

public class StudentResponse
{
    public Guid Id { get; set; }

    public string StudentNumber { get; set; } = string.Empty;

    public string MatricNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public bool IsEmailConfirmed { get; set; }
}
