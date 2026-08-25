using Attendance.Contracts.Students;
using MediatR;

namespace Attendance.Application.Features.Students.Commands.CreateStudent;

public sealed class CreateStudentCommand : IRequest<StudentResponse>
{
    public string MatricNumber { get; init; } = string.Empty;

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public CreateStudentCommand()
    {
    }

    public CreateStudentCommand(CreateStudentRequest request)
    {
        MatricNumber = request.MatricNumber;
        FirstName = request.FirstName;
        LastName = request.LastName;
        Email = request.Email;
    }
}