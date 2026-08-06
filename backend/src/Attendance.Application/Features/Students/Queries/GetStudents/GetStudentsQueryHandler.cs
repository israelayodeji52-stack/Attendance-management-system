using Attendance.Application.Interfaces;
using Attendance.Contracts.Students;
using Attendance.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Students.Queries.GetStudents;

public class GetStudentsQueryHandler
    : IRequestHandler<GetStudentsQuery, List<StudentResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<StudentResponse>> Handle(
        GetStudentsQuery request,
        CancellationToken cancellationToken)
    {
        var students = await _context.Users
            .Where(x => x.Role == UserRole.Student)
            .OrderBy(x => x.FirstName)
            .ToListAsync(cancellationToken);

        var response = students.Select(student => new StudentResponse
        {
            Id = student.Id,
            StudentNumber = student.StudentNumber,
            MatricNumber = student.MatricNumber,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            Role = student.Role.ToString(),
            IsEmailConfirmed = student.IsEmailConfirmed
        }).ToList();

        return response;
    }
}
