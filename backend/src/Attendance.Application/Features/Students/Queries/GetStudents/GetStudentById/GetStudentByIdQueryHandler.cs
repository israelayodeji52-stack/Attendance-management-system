using Attendance.Application.Interfaces;
using Attendance.Contracts.Students;
using Attendance.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Students.Queries.GetStudentById;

public class GetStudentByIdQueryHandler
    : IRequestHandler<GetStudentByIdQuery, StudentResponse>
{
    private readonly IApplicationDbContext _context;

    public GetStudentByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentResponse> Handle(
        GetStudentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Id == request.Id &&
                     x.Role == UserRole.Student,
                cancellationToken);

        if (student is null)
        {
            throw new KeyNotFoundException("Student not found.");
        }

        return new StudentResponse
        {
            Id = student.Id,
            StudentNumber = student.StudentNumber,
            MatricNumber = student.MatricNumber,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            Role = student.Role.ToString(),
            IsEmailConfirmed = student.IsEmailConfirmed
        };
    }
}
