using Attendance.Application.Interfaces;
using Attendance.Contracts.Attendance;
using Attendance.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AttendanceEntity = global::Attendance.Domain.Entities.Attendance;

namespace Attendance.Application.Features.Attendance.Commands.RecordAttendance;

public sealed class RecordAttendanceCommandHandler
    : IRequestHandler<
        RecordAttendanceCommand,
        RecordAttendanceResponse>
{
    private readonly IApplicationDbContext _context;

    public RecordAttendanceCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RecordAttendanceResponse> Handle(
        RecordAttendanceCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        // ==========================================
        // VALIDATION
        // ==========================================

        if (string.IsNullOrWhiteSpace(request.MatricNumber))
        {
            throw new ValidationException(
                "Matric number is required.");
        }

        if (request.CourseId == Guid.Empty)
        {
            throw new ValidationException(
                "Course is required.");
        }

        if (request.SemesterId == Guid.Empty)
        {
            throw new ValidationException(
                "Semester is required.");
        }

        if (request.AcademicSessionId == Guid.Empty)
        {
            throw new ValidationException(
                "Academic session is required.");
        }

        var matricNumber = request.MatricNumber.Trim();

        // ==========================================
        // FIND STUDENT
        // ==========================================

        var student = await _context.Users
            .FirstOrDefaultAsync(
                x =>
                    x.MatricNumber == matricNumber &&
                    x.Role == UserRole.Student,
                cancellationToken);

        if (student is null)
        {
            throw new ValidationException(
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
            throw new ValidationException(
                "Course not found.");
        }

        // ==========================================
        // FIND SEMESTER
        // ==========================================

        var semester = await _context.Semesters
            .FirstOrDefaultAsync(
                x => x.Id == request.SemesterId,
                cancellationToken);

        if (semester is null)
        {
            throw new ValidationException(
                "Semester not found.");
        }

        // ==========================================
        // FIND ACADEMIC SESSION
        // ==========================================

        var academicSession = await _context.AcademicSessions
            .FirstOrDefaultAsync(
                x => x.Id == request.AcademicSessionId,
                cancellationToken);

        if (academicSession is null)
        {
            throw new ValidationException(
                "Academic session not found.");
        }

        // ==========================================
        // VERIFY COURSE BELONGS TO SEMESTER
        // ==========================================

        if (course.SemesterId != semester.Id)
        {
            throw new ValidationException(
                "The selected course does not belong to the selected semester.");
        }

        // ==========================================
        // VERIFY STUDENT IS ENROLLED
        // ==========================================

        var enrolled = await _context.StudentCourses
            .AnyAsync(
                x =>
                    x.StudentId == student.Id &&
                    x.CourseId == course.Id,
                cancellationToken);

        if (!enrolled)
        {
            throw new ValidationException(
                "Student is not enrolled in this course.");
        }

        // ==========================================
        // CHECK DUPLICATE ATTENDANCE
        // ==========================================

        var today = DateTime.UtcNow.Date;

        var alreadyMarked = await _context.Attendances
            .AnyAsync(
                x =>
                    x.StudentId == student.Id &&
                    x.CourseId == course.Id &&
                    x.SemesterId == semester.Id &&
                    x.AcademicSessionId == academicSession.Id &&
                    x.AttendanceDate.Date == today,
                cancellationToken);

        if (alreadyMarked)
        {
            throw new ValidationException(
                "Attendance has already been recorded for this student today.");
        }

        // ==========================================
        // CREATE ATTENDANCE
        // ==========================================

        var attendance = new AttendanceEntity
        {
            StudentId = student.Id,
            CourseId = course.Id,
            SemesterId = semester.Id,
            AcademicSessionId = academicSession.Id,
            AttendanceDate = today,
            Status = AttendanceStatus.Present
        };

        _context.Attendances.Add(attendance);

        await _context.SaveChangesAsync(
            cancellationToken);

        // ==========================================
        // RETURN RESPONSE
        // ==========================================

        return new RecordAttendanceResponse
        {
            AttendanceId = attendance.Id,

            StudentId = student.Id,

            StudentNumber = student.StudentNumber,

            MatricNumber = student.MatricNumber,

            StudentName =
                $"{student.FirstName} {student.LastName}".Trim(),

            CourseCode = course.CourseCode,

            CourseTitle = course.CourseTitle,

            Semester = semester.Name,

            AcademicSession = academicSession.Name,

            AttendanceDate = attendance.AttendanceDate,

            Status = attendance.Status.ToString()
        };
    }
}