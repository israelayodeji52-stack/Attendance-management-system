using Attendance.Contracts.Students;
using MediatR;

namespace Attendance.Application.Features.Students.Commands.CreateStudent;

public record CreateStudentCommand(
    string MatricNumber,
    string FirstName,
    string LastName,
    string Email
) : IRequest<StudentResponse>
{
    public CreateStudentCommand(CreateStudentRequest request)
        : this(
            request.MatricNumber,
            request.FirstName,
            request.LastName,
            request.Email)
    {
    }
}
