using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace DesktopCodOperacional.Services.Security
{
    public class SecureTokenStorageService
    {
        private const string FilePath = "secure_tokens.dat";

        public void Save(string accessToken, string refreshToken)
        {
            var data = $"{accessToken}|{refreshToken}";
            var bytes = Encoding.UTF8.GetBytes(data);

            var encrypted = ProtectedData.Protect(
                bytes,
                null,
                DataProtectionScope.CurrentUser);

            File.WriteAllBytes(FilePath, encrypted);
        }

        public (string accessToken, string refreshToken)? Load()
        {
            if (!File.Exists(FilePath))
                return null;

            var encrypted = File.ReadAllBytes(FilePath);

            var decrypted = ProtectedData.Unprotect(
                encrypted,
                null,
                DataProtectionScope.CurrentUser);

            var data = Encoding.UTF8.GetString(decrypted);
            var parts = data.Split('|');

            if (parts.Length != 2)
                return null;

            return (parts[0], parts[1]);
        }

        public void Clear()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }
}
