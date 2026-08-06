namespace Attendance.Contracts.Semesters;

public class CreateSemesterRequest
{
    public string Name { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    public Guid AcademicSessionId { get; set; }
}
