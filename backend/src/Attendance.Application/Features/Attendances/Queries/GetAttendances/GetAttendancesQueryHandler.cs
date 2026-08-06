using Attendance.Application.Interfaces;
using Attendance.Contracts.Attendances;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Attendances.Queries.GetAttendances;

public sealed class GetAttendancesQueryHandler
    : IRequestHandler<GetAttendancesQuery, IEnumerable<AttendanceSummaryResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetAttendancesQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AttendanceSummaryResponse>> Handle(
        GetAttendancesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Attendances
            .Include(x => x.Student)
            .Include(x => x.Course)
            .OrderByDescending(x => x.AttendanceDate)
            .Select(x => new AttendanceSummaryResponse
            {
                Id = x.Id,
                StudentName = $"{x.Student.FirstName} {x.Student.LastName}",
                CourseCode = x.Course.CourseCode,
                Status = x.Status.ToString(),
                AttendanceDate = x.AttendanceDate
            })
            .ToListAsync(cancellationToken);
    }
}