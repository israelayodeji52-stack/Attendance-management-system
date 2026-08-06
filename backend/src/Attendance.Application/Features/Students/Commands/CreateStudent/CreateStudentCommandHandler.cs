using Attendance.Application.Interfaces;
using Attendance.Contracts.Students;
using Attendance.Domain.Entities;
using Attendance.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Students.Commands.CreateStudent;

public sealed class CreateStudentCommandHandler
    : IRequestHandler<CreateStudentCommand, StudentResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateStudentCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentResponse> Handle(
        CreateStudentCommand request,
        CancellationToken cancellationToken)
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(request.MatricNumber))
            throw new ValidationException("Matric Number is required.");

        if (string.IsNullOrWhiteSpace(request.FirstName))
            throw new ValidationException("First Name is required.");

        if (string.IsNullOrWhiteSpace(request.LastName))
            throw new ValidationException("Last Name is required.");

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ValidationException("Email is required.");

        var emailExists = await _context.Users.AnyAsync(
            x => x.Email == request.Email,
            cancellationToken);

        if (emailExists)
        {
            throw new ValidationException("Email already exists.");
        }

        var student = new ApplicationUser
        {
            StudentNumber = $"STU{DateTime.UtcNow:yyyyMMddHHmmss}",
            MatricNumber = request.MatricNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = string.Empty,
            Role = UserRole.Student,
            IsEmailConfirmed = false
        };

        _context.Users.Add(student);

        await _context.SaveChangesAsync(cancellationToken);

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
