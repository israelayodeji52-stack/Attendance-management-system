using Attendance.Application.Interfaces;
using Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();

    public DbSet<Course> Courses => Set<Course>();

    public DbSet<Semester> Semesters => Set<Semester>();

    public DbSet<AcademicSession> AcademicSessions
        => Set<AcademicSession>();

    public DbSet<StudentCourse> StudentCourses
        => Set<StudentCourse>();

    public DbSet<Attendance.Domain.Entities.Attendance> Attendances
        => Set<Attendance.Domain.Entities.Attendance>();

    public DbSet<PasswordSetupToken> PasswordSetupTokens
        => Set<PasswordSetupToken>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =====================================================
        // PASSWORD SETUP TOKEN
        // =====================================================

        modelBuilder.Entity<PasswordSetupToken>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.TokenHash)
                .IsRequired();

            entity.HasIndex(x => x.TokenHash)
                .IsUnique();

            entity.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // =====================================================
        // STUDENT COURSE
        // =====================================================

        modelBuilder.Entity<StudentCourse>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Student)
                .WithMany(x => x.StudentCourses)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new
            {
                x.StudentId,
                x.CourseId
            })
            .IsUnique();
        });

        // =====================================================
        // ATTENDANCE
        // =====================================================

        modelBuilder.Entity<Attendance.Domain.Entities.Attendance>(
            entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.AttendanceDate)
                    .IsRequired();

                entity.Property(x => x.Status)
                    .IsRequired();

                // Student → Attendance
                entity.HasOne(x => x.Student)
                    .WithMany(x => x.Attendances)
                    .HasForeignKey(x => x.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Course → Attendance
                entity.HasOne(x => x.Course)
                    .WithMany()
                    .HasForeignKey(x => x.CourseId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Semester → Attendance
                entity.HasOne(x => x.Semester)
                    .WithMany()
                    .HasForeignKey(x => x.SemesterId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Academic Session → Attendance
                entity.HasOne(x => x.AcademicSession)
                    .WithMany()
                    .HasForeignKey(x => x.AcademicSessionId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Prevent duplicate attendance records
                entity.HasIndex(x => new
                {
                    x.StudentId,
                    x.CourseId,
                    x.SemesterId,
                    x.AcademicSessionId,
                    x.AttendanceDate
                })
                .IsUnique();
            });
    }
}