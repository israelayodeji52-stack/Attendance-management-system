using Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> Users { get; }

    DbSet<Course> Courses { get; }

    DbSet<Semester> Semesters { get; }

    DbSet<AcademicSession> AcademicSessions { get; }

    DbSet<StudentCourse> StudentCourses { get; }

    DbSet<Attendance.Domain.Entities.Attendance> Attendances { get; }

    DbSet<PasswordSetupToken> PasswordSetupTokens { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken);
}