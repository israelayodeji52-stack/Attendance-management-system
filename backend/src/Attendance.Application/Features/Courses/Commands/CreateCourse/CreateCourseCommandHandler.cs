using Attendance.Application.Interfaces;
using Attendance.Contracts.Courses;
using Attendance.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Courses.Commands.CreateCourse;

public sealed class CreateCourseCommandHandler
    : IRequestHandler<CreateCourseCommand, CourseResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateCourseCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CourseResponse> Handle(
        CreateCourseCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Request.Code))
            throw new ValidationException("Course code is required.");

        if (string.IsNullOrWhiteSpace(request.Request.Title))
            throw new ValidationException("Course title is required.");

        var semesterExists = await _context.Semesters.AnyAsync(
            x => x.Id == request.Request.SemesterId,
            cancellationToken);

        if (!semesterExists)
            throw new ValidationException("Semester does not exist.");

        var codeExists = await _context.Courses.AnyAsync(
            x => x.CourseCode == request.Request.Code,
            cancellationToken);

        if (codeExists)
            throw new ValidationException("Course code already exists.");

        var course = new Course
        {
            CourseCode = request.Request.Code,
            CourseTitle = request.Request.Title,
            Units = request.Request.Unit,
            SemesterId = request.Request.SemesterId
        };

        _context.Courses.Add(course);

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
