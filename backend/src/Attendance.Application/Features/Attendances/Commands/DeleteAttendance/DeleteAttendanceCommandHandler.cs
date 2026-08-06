using Attendance.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Attendances.Commands.DeleteAttendance;

public sealed class DeleteAttendanceCommandHandler
    : IRequestHandler<DeleteAttendanceCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAttendanceCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        var attendance = await _context.Attendances
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (attendance is null)
            throw new KeyNotFoundException("Attendance record not found.");

        _context.Attendances.Remove(attendance);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
