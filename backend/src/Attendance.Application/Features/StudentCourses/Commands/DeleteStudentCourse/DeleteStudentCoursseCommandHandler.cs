using Attendance.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.StudentCourses.Commands.DeleteStudentCourse;

public sealed class DeleteStudentCourseCommandHandler
    : IRequestHandler<DeleteStudentCourseCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteStudentCourseCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteStudentCourseCommand request,
        CancellationToken cancellationToken)
    {
        var studentCourse = await _context.StudentCourses
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (studentCourse is null)
            throw new ValidationException(
                "Student course record not found.");

        _context.StudentCourses.Remove(studentCourse);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
