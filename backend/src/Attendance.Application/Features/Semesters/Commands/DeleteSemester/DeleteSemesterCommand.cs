using MediatR;

namespace Attendance.Application.Features.Semesters.Commands.DeleteSemester;

public record DeleteSemesterCommand(Guid Id) : IRequest;
