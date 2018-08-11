using System;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;


namespace MOT.Publisher
{
    /// <summary>
    /// The command line security
    /// </summary>
    static class CommandLineSecurity
    {
        /// <summary>
        /// The salt
        /// </summary>
        static byte[] s_salt = Encoding.Unicode.GetBytes("E5C89470D35F62D82A659732C7DEC8A0419E1EF26B1B255858D01ABBB87C9AEB");

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="input">The decrypted string</param>
        /// <returns>The encrypted string</returns>
        public static string EncryptString(SecureString input)
        {
            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(ToInsecureString(input)), s_salt, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Decrypts a string
        /// </summary>
        /// <param name="input">The encrypted string</param>
        /// <returns>The decrypted string</returns>
        public static SecureString DecryptString(string input)
        {
            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(input), s_salt, DataProtectionScope.CurrentUser);
                return ToSecureString(Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        /// <summary>
        /// Creates a secure string
        /// </summary>
        /// <param name="input">A insecure string</param>
        /// <returns>The secure string</returns>
        public static SecureString ToSecureString(string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        /// <summary>
        /// Creates a insecure string
        /// </summary>
        /// <param name="input">A secure string</param>
        /// <returns>The insecure string</returns>
        public static string ToInsecureString(SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }
    }
}
