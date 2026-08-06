using Attendance.Contracts.Students;
using MediatR;

namespace Attendance.Application.Features.Students.Queries.GetStudentById;

public record GetStudentByIdQuery(Guid Id)
    : IRequest<StudentResponse>;
