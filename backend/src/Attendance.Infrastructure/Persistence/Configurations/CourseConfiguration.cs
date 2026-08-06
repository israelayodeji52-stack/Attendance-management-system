using Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attendance.Infrastructure.Persistence.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CourseCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.CourseTitle)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Units)
            .IsRequired();

        builder.HasOne(c => c.Semester)
            .WithMany(s => s.Courses)
            .HasForeignKey(c => c.SemesterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
