using Attendance.Application.Interfaces;
using Attendance.Contracts.Courses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Courses.Commands.UpdateCourse;

public sealed class UpdateCourseCommandHandler
    : IRequestHandler<UpdateCourseCommand, CourseResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateCourseCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CourseResponse> Handle(
        UpdateCourseCommand request,
        CancellationToken cancellationToken)
    {
        var course = await _context.Courses
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (course is null)
            throw new KeyNotFoundException("Course not found.");

        var semesterExists = await _context.Semesters
            .AnyAsync(
                x => x.Id == request.SemesterId,
                cancellationToken);

        if (!semesterExists)
            throw new KeyNotFoundException("Semester not found.");

        var duplicateCode = await _context.Courses.AnyAsync(
            x => x.CourseCode == request.Code &&
                 x.Id != request.Id,
            cancellationToken);

        if (duplicateCode)
            throw new ValidationException("Course code already exists.");

        course.CourseCode = request.Code;
        course.CourseTitle = request.Title;
        course.Units = request.Unit;
        course.SemesterId = request.SemesterId;

        await _context.SaveChangesAsync(cancellationToken);

        return new CourseResponse
        {
            Id = course.Id,
            Code = course.CourseCode,
            Title = course.CourseTitle,
            Unit = course.Units,
            SemesterId = course.SemesterId
        };
    }
}
