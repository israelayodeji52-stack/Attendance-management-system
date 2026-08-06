using Attendance.Contracts.Courses;
using MediatR;

namespace Attendance.Application.Features.Courses.Queries.GetCourseById;

public record GetCourseByIdQuery(Guid Id)
    : IRequest<CourseResponse>;
