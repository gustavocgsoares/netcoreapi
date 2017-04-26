using System;
using System.Security.Cryptography;
using System.Text;

namespace Template.CrossCutting.Cryptography
{
    public static class TripleDesEncryption
    {
        #region Fields | Members
        private static readonly byte[] Iv = { 44, 66, 77, 3, 184, 7, 109, 13 };
        #endregion

        #region Public methods
        public static string Encrypt(string cryptoKey, string value, bool utf8 = false)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            byte[] buffer;

            var des = TripleDES.Create();

            if (utf8)
            {
                buffer = Encoding.UTF8.GetBytes(value);
                des.Key = Encoding.UTF8.GetBytes(cryptoKey);
            }
            else
            {
                buffer = Encoding.ASCII.GetBytes(value);
                des.Key = Encoding.ASCII.GetBytes(cryptoKey);
            }

            des.IV = Iv;

            var ecryptor = des.CreateEncryptor();
            var block = ecryptor.TransformFinalBlock(buffer, 0, buffer.Length);

            return Convert.ToBase64String(block).Replace('+', '-').Replace('/', '_').Replace("=", string.Empty);
        }

        public static string Decrypt(string cryptoKey, string value, bool utf8 = false)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            while (value.Length % 4 != 0)
            {
                value += "=";
            }

            value = value.Replace('-', '+').Replace('_', '/');

            byte[] buffer = Convert.FromBase64String(value);

            var des = TripleDES.Create();

            des.IV = Iv;

            if (utf8)
            {
                des.Key = Encoding.UTF8.GetBytes(cryptoKey);

                return Encoding.UTF8.GetString(
                    des.CreateDecryptor().TransformFinalBlock(
                        buffer, 0, buffer.Length));
            }

            des.Key = Encoding.ASCII.GetBytes(cryptoKey);

            return Encoding.ASCII.GetString(
                des.CreateDecryptor().TransformFinalBlock(
                    buffer, 0, buffer.Length));
        }
        #endregion
    }
}
