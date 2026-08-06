using MediatR;

namespace Attendance.Application.Features.AcademicSessions.Commands.DeleteAcademicSession;

public record DeleteAcademicSessionCommand(Guid Id) : IRequest<bool>;
