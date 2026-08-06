using Attendance.Contracts.Students;
using FluentValidation;

namespace Attendance.Application.Features.Students.Commands.CreateStudent.Validators;

public sealed class CreateStudentCommandValidator
    : AbstractValidator<CreateStudentRequest>
{
    public CreateStudentCommandValidator()
    {
        RuleFor(x => x.MatricNumber)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
