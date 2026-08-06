using Attendance.Contracts.Semesters;
using MediatR;

namespace Attendance.Application.Features.Semesters.Queries.GetSemesterById;

public record GetSemesterByIdQuery(Guid Id)
    : IRequest<SemesterResponse>;
