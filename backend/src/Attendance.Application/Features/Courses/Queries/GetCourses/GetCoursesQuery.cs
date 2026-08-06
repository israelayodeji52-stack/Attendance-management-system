using Attendance.Contracts.Courses;
using MediatR;

namespace Attendance.Application.Features.Courses.Queries.GetCourses;

public record GetCoursesQuery
    : IRequest<List<CourseResponse>>;
