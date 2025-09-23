using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace SharedKernel.Application.Extensions;

public static class MediaExtension
{
    public static string ToHashedFileName(this string originalFileName)
    {
        var fileExt = Path.GetExtension(originalFileName);
        using var sha = SHA256.Create();
        var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(originalFileName + Guid.NewGuid()));
        var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        return $"{hash}{fileExt}";
    }

    public static string ToEncryptedFileName(this IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        var encryptedName = Guid.NewGuid().ToString("N");
        return $"{encryptedName}{extension}";
    }

    public static string GetSavePath(this string fileName, string folder)
    {
        return Path.Combine(folder, fileName);
    }

    public static string ToRelativePath(this string fileName, string folder)
    {
        return Path.Combine("/", folder.Replace("wwwroot", "").Replace("\\", "/").Trim('/'), fileName);
    }
}