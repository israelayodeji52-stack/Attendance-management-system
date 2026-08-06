using Attendance.Contracts.Semesters;
using MediatR;

namespace Attendance.Application.Features.Semesters.Commands.CreateSemester;

public record CreateSemesterCommand(
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive,
    Guid AcademicSessionId
) : IRequest<SemesterResponse>
{
    public CreateSemesterCommand(CreateSemesterRequest request)
        : this(
            request.Name,
            request.StartDate,
            request.EndDate,
            request.IsActive,
            request.AcademicSessionId)
    {
    }
}
