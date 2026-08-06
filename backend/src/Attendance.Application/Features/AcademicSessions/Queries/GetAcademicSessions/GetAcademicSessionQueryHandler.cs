using Attendance.Application.Interfaces;
using Attendance.Contracts.AcademicSessions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.AcademicSessions.Queries.GetAcademicSessions;

public sealed class GetAcademicSessionsQueryHandler
    : IRequestHandler<GetAcademicSessionsQuery, IReadOnlyList<AcademicSessionResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetAcademicSessionsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<AcademicSessionResponse>> Handle(
        GetAcademicSessionsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.AcademicSessions
            .AsNoTracking()
            .OrderByDescending(x => x.StartDate)
            .Select(x => new AcademicSessionResponse
            {
                Id = x.Id,
                Name = x.Name,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}
