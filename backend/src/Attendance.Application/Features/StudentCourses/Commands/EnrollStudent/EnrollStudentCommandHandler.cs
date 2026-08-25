using Attendance.Application.Interfaces;
using Attendance.Contracts.StudentCourses;
using Attendance.Domain.Entities;
using Attendance.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.StudentCourses.Commands.EnrollStudent;

public sealed class EnrollStudentCommandHandler
    : IRequestHandler<EnrollStudentCommand, EnrollStudentResponse>
{
    private readonly IApplicationDbContext _context;

    public EnrollStudentCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EnrollStudentResponse> Handle(
        EnrollStudentCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        // ==========================================
        // VALIDATION
        // ==========================================

        if (request.StudentId == Guid.Empty)
        {
            throw new ValidationException(
                "Student is required.");
        }

        if (request.CourseId == Guid.Empty)
        {
            throw new ValidationException(
                "Course is required.");
        }

        // ==========================================
        // FIND STUDENT
        // ==========================================

        var student = await _context.Users
            .FirstOrDefaultAsync(
                x =>
                    x.Id == request.StudentId &&
                    x.Role == UserRole.Student,
                cancellationToken);

        if (student is null)
        {
            throw new KeyNotFoundException(
                "Student not found.");
        }

        // ==========================================
        // FIND COURSE
        // ==========================================

        var course = await _context.Courses
            .FirstOrDefaultAsync(
                x => x.Id == request.CourseId,
                cancellationToken);

        if (course is null)
        {
            throw new KeyNotFoundException(
                "Course not found.");
        }

        // ==========================================
        // CHECK EXISTING ENROLLMENT
        // ==========================================

        var existingEnrollment =
            await _context.StudentCourses
                .FirstOrDefaultAsync(
                    x =>
                        x.StudentId == student.Id &&
                        x.CourseId == course.Id,
                    cancellationToken);

        if (existingEnrollment is not null)
        {
            return new EnrollStudentResponse
            {
                Id = existingEnrollment.Id,

                StudentId = student.Id,

                StudentNumber = student.StudentNumber,

                MatricNumber = student.MatricNumber,

                StudentName =
                    $"{student.FirstName} {student.LastName}".Trim(),

                CourseId = course.Id,

                CourseCode = course.CourseCode,

                CourseTitle = course.CourseTitle,

                AlreadyEnrolled = true
            };
        }

        // ==========================================
        // CREATE ENROLLMENT
        // ==========================================

        var enrollment = new StudentCourse
        {
            StudentId = student.Id,
            CourseId = course.Id
        };

        _context.StudentCourses.Add(enrollment);

        await _context.SaveChangesAsync(
            cancellationToken);

        // ==========================================
        // RETURN RESPONSE
        // ==========================================

        return new EnrollStudentResponse
        {
            Id = enrollment.Id,

            StudentId = student.Id,

            StudentNumber = student.StudentNumber,

            MatricNumber = student.MatricNumber,

            StudentName =
                $"{student.FirstName} {student.LastName}".Trim(),

            CourseId = course.Id,

            CourseCode = course.CourseCode,

            CourseTitle = course.CourseTitle,

            AlreadyEnrolled = false
        };
    }
}