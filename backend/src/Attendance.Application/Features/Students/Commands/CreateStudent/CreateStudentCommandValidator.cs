using FluentValidation;

namespace Attendance.Application.Features.Students.Commands.CreateStudent;

public sealed class CreateStudentCommandValidator
    : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidator()
    {
        RuleFor(x => x.MatricNumber)
            .NotEmpty()
            .WithMessage("Matric Number is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First Name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last Name is required.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("A valid email address is required.");
    }
}