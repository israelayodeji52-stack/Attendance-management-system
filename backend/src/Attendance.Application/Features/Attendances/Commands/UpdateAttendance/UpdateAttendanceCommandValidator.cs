using FluentValidation;

namespace Attendance.Application.Features.Attendances.Commands.UpdateAttendance;

public sealed class UpdateAttendanceCommandValidator
    : AbstractValidator<UpdateAttendanceCommand>
{
    public UpdateAttendanceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.StudentId)
            .NotEmpty();

        RuleFor(x => x.CourseId)
            .NotEmpty();

        RuleFor(x => x.SemesterId)
            .NotEmpty();

        RuleFor(x => x.AcademicSessionId)
            .NotEmpty();
    }
}
