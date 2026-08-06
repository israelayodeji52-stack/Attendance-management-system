using Attendance.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Semesters.Commands.DeleteSemester;

public sealed class DeleteSemesterCommandHandler
    : IRequestHandler<DeleteSemesterCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteSemesterCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteSemesterCommand request,
        CancellationToken cancellationToken)
    {
        var semester = await _context.Semesters
            .Include(x => x.Courses)
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (semester is null)
            throw new KeyNotFoundException("Semester not found.");

        if (semester.Courses.Any())
            throw new ValidationException(
                "Cannot delete a semester that contains courses.");

        _context.Semesters.Remove(semester);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
