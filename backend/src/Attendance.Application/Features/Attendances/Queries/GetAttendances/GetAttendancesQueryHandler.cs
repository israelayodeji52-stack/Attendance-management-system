using Attendance.Application.Interfaces;
using Attendance.Contracts.Attendances;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Attendances.Queries.GetAttendances;

public sealed class GetAttendancesQueryHandler
    : IRequestHandler<GetAttendancesQuery, IEnumerable<AttendanceResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetAttendancesQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AttendanceResponse>> Handle(
        GetAttendancesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Attendances
            .Include(x => x.Student)
            .Include(x => x.Course)
            .Include(x => x.Semester)
            .Include(x => x.AcademicSession)
            .OrderByDescending(x => x.AttendanceDate)
            .Select(x => new AttendanceResponse
            {
                Id = x.Id,

                StudentId = x.StudentId,
                StudentName =
                    $"{x.Student.FirstName} {x.Student.LastName}",

                CourseId = x.CourseId,
                CourseCode = x.Course.CourseCode,
                CourseTitle = x.Course.CourseTitle,

                SemesterId = x.SemesterId,
                SemesterName = x.Semester.Name,

                AcademicSessionId = x.AcademicSessionId,
                AcademicSessionName = x.AcademicSession.Name,

                Status = x.Status.ToString(),

                AttendanceDate = x.AttendanceDate
            })
            .ToListAsync(cancellationToken);
    }
}