using Attendance.Application.Interfaces;
using Attendance.Contracts.Semesters;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Semesters.Commands.UpdateSemester;

public sealed class UpdateSemesterCommandHandler
    : IRequestHandler<UpdateSemesterCommand, SemesterResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateSemesterCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SemesterResponse> Handle(
        UpdateSemesterCommand request,
        CancellationToken cancellationToken)
    {
        var semester = await _context.Semesters
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (semester is null)
            throw new KeyNotFoundException("Semester not found.");

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException("Semester name is required.");

        if (request.EndDate <= request.StartDate)
            throw new ValidationException("End Date must be greater than Start Date.");

        var exists = await _context.Semesters.AnyAsync(
            x => x.Name == request.Name &&
                 x.AcademicSessionId == semester.AcademicSessionId &&
                 x.Id != request.Id,
            cancellationToken);

        if (exists)
            throw new ValidationException(
                "Semester already exists for this Academic Session.");

        if (request.IsActive)
        {
            var activeSemesters = await _context.Semesters
                .Where(x => x.IsActive && x.Id != request.Id)
                .ToListAsync(cancellationToken);

            foreach (var item in activeSemesters)
            {
                item.IsActive = false;
            }
        }

        semester.Name = request.Name;
        semester.StartDate = request.StartDate;
        semester.EndDate = request.EndDate;
        semester.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

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
