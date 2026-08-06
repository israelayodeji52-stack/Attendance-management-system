using Attendance.Application.Interfaces;
using Attendance.Contracts.Semesters;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Semesters.Queries.GetSemesterById;

public sealed class GetSemesterByIdQueryHandler
    : IRequestHandler<GetSemesterByIdQuery, SemesterResponse>
{
    private readonly IApplicationDbContext _context;

    public GetSemesterByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SemesterResponse> Handle(
        GetSemesterByIdQuery request,
        CancellationToken cancellationToken)
    {
        var semester = await _context.Semesters
            .AsNoTracking()
            .SingleOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (semester is null)
        {
            throw new KeyNotFoundException("Semester not found.");
        }

        return new SemesterResponse
        {
            Id = semester.Id,
            Name = semester.Name,
            StartDate = semester.StartDate,
            EndDate = semester.EndDate,
            IsActive = semester.IsActive,
            AcademicSessionId = semester.AcademicSessionId
        };
    }
}
