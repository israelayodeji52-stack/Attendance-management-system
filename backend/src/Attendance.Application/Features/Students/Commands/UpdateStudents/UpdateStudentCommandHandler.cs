using Attendance.Application.Interfaces;
using Attendance.Contracts.Students;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Features.Students.Commands.UpdateStudent;

public sealed class UpdateStudentCommandHandler
    : IRequestHandler<UpdateStudentCommand, StudentResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateStudentCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentResponse> Handle(
        UpdateStudentCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (student is null)
            throw new KeyNotFoundException("Student not found.");

        if (string.IsNullOrWhiteSpace(request.Request.MatricNumber))
            throw new ValidationException("Matric Number is required.");

        if (string.IsNullOrWhiteSpace(request.Request.FirstName))
            throw new ValidationException("First Name is required.");

        if (string.IsNullOrWhiteSpace(request.Request.LastName))
            throw new ValidationException("Last Name is required.");

        if (string.IsNullOrWhiteSpace(request.Request.Email))
            throw new ValidationException("Email is required.");

        var emailExists = await _context.Users.AnyAsync(
            x => x.Email == request.Request.Email &&
                 x.Id != request.Id,
            cancellationToken);

        if (emailExists)
            throw new ValidationException("Email already exists.");

        student.MatricNumber = request.Request.MatricNumber;
        student.FirstName = request.Request.FirstName;
        student.LastName = request.Request.LastName;
        student.Email = request.Request.Email;

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
