using Attendance.Contracts.Semesters;
using MediatR;

namespace Attendance.Application.Features.Semesters.Queries.GetSemesters;

public record GetSemestersQuery
    : IRequest<IEnumerable<SemesterResponse>>;
