using System.Net.Mail;

namespace APISegura.Common.Validators
{
    public static class EmailValidator
    {
        public static bool IsValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static string Normalize(string email)
        {
            return email?.Trim().ToLower();
        }
    }
}
