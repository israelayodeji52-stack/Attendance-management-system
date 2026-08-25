using Attendance.Contracts.StudentCourses;
using MediatR;

namespace Attendance.Application.Features.StudentCourses.Commands.EnrollStudent;

public sealed record EnrollStudentCommand(
    EnrollStudentRequest Request)
    : IRequest<EnrollStudentResponse>;