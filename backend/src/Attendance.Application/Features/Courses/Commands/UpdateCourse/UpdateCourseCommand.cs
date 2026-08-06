using Attendance.Contracts.Courses;
using MediatR;

namespace Attendance.Application.Features.Courses.Commands.UpdateCourse;

public record UpdateCourseCommand(
    Guid Id,
    string Code,
    string Title,
    int Unit,
    Guid SemesterId
) : IRequest<CourseResponse>;
