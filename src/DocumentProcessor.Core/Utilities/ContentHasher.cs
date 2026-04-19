using System.Security.Cryptography;
using System.Text;

namespace DocumentProcessor.Core.Utilities;

public static class ContentHasher
{
    public static string ComputeSha256Hex(string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }
}
