using Attendance.Domain.Entities;

namespace Attendance.Application.Interfaces;

public interface IJwtTokenProvider
{
    string GenerateToken(ApplicationUser user);
}
