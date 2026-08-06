using System.ComponentModel.DataAnnotations;
using Attendance.Application.Interfaces;
using Attendance.Contracts.Attendances;
using Attendance.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Attendances.Commands.MarkAttendance;

public sealed class MarkAttendanceCommandHandler
    : IRequestHandler<MarkAttendanceCommand, AttendanceResponse>
{
    private readonly IApplicationDbContext _dbContext;

    public MarkAttendanceCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AttendanceResponse> Handle(
        MarkAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == request.Request.StudentId, cancellationToken);

        if (student is null)
            throw new ValidationException("Student not found.");

        var course = await _dbContext.Courses
            .FirstOrDefaultAsync(x => x.Id == request.Request.CourseId, cancellationToken);

        if (course is null)
            throw new ValidationException("Course not found.");

        var semester = await _dbContext.Semesters
            .FirstOrDefaultAsync(x => x.Id == request.Request.SemesterId, cancellationToken);

        if (semester is null)
            throw new ValidationException("Semester not found.");

        var session = await _dbContext.AcademicSessions
            .FirstOrDefaultAsync(x => x.Id == request.Request.AcademicSessionId, cancellationToken);

        if (session is null)
            throw new ValidationException("Academic session not found.");

        var alreadyMarked = await _dbContext.Attendances.AnyAsync(
            x =>
                x.StudentId == request.Request.StudentId &&
                x.CourseId == request.Request.CourseId &&
                x.SemesterId == request.Request.SemesterId &&
                x.AcademicSessionId == request.Request.AcademicSessionId &&
                x.AttendanceDate.Date == DateTime.UtcNow.Date,
            cancellationToken);

        if (alreadyMarked)
            throw new ValidationException("Attendance has already been marked today.");

        var attendance = new Attendance.Domain.Entities.Attendance
        {
            StudentId = request.Request.StudentId,
            CourseId = request.Request.CourseId,
            SemesterId = request.Request.SemesterId,
            AcademicSessionId = request.Request.AcademicSessionId,
            AttendanceDate = DateTime.UtcNow,
            Status = AttendanceStatus.Present
        };

        _dbContext.Attendances.Add(attendance);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new AttendanceResponse
        {
            Id = attendance.Id,
            StudentId = attendance.StudentId,
            StudentName = $"{student.FirstName} {student.LastName}",
            CourseId = attendance.CourseId,
            CourseCode = course.CourseCode,
            CourseTitle = course.CourseTitle,
            SemesterId = attendance.SemesterId,
            SemesterName = semester.Name,
            AcademicSessionId = attendance.AcademicSessionId,
            AcademicSessionName = session.Name,
            Status = attendance.Status.ToString(),
            AttendanceDate = attendance.AttendanceDate
        };
    }
}