using Attendance.Application.Interfaces;
using Attendance.Contracts.Attendances;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Attendance.Application.Features.Attendances.Queries.GetAttendanceById;

public sealed class GetAttendanceByIdQueryHandlerImpl
    : IRequestHandler<GetAttendanceByIdQuery, AttendanceResponse>
{
    private readonly IApplicationDbContext _context;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public GetAttendanceByIdQueryHandlerImpl(IApplicationDbContext context)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        _context = context;
    }

    public async Task<AttendanceResponse> Handle(
        GetAttendanceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var attendance = await _context.Attendances
            .Include(x => x.Student)
            .Include(x => x.Course)
            .Include(x => x.Semester)
            .Include(x => x.AcademicSession)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (attendance is null)
            throw new ValidationException("Attendance record not found.");

        return new AttendanceResponse
        {
            Id = attendance.Id,
            StudentId = attendance.StudentId,
            StudentName = $"{attendance.Student.FirstName} {attendance.Student.LastName}",
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