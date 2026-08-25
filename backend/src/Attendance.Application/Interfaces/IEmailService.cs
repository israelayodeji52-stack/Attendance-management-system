namespace Attendance.Application.Interfaces;

public interface IEmailService
{
    Task SendAsync(string email, string v1, string v2, CancellationToken cancellationToken);
    Task SendEmailAsync(
        string to,
        string subject,
        string body);
}
