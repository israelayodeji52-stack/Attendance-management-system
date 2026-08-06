namespace Attendance.Application.Interfaces;

public interface IQrCodeService
{
    byte[] GenerateQrCode(string value);
}
