using Attendance.Application.Interfaces;
using Attendance.Contracts.AcademicSessions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.AcademicSessions.Queries.GetAcademicSessionById;

public sealed class GetAcademicSessionByIdQueryHandler
    : IRequestHandler<GetAcademicSessionByIdQuery, AcademicSessionResponse>
{
    private readonly IApplicationDbContext _context;

    public GetAcademicSessionByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AcademicSessionResponse> Handle(
        GetAcademicSessionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var session = await _context.AcademicSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (session is null)
            throw new KeyNotFoundException("Academic Session not found.");

        return new AcademicSessionResponse
        {
            Id = session.Id,
            Name = session.Name,
            StartDate = session.StartDate,
            EndDate = session.EndDate,
            IsActive = session.IsActive
        };
    }
}
