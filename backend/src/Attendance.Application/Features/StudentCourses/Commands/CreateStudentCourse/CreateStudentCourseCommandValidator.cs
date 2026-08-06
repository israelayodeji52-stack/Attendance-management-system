using Attendance.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.StudentCourses.Commands.CreateStudentCourse;

public sealed class CreateStudentCourseCommandValidator
    : AbstractValidator<CreateStudentCourseCommand>
{
    public CreateStudentCourseCommandValidator(
        IApplicationDbContext context)
    {
        RuleFor(x => x.Request.StudentId)
            .NotEmpty();

        RuleFor(x => x.Request.CourseId)
            .NotEmpty();

        RuleFor(x => x)
            .MustAsync(async (command, cancellationToken) =>
            {
                return !await context.StudentCourses.AnyAsync(
                    x =>
                        x.StudentId == command.Request.StudentId &&
                        x.CourseId == command.Request.CourseId,
                    cancellationToken);
            })
            .WithMessage("Student is already registered for this course.");
    }
}
