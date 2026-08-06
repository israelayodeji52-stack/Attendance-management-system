using Attendance.Application.Interfaces;
using Attendance.Contracts.Semesters;
using Attendance.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Semesters.Commands.CreateSemester;

public sealed class CreateSemesterCommandHandler
    : IRequestHandler<CreateSemesterCommand, SemesterResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateSemesterCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SemesterResponse> Handle(
        CreateSemesterCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("Semester name is required.");

        if (request.EndDate <= request.StartDate)
            throw new ValidationException("End Date must be greater than Start Date.");

        var academicSession = await _context.AcademicSessions
            .FirstOrDefaultAsync(
                x => x.Id == request.AcademicSessionId,
                cancellationToken);

        if (academicSession is null)
            throw new ValidationException("Academic Session does not exist.");

        var exists = await _context.Semesters.AnyAsync(
            x => x.Name == request.Name &&
                 x.AcademicSessionId == request.AcademicSessionId,
            cancellationToken);

        if (exists)
            throw new ValidationException(
                "Semester already exists for this Academic Session.");

        if (request.IsActive)
        {
            var activeSemesters = await _context.Semesters
                .Where(x => x.IsActive)
                .ToListAsync(cancellationToken);

            foreach (var semester in activeSemesters)
            {
                semester.IsActive = false;
            }
        }

        var entity = new Semester
        {
            Name = request.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsActive = request.IsActive,
            AcademicSessionId = request.AcademicSessionId
        };

        _context.Semesters.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return new SemesterResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            IsActive = entity.IsActive,
            AcademicSessionId = entity.AcademicSessionId
        };
    }
}
