namespace Attendance.Contracts.AcademicSessions;

public class CreateAcademicSessionRequest
{
    public string Name { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }
}
