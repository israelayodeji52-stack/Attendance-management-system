using Attendance.Contracts.Semesters;
using MediatR;

namespace Attendance.Application.Features.Semesters.Commands.UpdateSemester;

public record UpdateSemesterCommand(
    Guid Id,
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive
) : IRequest<SemesterResponse>;
