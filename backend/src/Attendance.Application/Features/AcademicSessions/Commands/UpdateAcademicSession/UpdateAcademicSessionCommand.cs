using Attendance.Contracts.AcademicSessions;
using MediatR;

namespace Attendance.Application.Features.AcademicSessions.Commands.UpdateAcademicSession;

public record UpdateAcademicSessionCommand(
    Guid Id,
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive
) : IRequest<AcademicSessionResponse>;
