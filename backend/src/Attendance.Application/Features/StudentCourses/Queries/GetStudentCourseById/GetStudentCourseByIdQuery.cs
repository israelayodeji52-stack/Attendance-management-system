using Attendance.Contracts.StudentCourses;
using MediatR;

namespace Attendance.Application.Features.StudentCourses.Queries.GetStudentCourseById;

public sealed record GetStudentCourseByIdQuery(Guid Id)
    : IRequest<StudentCourseResponse>;
