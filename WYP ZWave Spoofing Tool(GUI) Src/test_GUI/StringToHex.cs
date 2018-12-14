using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYP_ZWave_Spoofing_Tool
{
    class StringToHex
    {
        public static void String_to_Hex(char[] test, byte[] target, int len)
        {
            char ch;
            byte[] us_file_text = Enumerable.Repeat<byte>(0, 100).ToArray<byte>();

            for (int i = 0; i < len; i++)
            {
                ch = test[i];
                if (ch >= '0' && ch <= '9') us_file_text[i] = (byte)(ch - '0');
                else if (ch >= 'a' && ch <= 'f') us_file_text[i] = (byte)(ch - 'a' + 0xa);
                else if (ch >= 'A' && ch <= 'F') us_file_text[i] = (byte)(ch - 'A' + 0xa);
            }

            if ((len % 2) == 1)
            {
                for (int i = 0; i < len - 1; i += 2)
                {
                    target[i / 2] = (byte)((us_file_text[i] << 4) ^ (us_file_text[i + 1]));
                }
                target[len / 2] = (byte)(0x0F & us_file_text[len - 1]);
            }
            else
            {
                for (int i = 0; i < len; i += 2)
                {
                    target[i / 2] = (byte)((us_file_text[i] << 4) ^ us_file_text[i + 1]);
                }
            }

            //return target;
        }


        public static void String_to_Hex_For_Dos(char[] test, byte[] target, int len)
        {
            char ch;
            byte[] us_file_text = Enumerable.Repeat<byte>(0, 2000).ToArray<byte>();

            for (int i = 0; i < len; i++)
            {
                ch = test[i];
                if (ch >= '0' && ch <= '9') us_file_text[i] = (byte)(ch - '0');
                else if (ch >= 'a' && ch <= 'f') us_file_text[i] = (byte)(ch - 'a' + 0xa);
                else if (ch >= 'A' && ch <= 'F') us_file_text[i] = (byte)(ch - 'A' + 0xa);
            }

            if ((len % 2) == 1)
            {
                for (int i = 0; i < len - 1; i += 2)
                {
                    target[i / 2] = (byte)((us_file_text[i] << 4) ^ (us_file_text[i + 1]));
                }
                target[len / 2] = (byte)(0x0F & us_file_text[len - 1]);
            }
            else
            {
                for (int i = 0; i < len; i += 2)
                {
                    target[i / 2] = (byte)((us_file_text[i] << 4) ^ us_file_text[i + 1]);
                }
            }

            //return target;
        }


    }
}
