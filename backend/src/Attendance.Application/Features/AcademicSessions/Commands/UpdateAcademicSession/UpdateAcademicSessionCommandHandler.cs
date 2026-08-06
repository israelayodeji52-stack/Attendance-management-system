using Attendance.Application.Interfaces;
using Attendance.Contracts.AcademicSessions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.AcademicSessions.Commands.UpdateAcademicSession;

public sealed class UpdateAcademicSessionCommandHandler
    : IRequestHandler<UpdateAcademicSessionCommand, AcademicSessionResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateAcademicSessionCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AcademicSessionResponse> Handle(
        UpdateAcademicSessionCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _context.AcademicSessions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (session is null)
            throw new KeyNotFoundException("Academic Session not found.");

        if (request.EndDate <= request.StartDate)
            throw new ValidationException("End Date must be greater than Start Date.");

        if (request.IsActive)
        {
            var activeSessions = await _context.AcademicSessions
                .Where(x => x.IsActive && x.Id != request.Id)
                .ToListAsync(cancellationToken);

            foreach (var item in activeSessions)
            {
                item.IsActive = false;
            }
        }

        session.Name = request.Name;
        session.StartDate = request.StartDate;
        session.EndDate = request.EndDate;
        session.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

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
