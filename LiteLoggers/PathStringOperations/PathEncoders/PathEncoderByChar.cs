using System.IO;

namespace AG.PathStringOperations.PathEncoders
{
    class PathEncoderByChar : IPathEncoder
    {
        private readonly char _charForReplacingInvalidChars;
        public static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();

        public PathEncoderByChar(char charForReplacingInvalidChars)
        {
            _charForReplacingInvalidChars = charForReplacingInvalidChars;
        }

        public string EncodeFileName(string fileNameToEncode)
        {
            char[] encodedFileNameChars = new char[fileNameToEncode.Length];

            for (int i = 0; i < fileNameToEncode.Length; i++)
            {
                char curChar = fileNameToEncode[i];
                if (IsCharInvalid(curChar))
                {
                    encodedFileNameChars[i] = _charForReplacingInvalidChars;
                }
                else
                {
                    encodedFileNameChars[i] = curChar;
                }
            }

            return new string(encodedFileNameChars);
        }

        public static bool IsCharInvalid(char c)
        {
            for (int i = 0; i < InvalidFileNameChars.Length; i++)
            {
                if (c == InvalidFileNameChars[i])
                    return true;
            }
            return false;
        }
    }
}
