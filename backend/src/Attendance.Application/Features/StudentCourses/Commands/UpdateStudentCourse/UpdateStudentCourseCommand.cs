using Attendance.Contracts.StudentCourses;
using MediatR;

namespace Attendance.Application.Features.StudentCourses.Commands.UpdateStudentCourse;

public sealed record UpdateStudentCourseCommand(
    Guid Id,
    Guid StudentId,
    Guid CourseId)
    : IRequest<StudentCourseResponse>;
