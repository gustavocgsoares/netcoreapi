using System.Net;
using Template.CrossCutting.Cryptography;

namespace Template.CrossCutting.ExtensionMethods
{
    public static class EncryptionExtensions
    {
        #region Enums
        public enum EncryptionType
        {
            TripleDes = 0
        }
        #endregion

        #region Public methods
        public static string Encrypt(this object value, string cryptoKey, EncryptionType type = EncryptionType.TripleDes, bool utf8 = false)
        {
            if (value.IsNull())
            {
                return null;
            }

            string result = null;

            switch (type)
            {
                case EncryptionType.TripleDes:
                    result = TripleDesEncryption.Encrypt(cryptoKey, value.ToString(), utf8);
                    break;
            }

            result = WebUtility.UrlEncode(result);

            return result;
        }

        public static T Decrypt<T>(this string value, string cryptoKey, bool skipFault = false, EncryptionType type = EncryptionType.TripleDes, bool utf8 = false)
        {
            string result = null;

            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return default(T);
                }

                var valueDecoded = WebUtility.UrlDecode(value.Replace("+", "-"));

                if (!valueDecoded.Equals(value))
                {
                    value = valueDecoded;
                }

                switch (type)
                {
                    case EncryptionType.TripleDes:
                        result = TripleDesEncryption.Decrypt(cryptoKey, value, utf8);
                        break;
                }
            }
            catch
            {
                if (skipFault.Not())
                {
                    throw;
                }
            }

            return result.HasValue() ? result.To<T>() : default(T);
        }
        #endregion
    }
}
