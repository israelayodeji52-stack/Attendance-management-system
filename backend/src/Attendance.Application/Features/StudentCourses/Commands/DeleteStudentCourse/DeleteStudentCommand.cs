using MediatR;

namespace Attendance.Application.Features.StudentCourses.Commands.DeleteStudentCourse;

public sealed record DeleteStudentCourseCommand(Guid Id)
    : IRequest;
