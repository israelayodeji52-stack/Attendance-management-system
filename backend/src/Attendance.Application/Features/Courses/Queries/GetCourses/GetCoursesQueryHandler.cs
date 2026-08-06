using Attendance.Application.Interfaces;
using Attendance.Contracts.Courses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Courses.Queries.GetCourses;

public sealed class GetCoursesQueryHandler
    : IRequestHandler<GetCoursesQuery, List<CourseResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetCoursesQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CourseResponse>> Handle(
        GetCoursesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Courses
            .AsNoTracking()
            .Select(course => new CourseResponse
            {
                Id = course.Id,
                Code = course.CourseCode,
                Title = course.CourseTitle,
                Unit = course.Units,
                SemesterId = course.SemesterId
            })
            .ToListAsync(cancellationToken);
    }
}
