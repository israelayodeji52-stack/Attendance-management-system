using Attendance.Application.Interfaces;
using Attendance.Contracts.StudentCourses;
using Attendance.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.StudentCourses.Commands.CreateStudentCourse;

public sealed class CreateStudentCourseCommandHandler
    : IRequestHandler<CreateStudentCourseCommand, StudentCourseResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateStudentCourseCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentCourseResponse> Handle(
        CreateStudentCourseCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Id == request.Request.StudentId,
                cancellationToken);

        if (student is null)
            throw new KeyNotFoundException("Student not found.");

        var course = await _context.Courses
            .FirstOrDefaultAsync(
                x => x.Id == request.Request.CourseId,
                cancellationToken);

        if (course is null)
            throw new KeyNotFoundException("Course not found.");

        var studentCourse = new StudentCourse
        {
            StudentId = student.Id,
            CourseId = course.Id
        };

        _context.StudentCourses.Add(studentCourse);

        await _context.SaveChangesAsync(cancellationToken);

        return new StudentCourseResponse
        {
            Id = studentCourse.Id,
            StudentId = student.Id,
            StudentName = $"{student.FirstName} {student.LastName}",
            CourseId = course.Id,
            CourseCode = course.CourseCode,
            CourseTitle = course.CourseTitle
        };
    }
}
