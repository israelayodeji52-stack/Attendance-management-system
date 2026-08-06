using Attendance.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.AcademicSessions.Commands.DeleteAcademicSession;

public sealed class DeleteAcademicSessionCommandHandler
    : IRequestHandler<DeleteAcademicSessionCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteAcademicSessionCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(
        DeleteAcademicSessionCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _context.AcademicSessions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (session is null)
            throw new KeyNotFoundException("Academic Session not found.");

        _context.AcademicSessions.Remove(session);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
