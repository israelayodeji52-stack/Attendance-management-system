namespace Attendance.Application.Interfaces;

public interface IQrCodeService
{
    string GenerateQrCode(string matricNumber);
}