using Attendance.Application.Interfaces;
using Attendance.Contracts.Attendances;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Attendances.Queries.GetStudentAttendanceSummary;

public sealed class GetStudentAttendanceSummaryQueryHandler
    : IRequestHandler<
        GetStudentAttendanceSummaryQuery,
        StudentAttendanceSummaryResponse>
{
    private readonly IApplicationDbContext _context;

    public GetStudentAttendanceSummaryQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentAttendanceSummaryResponse> Handle(
        GetStudentAttendanceSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var records = await _context.Attendances
            .Where(x => x.StudentId == request.StudentId)
            .Include(x => x.Student)
            .Include(x => x.Course)
            .Select(x => new
            {
                StudentId = x.StudentId,

                StudentName =
                    $"{x.Student.FirstName} {x.Student.LastName}",

                CourseId = x.CourseId,

                CourseCode = x.Course.CourseCode,

                CourseTitle = x.Course.CourseTitle,

                Status = x.Status.ToString()
            })
            .ToListAsync(cancellationToken);

        if (records.Count == 0)
        {
            throw new KeyNotFoundException(
                "No attendance records found for this student.");
        }

        var firstRecord = records.First();

        var totalRecords = records.Count;

        var presentCount = records.Count(x =>
            x.Status == "Present");

        var absentCount = records.Count(x =>
            x.Status == "Absent");

        var lateCount = records.Count(x =>
            x.Status == "Late");

        var attendancePercentage = totalRecords == 0
            ? 0
            : Math.Round(
                (double)presentCount /
                totalRecords *
                100,
                2);

        var courses = records
            .GroupBy(x => new
            {
                x.CourseId,
                x.CourseCode,
                x.CourseTitle
            })
            .Select(group =>
            {
                var total = group.Count();

                var present = group.Count(x =>
                    x.Status == "Present");

                var absent = group.Count(x =>
                    x.Status == "Absent");

                var late = group.Count(x =>
                    x.Status == "Late");

                return new CourseAttendanceSummaryResponse
                {
                    CourseId = group.Key.CourseId,

                    CourseCode = group.Key.CourseCode,

                    CourseTitle = group.Key.CourseTitle,

                    TotalRecords = total,

                    PresentCount = present,

                    AbsentCount = absent,

                    LateCount = late,

                    AttendancePercentage = total == 0
                        ? 0
                        : Math.Round(
                            (double)present /
                            total *
                            100,
                            2)
                };
            })
            .OrderBy(x => x.CourseCode)
            .ToList();

        return new StudentAttendanceSummaryResponse
        {
            StudentId = firstRecord.StudentId,

            StudentName = firstRecord.StudentName,

            TotalRecords = totalRecords,

            PresentCount = presentCount,

            AbsentCount = absentCount,

            LateCount = lateCount,

            AttendancePercentage =
                attendancePercentage,

            Courses = courses
        };
    }
}