using Attendance.Application.Interfaces;
using Attendance.Contracts.AcademicSessions;
using Attendance.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.AcademicSessions.Commands.CreateAcademicSession;

public sealed class CreateAcademicSessionCommandHandler
    : IRequestHandler<CreateAcademicSessionCommand, AcademicSessionResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateAcademicSessionCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AcademicSessionResponse> Handle(
        CreateAcademicSessionCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("Academic Session name is required.");

        if (request.EndDate <= request.StartDate)
            throw new ValidationException("End Date must be greater than Start Date.");

        var exists = await _context.AcademicSessions.AnyAsync(
            x => x.Name == request.Name,
            cancellationToken);

        if (exists)
            throw new ValidationException("Academic Session already exists.");

        if (request.IsActive)
        {
            var activeSessions = await _context.AcademicSessions
                .Where(x => x.IsActive)
                .ToListAsync(cancellationToken);

            foreach (var session in activeSessions)
            {
                session.IsActive = false;
            }
        }

        var academicSession = new AcademicSession
        {
            Name = request.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsActive = request.IsActive
        };

        _context.AcademicSessions.Add(academicSession);

        await _context.SaveChangesAsync(cancellationToken);

        return new AcademicSessionResponse
        {
            Id = academicSession.Id,
            Name = academicSession.Name,
            StartDate = academicSession.StartDate,
            EndDate = academicSession.EndDate,
            IsActive = academicSession.IsActive
        };
    }
}
