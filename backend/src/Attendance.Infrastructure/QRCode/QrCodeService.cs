using Attendance.Application.Interfaces;
using QRCoder;

namespace Attendance.Infrastructure.QRCode;

public sealed class QrCodeService : IQrCodeService
{
    public string GenerateQrCode(string matricNumber)
    {
        if (string.IsNullOrWhiteSpace(matricNumber))
        {
            throw new ArgumentException(
                "Matric number is required.",
                nameof(matricNumber));
        }

        using var qrGenerator = new QRCodeGenerator();

        using var qrData = qrGenerator.CreateQrCode(
            matricNumber,
            QRCodeGenerator.ECCLevel.Q);

        var pngQrCode = new PngByteQRCode(qrData);

        var qrCodeBytes = pngQrCode.GetGraphic(20);

        return Convert.ToBase64String(qrCodeBytes);
    }
}