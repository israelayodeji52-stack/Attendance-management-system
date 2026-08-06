using Attendance.Application.Interfaces;
using Attendance.Contracts.StudentCourses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.StudentCourses.Commands.UpdateStudentCourse;

public sealed class UpdateStudentCourseCommandHandler
    : IRequestHandler<UpdateStudentCourseCommand, StudentCourseResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateStudentCourseCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentCourseResponse> Handle(
        UpdateStudentCourseCommand request,
        CancellationToken cancellationToken)
    {
        var studentCourse = await _context.StudentCourses
            .Include(x => x.Student)
            .Include(x => x.Course)
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (studentCourse is null)
            throw new ValidationException(
                "Student course record not found.");

        var studentExists = await _context.Users.AnyAsync(
            x => x.Id == request.StudentId,
            cancellationToken);

        if (!studentExists)
            throw new ValidationException(
                "Student not found.");

        var courseExists = await _context.Courses.AnyAsync(
            x => x.Id == request.CourseId,
            cancellationToken);

        if (!courseExists)
            throw new ValidationException(
                "Course not found.");

        studentCourse.StudentId = request.StudentId;
        studentCourse.CourseId = request.CourseId;

        await _context.SaveChangesAsync(cancellationToken);

        studentCourse = await _context.StudentCourses
            .Include(x => x.Student)
            .Include(x => x.Course)
            .FirstAsync(
                x => x.Id == request.Id,
                cancellationToken);

        return new StudentCourseResponse
        {
            Id = studentCourse.Id,
            StudentId = studentCourse.StudentId,
            StudentName =
                $"{studentCourse.Student.FirstName} {studentCourse.Student.LastName}",
            CourseId = studentCourse.CourseId,
            CourseCode = studentCourse.Course.CourseCode,
            CourseTitle = studentCourse.Course.CourseTitle
        };
    }
}
