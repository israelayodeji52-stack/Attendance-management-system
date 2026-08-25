using Attendance.Application.Interfaces;
using Attendance.Contracts.Attendances;
using Attendance.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Attendance.Application.Features.Attendances.Commands.UpdateAttendance;

public sealed class UpdateAttendanceCommandHandler
    : IRequestHandler<UpdateAttendanceCommand, AttendanceResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateAttendanceCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AttendanceResponse> Handle(
        UpdateAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        var attendance = await _context.Attendances
            .Include(x => x.Student)
            .Include(x => x.Course)
            .Include(x => x.Semester)
            .Include(x => x.AcademicSession)
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (attendance is null)
            throw new ValidationException(
                "Attendance record not found.");

        if (!Enum.TryParse<AttendanceStatus>(
                request.Status,
                true,
                out var status))
        {
            throw new ValidationException(
                $"Invalid attendance status: {request.Status}. " +
                "Valid statuses are Present, Late, and Absent.");
        }

        attendance.Status = status;

        await _context.SaveChangesAsync(cancellationToken);

        return new AttendanceResponse
        {
            Id = attendance.Id,

            StudentId = attendance.StudentId,
            StudentName =
                $"{attendance.Student.FirstName} {attendance.Student.LastName}",

            CourseId = attendance.CourseId,
            CourseCode = attendance.Course.CourseCode,
            CourseTitle = attendance.Course.CourseTitle,

            SemesterId = attendance.SemesterId,
            SemesterName = attendance.Semester.Name,

            AcademicSessionId = attendance.AcademicSessionId,
            AcademicSessionName = attendance.AcademicSession.Name,

            Status = attendance.Status.ToString(),

            AttendanceDate = attendance.AttendanceDate
        };
    }
}