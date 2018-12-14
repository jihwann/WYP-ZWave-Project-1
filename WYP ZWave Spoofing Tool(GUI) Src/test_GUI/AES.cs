using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace WYP_ZWave_Spoofing_Tool
{
    class AES
    {
        public static byte[] AES128_CBC_ENC(byte[] textToEncrypt, byte[] key, byte[] IV)
        {
            byte[] return_value = new byte[16];
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;

            //Console.WriteLine("Key : {0:X} {1:X} {2:X} {3:X} {4:X} {5:X} {6:X} {7:X} {8:X} {9:X} {10:X} {11:X} {12:X} {13:X} {14:X} {15:X} ", key[0], key[1], key[2], key[3], key[4], key[5], key[6], key[7], key[8], key[9], key[10], key[11], key[12], key[13], key[14], key[15]);
            rijndaelCipher.Key = key;
            rijndaelCipher.IV = IV;
            byte[] encText = rijndaelCipher.CreateEncryptor().TransformFinalBlock(textToEncrypt, 0, textToEncrypt.Length);

            //return encText;
            for (int i = 0; i < 16; i++) { return_value[i] = encText[i]; }
            return return_value;
        }

        public static byte[] AES128_CBC_DEC(byte[] textToDecrypt, byte[] key, byte[] IV)
        {
            byte[] return_value = new byte[16];
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;

            //Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = key;
            rijndaelCipher.IV = IV;
            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(textToDecrypt, 0, textToDecrypt.Length); // have to revise

            //return plainText; // have to revise
            for (int i = 0; i < 16; i++) { return_value[i] = plainText[i]; }
            return return_value;
        }

        public static byte[] AES128_ECB_ENC(byte[] input, byte[] key)
        {
            byte[] return_value = new byte[16];
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.ECB;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;
            rijndaelCipher.Key = key;
            byte[] encText = rijndaelCipher.CreateEncryptor().TransformFinalBlock(input, 0, input.Length);

            for (int i = 0; i < 16; i++) { return_value[i] = encText[i]; }
            return return_value;
        }

        public static byte[] AES128_OFB_ENC(byte[] data, byte[] key, byte[] IV)
        {
            byte[] return_value = new byte[data.Length];
            byte[] tempIV;
            tempIV = IV;

            int cipherIndex;
            int blockIndex = 0;
            byte[] plaintext16ByteChunk = new byte[16];

            /*
            Console.WriteLine("Input Your Commnad ");
            string input = Console.ReadLine();
            input = Regex.Replace(input, " ", "");
            char[] buf = Nonce_Get.ToCharArray();
            int L = buf.Length;
            byte[] data = new byte[L];
            String_to_Hex(buf, data, buf.Length);
            Console.WriteLine("Your Input data : {0:X} {1:X} {2:X} {3:X} {4:X} {5:X} {6:X} {7:X}", Nonce_DoorLock_KT[0], Nonce_DoorLock_KT[1], Nonce_DoorLock_KT[2], Nonce_DoorLock_KT[3], Nonce_DoorLock_KT[4], Nonce_DoorLock_KT[5], Nonce_DoorLock_KT[6], Nonce_DoorLock_KT[7]);
            */
            
            for (cipherIndex = 0; cipherIndex < data.Length; cipherIndex++)
            {
                plaintext16ByteChunk[blockIndex] = data[cipherIndex];
                blockIndex++;
                if (blockIndex == 16)
                {
                    tempIV = AES128_ECB_ENC(tempIV, key);
                    int ivIndex = 0;
                    for (int i = (cipherIndex - 15); i <= cipherIndex; i++)
                    {
                        data[i] = (byte)(plaintext16ByteChunk[ivIndex] ^ tempIV[ivIndex]);
                        ivIndex++;
                    }
                    plaintext16ByteChunk = new byte[16];
                    blockIndex = 0;
                }
            }

            if (blockIndex != 0)
            {
                tempIV = AES128_ECB_ENC(tempIV, key);
                //AES128_ECB_ENC(tempIV, key, tempIV);
                int ivIndex = 0;
                for (int i = 0; i < blockIndex; i++)
                {
                    data[cipherIndex - blockIndex + i] = (byte)(plaintext16ByteChunk[i] ^ tempIV[i]);
                    ivIndex++;
                }
            }

            for (int i = 0; i < return_value.Length; i++) { return_value[i] = data[i]; }
            //return data;
            return return_value;
        }

        /*
        public static byte[] AES128_OFB_ENC_9(byte[] data, byte[] key, byte[] IV)
        {
            byte[] return_value = new byte[9];
            byte[] tempIV;
            tempIV = IV;

            int cipherIndex;
            int blockIndex = 0;
            byte[] plaintext16ByteChunk = new byte[16];

            for (cipherIndex = 0; cipherIndex < data.Length; cipherIndex++)
            {
                plaintext16ByteChunk[blockIndex] = data[cipherIndex];
                blockIndex++;
                if (blockIndex == 16)
                {
                    tempIV = AES128_ECB_ENC(tempIV, key);
                    int ivIndex = 0;
                    for (int i = (cipherIndex - 15); i <= cipherIndex; i++)
                    {
                        data[i] = (byte)(plaintext16ByteChunk[ivIndex] ^ tempIV[ivIndex]);
                        ivIndex++;
                    }
                    plaintext16ByteChunk = new byte[16];
                    blockIndex = 0;
                }
            }

            if (blockIndex != 0)
            {
                tempIV = AES128_ECB_ENC(tempIV, key);
                //AES128_ECB_ENC(tempIV, key, tempIV);
                int ivIndex = 0;
                for (int i = 0; i < blockIndex; i++)
                {
                    data[cipherIndex - blockIndex + i] = (byte)(plaintext16ByteChunk[i] ^ tempIV[i]);
                    ivIndex++;
                }
            }

            for (int i = 0; i < return_value.Length; i++) { return_value[i] = data[i]; }
            //return data;
            return return_value;
        }
        */
        public static byte[] AES128_CBCMAC_ENC(byte[] header, byte[] data, byte[] Km)
        {
            byte[] return_value = new byte[8];
            byte[] input16Byte = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
            byte[] MAC = Enumerable.Repeat<byte>(0, 16).ToArray<byte>();
            byte[] input = new byte[20 + data.Length];

            for (int i = 0; i < 20; i++)
            {
                input[i] = header[i];
            }
            for (int i = 0; i < data.Length; i++)
            {
                input[20 + i] = data[i];
            }
            for (int i = 0; i < 16; i++)
            {
                if (i >= input.Length)
                    input16Byte[i] = 0;
                else
                    input16Byte[i] = input[i];
            }
            MAC = AES128_ECB_ENC(input16Byte, Km);
            System.Array.Clear(input16Byte, 0, 16);

            int cipherIndex;
            int blockIndex = 0;

            for (cipherIndex = 16; cipherIndex < input.Length; cipherIndex++)
            {
                input16Byte[blockIndex] = input[cipherIndex];
                blockIndex++;

                if (blockIndex == 16)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        MAC[i] = (byte)(input16Byte[i] ^ MAC[i]);
                    }
                    System.Array.Clear(input16Byte, 0, 16);
                    blockIndex = 0;

                    MAC = AES128_ECB_ENC(MAC, Km);
                }
            }
            if (blockIndex != 0)
            {
                for (int i = 0; i < 16; i++)
                {
                    MAC[i] = (byte)(input16Byte[i] ^ MAC[i]);
                }
                MAC = AES128_ECB_ENC(MAC, Km);
            }

            //return MAC;
            for (int i = 0; i < 8; i++) { return_value[i] = MAC[i]; }
            return return_value;
        }
        
    }
}
