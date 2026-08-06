using Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attendance.Infrastructure.Persistence.Configurations;

public sealed class StudentCourseConfiguration
    : IEntityTypeConfiguration<StudentCourse>
{
    public void Configure(EntityTypeBuilder<StudentCourse> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Student)
            .WithMany(x => x.StudentCourses)
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Course)
            .WithMany(x => x.StudentCourses)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new
        {
            x.StudentId,
            x.CourseId
        }).IsUnique();
    }
}
