using Attendance.Contracts.StudentCourses;
using MediatR;

namespace Attendance.Application.Features.StudentCourses.Commands.CreateStudentCourse;

public sealed record CreateStudentCourseCommand(
    CreateStudentCourseRequest Request)
    : IRequest<StudentCourseResponse>;
