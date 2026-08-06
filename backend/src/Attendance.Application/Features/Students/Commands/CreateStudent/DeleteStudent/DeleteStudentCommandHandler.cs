using Attendance.Application.Interfaces;
using Attendance.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Students.Commands.DeleteStudent;

public class DeleteStudentCommandHandler
    : IRequestHandler<DeleteStudentCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteStudentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteStudentCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Id == request.Id &&
                     x.Role == UserRole.Student,
                cancellationToken);

        if (student is null)
        {
            throw new KeyNotFoundException("Student not found.");
        }

        _context.Users.Remove(student);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
