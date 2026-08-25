using System.Security.Cryptography;
using System.Text;
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
    private readonly IEmailService _emailService;
    private readonly IQrCodeService _qrCodeService;

    public CreateStudentCommandHandler(
        IApplicationDbContext context,
        IEmailService emailService,
        IQrCodeService qrCodeService)
    {
        _context = context;
        _emailService = emailService;
        _qrCodeService = qrCodeService;
    }

    public async Task<StudentResponse> Handle(
        CreateStudentCommand request,
        CancellationToken cancellationToken)
    {
        // ==========================================
        // VALIDATION
        // ==========================================

        if (string.IsNullOrWhiteSpace(request.MatricNumber))
            throw new ValidationException(
                "Matric Number is required.");

        if (string.IsNullOrWhiteSpace(request.FirstName))
            throw new ValidationException(
                "First Name is required.");

        if (string.IsNullOrWhiteSpace(request.LastName))
            throw new ValidationException(
                "Last Name is required.");

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ValidationException(
                "Email is required.");

        // ==========================================
        // NORMALIZE INPUT
        // ==========================================

        var email = request.Email
            .Trim()
            .ToLowerInvariant();

        var matricNumber = request.MatricNumber.Trim();
        var firstName = request.FirstName.Trim();
        var lastName = request.LastName.Trim();

        // ==========================================
        // CHECK DUPLICATE EMAIL
        // ==========================================

        var emailExists = await _context.Users
            .AnyAsync(
                x => x.Email.ToLower() == email,
                cancellationToken);

        if (emailExists)
        {
            throw new ValidationException(
                "Email already exists.");
        }

        // ==========================================
        // CHECK DUPLICATE MATRIC NUMBER
        // ==========================================

        var matricExists = await _context.Users
            .AnyAsync(
                x => x.MatricNumber == matricNumber,
                cancellationToken);

        if (matricExists)
        {
            throw new ValidationException(
                "Matric Number already exists.");
        }

        // ==========================================
        // GENERATE STUDENT NUMBER
        // ==========================================

        var studentNumber =
            $"STU{DateTime.UtcNow:yyyyMMddHHmmss}";

        // ==========================================
        // CREATE STUDENT
        // ==========================================

        var student = new ApplicationUser
        {
            StudentNumber = studentNumber,
            MatricNumber = matricNumber,
            FirstName = firstName,
            LastName = lastName,
            Email = email,

            // Password is created later through
            // the password setup link.
            PasswordHash = string.Empty,

            Role = UserRole.Student,

            IsEmailConfirmed = false,

            QrCode = null
        };

        _context.Users.Add(student);

        await _context.SaveChangesAsync(
            cancellationToken);

        // ==========================================
        // GENERATE QR CODE
        // ==========================================

        var qrCode = _qrCodeService.GenerateQrCode(
            matricNumber);

        student.QrCode = qrCode;

        await _context.SaveChangesAsync(
            cancellationToken);

        // ==========================================
        // GENERATE PASSWORD SETUP TOKEN
        // ==========================================

        var rawToken = Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(32));

        var tokenHash = HashToken(rawToken);

        var setupToken = new PasswordSetupToken
        {
            StudentId = student.Id,
            TokenHash = tokenHash,

            // Token is valid for 24 hours.
            ExpiresAt = DateTime.UtcNow.AddHours(24),

            IsUsed = false
        };

        _context.PasswordSetupTokens.Add(setupToken);

        await _context.SaveChangesAsync(
            cancellationToken);

        // ==========================================
        // BUILD PASSWORD SETUP LINK
        // ==========================================

        var setupLink =
            $"http://localhost:3000/account/setup-password" +
            $"?token={Uri.EscapeDataString(rawToken)}";

        // ==========================================
        // SEND PASSWORD SETUP EMAIL
        // ==========================================

        var subject =
            "Attendance Management System - Account Setup";

        var body =
            $"""
            <html>
            <body>
                <h2>Welcome to the Attendance Management System</h2>

                <p>Hello {student.FirstName},</p>

                <p>
                    Your student account has been created successfully.
                </p>

                <p>
                    <strong>Student Number:</strong>
                    {student.StudentNumber}
                </p>

                <p>
                    <strong>Matric Number:</strong>
                    {student.MatricNumber}
                </p>

                <p>
                    Please click the button below to create your password:
                </p>

                <p>
                    <a
                        href="{setupLink}"
                        style="
                            display:inline-block;
                            padding:12px 20px;
                            background:#2563eb;
                            color:white;
                            text-decoration:none;
                            border-radius:6px;
                        ">
                        Create Password
                    </a>
                </p>

                <p>
                    Or copy this link into your browser:
                </p>

                <p>{setupLink}</p>

                <p>
                    This link will expire in 24 hours.
                </p>

                <p>
                    If you did not expect this email,
                    please ignore it.
                </p>

                <p>
                    Regards,<br />
                    Attendance Management System
                </p>
            </body>
            </html>
            """;

        await _emailService.SendEmailAsync(
            student.Email,
            subject,
            body);

        // ==========================================
        // RETURN RESPONSE
        // ==========================================

        return new StudentResponse
        {
            Id = student.Id,

            StudentNumber = student.StudentNumber,

            MatricNumber = student.MatricNumber,

            FirstName = student.FirstName,

            LastName = student.LastName,

            Email = student.Email,

            Role = student.Role.ToString(),

            IsEmailConfirmed =
                student.IsEmailConfirmed,

            QrCode = student.QrCode
        };
    }

    // ==========================================
    // HASH SETUP TOKEN
    // ==========================================

    private static string HashToken(string token)
    {
        var bytes = SHA256.HashData(
            Encoding.UTF8.GetBytes(token));

        return Convert.ToHexString(bytes);
    }
}