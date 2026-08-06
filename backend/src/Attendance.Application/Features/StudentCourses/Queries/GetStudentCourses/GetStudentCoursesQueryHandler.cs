using Attendance.Application.Interfaces;
using Attendance.Contracts.StudentCourses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.StudentCourses.Queries.GetStudentCourses;

public sealed class GetStudentsCoursesQueryHandler
    : IRequestHandler<GetStudentsCoursesQuery, List<StudentCourseResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentsCoursesQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<StudentCourseResponse>> Handle(
        GetStudentsCoursesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.StudentCourses
            .Include(x => x.Student)
            .Include(x => x.Course)
            .Select(x => new StudentCourseResponse
            {
                Id = x.Id,
                StudentId = x.StudentId,
                StudentName = $"{x.Student.FirstName} {x.Student.LastName}",
                CourseId = x.CourseId,
                CourseCode = x.Course.CourseCode,
                CourseTitle = x.Course.CourseTitle
            })
            .ToListAsync(cancellationToken);
    }
}

public sealed class GetStudentsCoursesQuery : IRequest<List<StudentCourseResponse>>
{
}
