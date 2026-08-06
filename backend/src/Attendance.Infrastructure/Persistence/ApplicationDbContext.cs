using Attendance.Application.Interfaces;
using Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();

    public DbSet<Course> Courses => Set<Course>();

    public DbSet<Semester> Semesters => Set<Semester>();

    public DbSet<AcademicSession> AcademicSessions => Set<AcademicSession>();

    public DbSet<StudentCourse> StudentCourses => Set<StudentCourse>();

    public DbSet<Attendance.Domain.Entities.Attendance> Attendances => Set<Attendance.Domain.Entities.Attendance>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
