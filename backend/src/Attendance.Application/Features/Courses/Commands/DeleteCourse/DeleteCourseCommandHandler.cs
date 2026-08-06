using Attendance.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Courses.Commands.DeleteCourse;

public sealed class DeleteCourseCommandHandler
    : IRequestHandler<DeleteCourseCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCourseCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteCourseCommand request,
        CancellationToken cancellationToken)
    {
        var course = await _context.Courses
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (course is null)
            throw new KeyNotFoundException("Course not found.");

        _context.Courses.Remove(course);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
