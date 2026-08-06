using Attendance.Application.Interfaces;
using Attendance.Contracts.Courses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Courses.Queries.GetCourseById;

public sealed class GetCourseByIdQueryHandler
    : IRequestHandler<GetCourseByIdQuery, CourseResponse>
{
    private readonly IApplicationDbContext _context;

    public GetCourseByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CourseResponse> Handle(
        GetCourseByIdQuery request,
        CancellationToken cancellationToken)
    {
        var course = await _context.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (course is null)
            throw new KeyNotFoundException("Course not found.");

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
