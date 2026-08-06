using MediatR;

namespace Attendance.Application.Features.Students.Commands.DeleteStudent;

public record DeleteStudentCommand(Guid Id) : IRequest;
