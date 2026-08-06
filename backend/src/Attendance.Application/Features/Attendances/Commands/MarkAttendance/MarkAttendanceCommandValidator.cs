using FluentValidation;

namespace Attendance.Application.Features.Attendances.Commands.MarkAttendance;

public sealed class MarkAttendanceCommandValidator
    : AbstractValidator<MarkAttendanceCommand>
{
    public MarkAttendanceCommandValidator()
    {
        RuleFor(x => x.Request.StudentId)
            .NotEmpty()
            .WithMessage("Student is required.");

        RuleFor(x => x.Request.CourseId)
            .NotEmpty()
            .WithMessage("Course is required.");

        RuleFor(x => x.Request.SemesterId)
            .NotEmpty()
            .WithMessage("Semester is required.");

        RuleFor(x => x.Request.AcademicSessionId)
            .NotEmpty()
            .WithMessage("Academic session is required.");
    }
}
