using Attendance.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.StudentCourses.Commands.UpdateStudentCourse;

public sealed class UpdateStudentCourseCommandValidator
    : AbstractValidator<UpdateStudentCourseCommand>
{
    public UpdateStudentCourseCommandValidator(
        IApplicationDbContext context)
    {
        RuleFor(x => x.StudentId)
            .NotEmpty();

        RuleFor(x => x.CourseId)
            .NotEmpty();

        RuleFor(x => x)
            .MustAsync(async (command, cancellationToken) =>
            {
                return !await context.StudentCourses.AnyAsync(
                    x =>
                        x.StudentId == command.StudentId &&
                        x.CourseId == command.CourseId &&
                        x.Id != command.Id,
                    cancellationToken);
            })
            .WithMessage("Student is already registered for this course.");
    }
}
