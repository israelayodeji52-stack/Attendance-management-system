using Attendance.Application.Interfaces;
using Attendance.Contracts.Attendances;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Attendances.Queries.GetAttendancesByStudent;

public sealed class GetAttendancesByStudentQueryHandler
    : IRequestHandler<
        GetAttendancesByStudentQuery,
        IEnumerable<AttendanceSummaryResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetAttendancesByStudentQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AttendanceSummaryResponse>> Handle(
        GetAttendancesByStudentQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Attendances
            .Where(x => x.StudentId == request.StudentId)
            .Include(x => x.Student)
            .Include(x => x.Course)
            .OrderByDescending(x => x.AttendanceDate)
            .Select(x => new AttendanceSummaryResponse
            {
                Id = x.Id,
                StudentName =
                    $"{x.Student.FirstName} {x.Student.LastName}",
                CourseCode = x.Course.CourseCode,
                Status = x.Status.ToString(),
                AttendanceDate = x.AttendanceDate
            })
            .ToListAsync(cancellationToken);
    }
}