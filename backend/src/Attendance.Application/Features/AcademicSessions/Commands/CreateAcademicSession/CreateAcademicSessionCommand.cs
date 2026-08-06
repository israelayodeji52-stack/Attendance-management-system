using Attendance.Contracts.AcademicSessions;
using MediatR;

namespace Attendance.Application.Features.AcademicSessions.Commands.CreateAcademicSession;

public record CreateAcademicSessionCommand(
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive
) : IRequest<AcademicSessionResponse>
{
    public CreateAcademicSessionCommand(CreateAcademicSessionRequest request)
        : this(
            request.Name,
            request.StartDate,
            request.EndDate,
            request.IsActive)
    {
    }
}
