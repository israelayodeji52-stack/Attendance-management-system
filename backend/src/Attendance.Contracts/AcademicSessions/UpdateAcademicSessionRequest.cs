namespace Attendance.Contracts.AcademicSessions;

public class UpdateAcademicSessionRequest
{
    public string Name { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }
}
