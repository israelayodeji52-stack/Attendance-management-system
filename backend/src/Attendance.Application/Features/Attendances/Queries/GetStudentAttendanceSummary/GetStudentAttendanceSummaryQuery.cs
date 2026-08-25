using Attendance.Contracts.Attendances;
using MediatR;

namespace Attendance.Application.Features.Attendances.Queries.GetStudentAttendanceSummary;

public sealed record GetStudentAttendanceSummaryQuery(Guid StudentId)
    : IRequest<StudentAttendanceSummaryResponse>;