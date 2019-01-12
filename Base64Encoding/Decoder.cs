using System;
using System.Collections.Generic;

namespace Base64Encoding
{
    internal class Decoder
    {
        internal string Decode(string encodedText)
        {
            var asBinary = "";
            foreach (var block in FourCharacters(encodedText))
            {
                if (block.EndsWith("=="))
                {
                    asBinary += LookupCharAsBinary(block[0]);
                    asBinary += LookupCharAsBinary(block[1]).Substring(0, 2);
                }
                else if (block.EndsWith("="))
                {
                    asBinary += LookupCharAsBinary(block[0]);
                    asBinary += LookupCharAsBinary(block[1]);
                    asBinary += LookupCharAsBinary(block[2]).Substring(0, 4);
                }
                else
                {
                    asBinary += LookupCharAsBinary(block[0]);
                    asBinary += LookupCharAsBinary(block[1]);
                    asBinary += LookupCharAsBinary(block[2]);
                    asBinary += LookupCharAsBinary(block[3]);
                }
            }

            var asText = "";
            foreach (var octet in Octets(asBinary))
                asText += ((char)Convert.ToByte(octet, 2)).ToString();

            return asText;
        }

        private string LookupCharAsBinary(char c)
        {
            var base10 = 0;

            if (c >= 'A' && c <= 'Z')
                base10 = c - 'A';
            else if (c >= 'a' && c <= 'z')
                base10 = c - 'a' + 26;
            else if (c >= '0' && c <= '9')
                base10 = c - '0' + 52;
            else if (c == '+')
                base10 = 62;
            else if (c == '/')
                base10 = 63;

            return Convert.ToString(base10, 2).PadLeft(6, '0');
        }

        private IEnumerable<string> Octets(string binaryText)
        {
            var offset = 0;
            while (offset < binaryText.Length)
            {
                yield return binaryText.Substring(offset, 8);
                offset += 8;
            }
        }
        private IEnumerable<string> FourCharacters(string encodedText)
        {
            var offset = 0;
            while (offset < encodedText.Length)
            {
                yield return encodedText.Substring(offset, 4);
                offset += 4;
            }
        }
    }
}