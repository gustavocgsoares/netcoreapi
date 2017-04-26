namespace Template.CrossCutting.ExtensionMethods
{
    public static class ByteExtensions
    {
        #region Public methods
        public static string GetString(this byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        #endregion
    }
}
