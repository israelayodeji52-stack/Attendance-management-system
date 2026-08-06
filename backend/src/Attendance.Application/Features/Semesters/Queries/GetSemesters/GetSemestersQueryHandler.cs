using Attendance.Application.Interfaces;
using Attendance.Contracts.Semesters;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Semesters.Queries.GetSemesters;

public sealed class GetSemestersQueryHandler
    : IRequestHandler<GetSemestersQuery, IEnumerable<SemesterResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetSemestersQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SemesterResponse>> Handle(
        GetSemestersQuery request,
        CancellationToken cancellationToken)
    {
        var semesters = await _context.Semesters
            .AsNoTracking()
            .OrderBy(x => x.StartDate)
            .Select(x => new SemesterResponse
            {
                Id = x.Id,
                Name = x.Name,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                IsActive = x.IsActive,
                AcademicSessionId = x.AcademicSessionId
            })
            .ToListAsync(cancellationToken);

        return semesters;
    }
}
