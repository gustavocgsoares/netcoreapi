using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Template.CrossCutting.ExtensionMethods
{
    public static class StringExtensions
    {
        #region Public methods
        public static TClass JsonToObject<TClass>(this string value)
            where TClass : class
        {
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<TClass>(value);
            }

            return null;
        }

        public static bool Contains(this string source, string input, StringComparison comparison)
        {
            return source.IndexOf(input, comparison) >= 0;
        }

        public static string EmptyIfNull(this string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value;
        }

        public static string FormatCpf(this string value)
        {
            if (!value.IsNullOrEmpty())
            {
                return string.Empty;
            }

            value = value.Trim();

            if (value.Length != 11)
            {
                return string.Empty;
            }

            return string.Format("{0}.{1}.{2}-{3}", value.Substring(0, 3), value.Substring(3, 3), value.Substring(6, 3), value.Substring(9));
        }

        public static byte[] GetBytes(this string value)
        {
            var bytes = new byte[value.Length * sizeof(char)];
            Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static bool HasNoValue(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static string UrlEncode(this string value)
        {
            return WebUtility.UrlEncode(value);
        }

        public static string UrlDecode(this string value)
        {
            return WebUtility.UrlDecode(value);
        }

        public static bool IsCnpj(this string value)
        {
            string cnpj = value.Replace(".", string.Empty);
            cnpj = cnpj.Replace("/", string.Empty);
            cnpj = cnpj.Replace("-", string.Empty);

            int[] digitos, soma, resultado;
            int nrDig;
            string ftmt;
            bool[] rightCnpj;

            ftmt = "6543298765432";
            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;
            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;
            rightCnpj = new bool[2];
            rightCnpj[0] = false;
            rightCnpj[1] = false;

            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(cnpj.Substring(nrDig, 1));

                    if (nrDig <= 11)
                    {
                        soma[0] += digitos[nrDig] * int.Parse(ftmt.Substring(nrDig + 1, 1));
                    }

                    if (nrDig <= 12)
                    {
                        soma[1] += digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1));
                    }
                }

                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = soma[nrDig] % 11;

                    if (resultado[nrDig] == 0 || resultado[nrDig] == 1)
                    {
                        rightCnpj[nrDig] = digitos[12 + nrDig] == 0;
                    }
                    else
                    {
                        rightCnpj[nrDig] = digitos[12 + nrDig] == 11 - resultado[nrDig];
                    }
                }

                return rightCnpj[0] && rightCnpj[1];
            }
            catch
            {
                return false;
            }
        }

        public static bool IsCpf(this string value)
        {
            if (value.HasNoValue())
            {
                return false;
            }

            value = value.Replace(".", string.Empty).Replace("-", string.Empty).Trim();

            if (value.Length != 11)
            {
                return false;
            }

            switch (value)
            {
                case "00000000000":
                case "11111111111":
                case "22222222222":
                case "33333333333":
                case "44444444444":
                case "55555555555":
                case "66666666666":
                case "77777777777":
                case "88888888888":
                case "99999999999":
                    return false;
            }

            var soma = 0;
            for (int i = 0, j = 10; i < 9; i++, j--)
            {
                int d;

                if (!int.TryParse(value[i].ToString(), out d))
                {
                    return false;
                }

                soma += d * j;
            }

            var resto = soma % 11;

            var digito = (resto < 2 ? 0 : 11 - resto).ToString(CultureInfo.InvariantCulture);
            var prefixo = value.Substring(0, 9) + digito;

            soma = 0;
            for (int i = 0, j = 11; i < 10; i++, j--)
            {
                soma += int.Parse(prefixo[i].ToString()) * j;
            }

            resto = soma % 11;
            digito += (resto < 2 ? 0 : 11 - resto).ToString(CultureInfo.InvariantCulture);

            return value.EndsWith(digito);
        }

        public static bool IsEmail(this string email)
        {
            return email.HasValue()
                && Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static string Limit(this string source, int maxLength, string suffix = null)
        {
            if (suffix.HasValue())
            {
                maxLength = maxLength - suffix.Length;
            }

            if (source.Length <= maxLength)
            {
                return source;
            }

            return string.Concat(source.Substring(0, maxLength).Trim(), suffix ?? string.Empty);
        }

        public static string NullIfEmpty(this string value)
        {
            if (value == string.Empty)
            {
                return null;
            }

            return value;
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            var sb = new StringBuilder();

            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public static string RemoveAccent(this string value)
        {
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            return Encoding.ASCII.GetString(bytes);
        }

        public static IEnumerable<string> SplitAndTrim(this string value, params char[] separators)
        {
            return value
                .Trim()
                .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());
        }

        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value) || !char.IsUpper(value[0]))
            {
                return value;
            }

            string lowerCasedFirstChar = char.ToLower(value[0]).ToString();

            if (value.Length > 1)
            {
                lowerCasedFirstChar = lowerCasedFirstChar + value.Substring(1);
            }

            return lowerCasedFirstChar;
        }

        public static string ToDelimited(this string value, char delimiter)
        {
            var camelCaseString = value.ToCamelCase();
            return new string(InsertDelimiterBeforeCaps(camelCaseString, delimiter));
        }

        public static string ToPascalCase(this string value)
        {
            var tokens = value.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                tokens[i] = token.Substring(0, 1).ToUpper() + token.Substring(1);
            }

            return string.Join(" ", tokens);
        }
        #endregion

        #region Private Methods
        private static char[] InsertDelimiterBeforeCaps(string input, char delimiter)
        {
            var result = new List<char>();
            var lastCharWasUpper = false;

            foreach (var c in input)
            {
                if (char.IsUpper(c))
                {
                    if (!lastCharWasUpper)
                    {
                        result.Add(delimiter);
                        lastCharWasUpper = true;
                    }

                    result.Add(char.ToLower(c));
                    continue;
                }

                result.Add(c);
                lastCharWasUpper = false;
            }

            return result.ToArray();
        }
        #endregion
    }
}
