using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace BusinessManagement.Models
{
    public class Common
    {
        private static readonly string CharSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_=+";

        #region  PasswordToVarbinary
        public static string PasswordToVarbinary(string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password); // Choose appropriate encoding
            string varbinary = BitConverter.ToString(passwordBytes).Replace("-", "");
            return varbinary;
        }

        #endregion  PasswordToVarbinary

        #region  VarbinaryToPassword
        public static string VarbinaryToPassword(string varbinary)
        {
            try
            {
                byte[] passwordBytes = new byte[varbinary.Length / 2];
                for (int i = 0; i < varbinary.Length; i += 2)
                {
                    passwordBytes[i / 2] = Convert.ToByte(varbinary.Substring(i, 2), 16);
                }
                string password = Encoding.UTF8.GetString(passwordBytes); // Choose appropriate encoding
                return password;
            }
            catch (FormatException)
            {
                // Handle invalid varbinary input here
                return string.Empty;
            }
        }

        #endregion  VarbinaryToPassword


        #region GenerateRandomPassword
        public static string GenerateRandomPassword()
        {
            StringBuilder password = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                int charIndex = random.Next(CharSet.Length);
                password.Append(CharSet[charIndex]);
            }

            return password.ToString();
        }
        #endregion GenerateRandomPassword


    }
}
