using Attendance.Contracts.Students;

namespace Attendance.Application.Interfaces;

public interface IExcelService
{
    Task<IEnumerable<CreateStudentRequest>> ReadStudentsAsync(
        Stream fileStream);
}
