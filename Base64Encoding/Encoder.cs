using System;
using System.Collections.Generic;
using System.Linq;

namespace Base64Encoding
{
    public class Encoder
    {
        internal string Encode(string text)
        {
            var asBinary = string.Join("", text.Select(c => Convert.ToString(c, 2).PadLeft(8, '0')));

            var result = "";
            foreach (var block in ThreeOctets(asBinary))
            {
                switch (block.Length)
                {
                    case 8:
                        result += ConvertSextet(0, block) +
                                  ConvertSextet(1, block) +
                                  "==";
                        break;
                    case 16:
                        result += ConvertSextet(0, block) +
                                  ConvertSextet(1, block) +
                                  ConvertSextet(2, block) +
                                  "=";
                        break;

                    default:
                        result += ConvertSextet(0, block) +
                                  ConvertSextet(1, block) +
                                  ConvertSextet(2, block) +
                                  ConvertSextet(3, block);
                        break;
                }
            }

            return result;
        }

        private string ConvertSextet(int sextetNumber, string block)
        {
            var blockStart = sextetNumber * 6;
            if (blockStart + 6 >= block.Length)
                return Lookup(Convert.ToByte(block.Substring(blockStart).PadRight(6, '0'), 2)).ToString();
            else
                return Lookup(Convert.ToByte(block.Substring(blockStart, 6), 2)).ToString();
        }

        private IEnumerable<string> ThreeOctets(string binaryText)
        {
            var offset = 0;
            while (offset < binaryText.Length)
            {
                if (offset + 24 >= binaryText.Length)
                    yield return binaryText.Substring(offset);
                else
                    yield return binaryText.Substring(offset, 24);
                offset += 24;
            }
        }

        private char Lookup(byte index)
        {
            if (index <= 25)
                return (char)(index + 'A');
            else if (index <= 51)
                return (char)(index - 26 + 'a');
            else if (index <= 61)
                return (char)(index - 52 + '0');
            else if (index <= 62)
                return '+';
            else if (index <= 63)
                return '/';
            else
                return ' ';
        }
    }
}
