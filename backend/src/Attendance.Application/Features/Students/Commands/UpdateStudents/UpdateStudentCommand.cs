using Attendance.Contracts.Students;
using MediatR;

namespace Attendance.Application.Features.Students.Commands.UpdateStudent;

public record UpdateStudentCommand(
    Guid Id,
    UpdateStudentRequest Request
) : IRequest<StudentResponse>;
