using System.Text.RegularExpressions;

namespace Helpers.Validators
{
    public static class StringValidator
    {
        public static bool isValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            if (Regex.IsMatch(email, pattern))
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public static bool IsValidFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            char[] invalidPathChars = Path.GetInvalidPathChars();
            if (filePath.IndexOfAny(invalidPathChars) >= 0)
            {
                return false;
            }

            try
            {
                Path.GetFullPath(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

