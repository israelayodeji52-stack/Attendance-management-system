using Attendance.Application.Interfaces;
using Attendance.Contracts.StudentCourses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.StudentCourses.Queries.GetStudentCourseById;

public sealed class GetStudentCourseByIdQueryHandler
    : IRequestHandler<GetStudentCourseByIdQuery, StudentCourseResponse>
{
    private readonly IApplicationDbContext _context;

    public GetStudentCourseByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentCourseResponse> Handle(
        GetStudentCourseByIdQuery request,
        CancellationToken cancellationToken)
    {
        var studentCourse = await _context.StudentCourses
            .Include(x => x.Student)
            .Include(x => x.Course)
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (studentCourse is null)
            throw new KeyNotFoundException("Student course record not found.");

        return new StudentCourseResponse
        {
            Id = studentCourse.Id,
            StudentId = studentCourse.StudentId,
            StudentName = $"{studentCourse.Student.FirstName} {studentCourse.Student.LastName}",
            CourseId = studentCourse.CourseId,
            CourseCode = studentCourse.Course.CourseCode,
            CourseTitle = studentCourse.Course.CourseTitle
        };
    }
}
