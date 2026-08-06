using Attendance.Contracts.Students;
using MediatR;

namespace Attendance.Application.Features.Students.Queries.GetStudents;

public record GetStudentsQuery()
    : IRequest<List<StudentResponse>>;
