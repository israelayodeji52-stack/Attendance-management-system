using FluentValidation;

namespace Attendance.Application.Features.Courses.Commands.UpdateCourse;

public class UpdateCourseCommandValidator
    : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Unit)
            .GreaterThan(0);

        RuleFor(x => x.SemesterId)
            .NotEmpty();
    }
}
