using MediatR;

namespace Attendance.Application.Features.Courses.Commands.DeleteCourse;

public record DeleteCourseCommand(Guid Id) : IRequest;
