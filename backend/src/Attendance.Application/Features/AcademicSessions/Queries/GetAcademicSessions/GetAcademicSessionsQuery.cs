using Attendance.Contracts.AcademicSessions;
using MediatR;

namespace Attendance.Application.Features.AcademicSessions.Queries.GetAcademicSessions;

public record GetAcademicSessionsQuery()
    : IRequest<IReadOnlyList<AcademicSessionResponse>>;
