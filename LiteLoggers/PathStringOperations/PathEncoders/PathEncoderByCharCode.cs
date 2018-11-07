using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AG.PathStringOperations.PathEncoders
{
    public class PathEncoderByCharCode : ISymetrictPathEncoder
    {
        private const char PrefixChar = '%';
        private readonly int MaxLength;
        private readonly Dictionary<char, char[]> Illegals;

        static PathEncoderByCharCode()
        {
            Instance = new PathEncoderByCharCode();
        }

        public static PathEncoderByCharCode Instance;

        public PathEncoderByCharCode()
        {
            List<char> illegalChars = new List<char> { PrefixChar };
            illegalChars.AddRange(Path.GetInvalidFileNameChars());

            Illegals = new Dictionary<char, char[]>();
            foreach (char c in illegalChars)
            {
                string tmpString = ((int)c).ToString();
                int length = tmpString.Length;
                if (length > MaxLength)
                    MaxLength = length;
            }
            foreach (char c in illegalChars)
            {
                char[] encodedChars = ((int)c).ToString("D" + MaxLength).ToCharArray();
                Illegals.Add(c, encodedChars);
            }
        }

        public string EncodeFileName(string s)
        {
            var builder = new StringBuilder();
            char[] replacement;
            using (var reader = new StringReader(s))
            {
                while (true)
                {
                    int read = reader.Read();
                    if (read == -1)
                        break;
                    char c = (char)read;
                    if (Illegals.TryGetValue(c, out replacement))
                    {
                        builder.Append(PrefixChar);
                        builder.Append(replacement);
                    }
                    else
                    {
                        builder.Append(c);
                    }
                }
            }
            return builder.ToString();
        }

        public string DecodeFileName(string s)
        {
            var builder = new StringBuilder();
            char[] buffer = new char[MaxLength];
            using (var reader = new StringReader(s))
            {
                while (true)
                {
                    int read = reader.Read();
                    if (read == -1)
                        break;
                    char c = (char)read;
                    if (c == PrefixChar)
                    {
                        reader.Read(buffer, 0, MaxLength);
                        var encoded = (char)ParseCharArray(buffer);
                        builder.Append(encoded);
                    }
                    else
                    {
                        builder.Append(c);
                    }
                }
            }
            return builder.ToString();
        }

        public int ParseCharArray(char[] buffer)
        {
            int result = 0;
            foreach (char t in buffer)
            {
                int digit = t - '0';
                if ((digit < 0) || (digit > 9))
                {
                    throw new ArgumentException("Input string was not in the correct format");
                }
                result *= 10;
                result += digit;
            }
            return result;
        }
    }
}
