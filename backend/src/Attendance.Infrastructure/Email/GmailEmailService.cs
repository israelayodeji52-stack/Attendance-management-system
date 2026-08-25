using Attendance.Application.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Attendance.Infrastructure.Email;

public sealed class GmailEmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public GmailEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAsync(
        string email,
        string subject,
        string body,
        CancellationToken cancellationToken)
    {
        await SendInternalAsync(
            email,
            subject,
            body,
            cancellationToken);
    }

    public async Task SendEmailAsync(
        string to,
        string subject,
        string body)
    {
        await SendInternalAsync(
            to,
            subject,
            body,
            CancellationToken.None);
    }

    private async Task SendInternalAsync(
        string to,
        string subject,
        string body,
        CancellationToken cancellationToken)
    {
        var gmailAddress =
            _configuration["Email:GmailAddress"];

        var gmailAppPassword =
            _configuration["Email:GmailAppPassword"];

        if (string.IsNullOrWhiteSpace(gmailAddress))
        {
            throw new InvalidOperationException(
                "Email:GmailAddress is not configured.");
        }

        if (string.IsNullOrWhiteSpace(gmailAppPassword))
        {
            throw new InvalidOperationException(
                "Email:GmailAppPassword is not configured.");
        }

        Console.WriteLine("=========================================");
        Console.WriteLine("[EMAIL] Preparing email");
        Console.WriteLine($"[EMAIL] From: {gmailAddress}");
        Console.WriteLine($"[EMAIL] To: {to}");
        Console.WriteLine("[EMAIL] SMTP: smtp.gmail.com:465");
        Console.WriteLine("[EMAIL] Security: SSL/TLS");
        Console.WriteLine("=========================================");

        var message = new MimeMessage();

        message.From.Add(
            new MailboxAddress(
                "Attendance Management System",
                gmailAddress));

        message.To.Add(
            MailboxAddress.Parse(to));

        message.Subject = subject;

        message.Body = new BodyBuilder
        {
            HtmlBody = body
        }.ToMessageBody();

        using var smtp = new SmtpClient();

        try
        {
            Console.WriteLine(
                "[EMAIL] Connecting to Gmail...");

            cancellationToken.ThrowIfCancellationRequested();

            // Gmail SMTP submission over implicit TLS.
            await smtp.ConnectAsync(
                "smtp.gmail.com",
                465,
                SecureSocketOptions.SslOnConnect,
                cancellationToken);

            Console.WriteLine(
                "[EMAIL] Connected to Gmail.");

            Console.WriteLine(
                "[EMAIL] Authenticating...");

            await smtp.AuthenticateAsync(
                gmailAddress,
                gmailAppPassword,
                cancellationToken);

            Console.WriteLine(
                "[EMAIL] Authentication successful.");

            Console.WriteLine(
                "[EMAIL] Sending email...");

            await smtp.SendAsync(
                message,
                cancellationToken);

            Console.WriteLine(
                "[EMAIL] Email sent successfully.");

            await smtp.DisconnectAsync(
                true,
                cancellationToken);

            Console.WriteLine(
                "[EMAIL] Disconnected from Gmail.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("[EMAIL] SMTP ERROR");
            Console.WriteLine("=========================================");
            Console.WriteLine(
                $"Exception: {ex.GetType().Name}");
            Console.WriteLine(
                $"Message: {ex.Message}");

            if (ex.InnerException != null)
            {
                Console.WriteLine(
                    $"Inner Exception: {ex.InnerException.Message}");
            }

            Console.WriteLine("=========================================");

            try
            {
                if (smtp.IsConnected)
                {
                    await smtp.DisconnectAsync(
                        true,
                        CancellationToken.None);
                }
            }
            catch
            {
                // Ignore disconnect errors.
            }

            throw new InvalidOperationException(
                $"Gmail email sending failed: {ex.Message}",
                ex);
        }
    }
}