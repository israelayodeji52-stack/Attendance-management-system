using FluentValidation;

namespace Attendance.Application.Features.Students.Commands.CreateStudent;

public class CreateStudentCommandValidator
    : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidator()
    {
        RuleFor(x => x.MatricNumber)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);
    }
}
