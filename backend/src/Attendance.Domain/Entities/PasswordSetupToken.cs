using Attendance.Domain.Common;

namespace Attendance.Domain.Entities;

public class PasswordSetupToken : BaseEntity
{
    public Guid StudentId { get; set; }

    public ApplicationUser Student { get; set; } = null!;

    public string TokenHash { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }
}