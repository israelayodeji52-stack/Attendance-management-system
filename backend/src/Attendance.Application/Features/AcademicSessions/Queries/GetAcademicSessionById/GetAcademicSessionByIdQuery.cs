using Attendance.Contracts.AcademicSessions;
using MediatR;

namespace Attendance.Application.Features.AcademicSessions.Queries.GetAcademicSessionById;

public record GetAcademicSessionByIdQuery(Guid Id)
    : IRequest<AcademicSessionResponse>;
