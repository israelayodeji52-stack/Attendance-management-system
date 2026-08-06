using Attendance.Contracts.Courses;
using MediatR;

namespace Attendance.Application.Features.Courses.Commands.CreateCourse;

public record CreateCourseCommand(
    CreateCourseRequest Request)
    : IRequest<CourseResponse>;
