namespace Attendance.Contracts.Attendances;

public class StudentAttendanceSummaryResponse
{
    public Guid StudentId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public int TotalRecords { get; set; }

    public int PresentCount { get; set; }

    public int AbsentCount { get; set; }

    public int LateCount { get; set; }

    public double AttendancePercentage { get; set; }

    public List<CourseAttendanceSummaryResponse> Courses { get; set; }
        = new();
}

public class CourseAttendanceSummaryResponse
{
    public Guid CourseId { get; set; }

    public string CourseCode { get; set; } = string.Empty;

    public string CourseTitle { get; set; } = string.Empty;

    public int TotalRecords { get; set; }

    public int PresentCount { get; set; }

    public int AbsentCount { get; set; }

    public int LateCount { get; set; }

    public double AttendancePercentage { get; set; }
}