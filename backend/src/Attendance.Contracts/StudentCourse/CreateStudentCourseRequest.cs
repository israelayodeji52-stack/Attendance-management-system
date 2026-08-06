namespace Attendance.Contracts.StudentCourses;

public class CreateStudentCourseRequest
{
    public Guid StudentId { get; set; }

    public Guid CourseId { get; set; }
}
