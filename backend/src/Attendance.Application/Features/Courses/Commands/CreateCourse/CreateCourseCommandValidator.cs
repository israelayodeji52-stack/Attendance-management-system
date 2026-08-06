using FluentValidation;

namespace Attendance.Application.Features.Courses.Commands.CreateCourse;

public class CreateCourseCommandValidator
    : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Request.Code)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Request.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Request.Unit)
            .GreaterThan(0);

        RuleFor(x => x.Request.SemesterId)
            .NotEmpty();
    }
}
