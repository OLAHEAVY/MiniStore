using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MiniStore.Core.Security
{
    public class CryptoUtility
    {
        protected RijndaelManaged myRijndael;
        protected static readonly CryptoUtility _instance = new CryptoUtility();

        public static CryptoUtility Instance
        {

            get { return _instance; }

        }

        public CryptoUtility()
        {

        }


        public static string EncryptMessage(string Plain_Text, string encryptionKey, string initialisationVector)
        {
            string APIKey = encryptionKey;
            string IVKey = initialisationVector;

            byte[] Key = (new UTF8Encoding()).GetBytes(APIKey);
            byte[] IV = (new UTF8Encoding()).GetBytes(IVKey);

            RijndaelManaged Crypto = null;
            MemoryStream MemStream = null;
            ICryptoTransform Encryptor = null;
            CryptoStream Crypto_Stream = null;
            UTF8Encoding Byte_Transform = new UTF8Encoding();

            byte[] PlainBytes = Byte_Transform.GetBytes(Plain_Text);

            try
            {
                Crypto = new RijndaelManaged();
                Crypto.Key = Key;
                Crypto.IV = IV;
                MemStream = new MemoryStream();
                Encryptor = Crypto.CreateEncryptor(Crypto.Key, Crypto.IV);
                Crypto_Stream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write);


                Crypto_Stream.Write(PlainBytes, 0, PlainBytes.Length);
            }
            finally
            {
                if (Crypto != null)
                    Crypto.Clear();

                if (Crypto_Stream != null)
                    Crypto_Stream.Close();
            }
            string body = BitConverter.ToString(MemStream.ToArray()).Replace("-", string.Empty);

            return body;
        }

        public static string DecryptMessage(string txtToDecrypt, string encryptionKey, string initialisationVector)
        {
            string APIKey = encryptionKey;
            string IVKey = initialisationVector;

            byte[] Key = (new System.Text.UTF8Encoding()).GetBytes(APIKey);
            byte[] IV = (new System.Text.UTF8Encoding()).GetBytes(IVKey);

            RijndaelManaged Crypto = null;
            MemoryStream MemStream = null;
            ICryptoTransform Decryptor = null;

            CryptoStream Crypto_Stream = null;

            UTF8Encoding Byte_Transform = new UTF8Encoding();


            byte[] PlainBytes = Byte_Transform.GetBytes(txtToDecrypt);
            string plainText = string.Empty;
            StreamReader srDecrypt = null;
            try

            {
                Crypto = new RijndaelManaged();
                Crypto.Key = Key;
                Crypto.IV = IV;
                MemStream = new MemoryStream();

                Decryptor = Crypto.CreateDecryptor(Crypto.Key, Crypto.IV);

                Crypto_Stream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read);

                Crypto_Stream.Read(PlainBytes, 0, PlainBytes.Length);


                srDecrypt = new StreamReader(Crypto_Stream);

                plainText = srDecrypt.ReadToEnd();


            }
            finally
            {
                if (Crypto != null)
                    Crypto.Clear();
                Crypto_Stream.Close();
                srDecrypt.Dispose();
            }
            return plainText;
        }


        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string DecryptStringFromBytes(string cipher, string encryptionKey, string initialisationVector)
        {

            string APIKey = encryptionKey;
            string IVKey = initialisationVector;

            byte[] Key = (new System.Text.UTF8Encoding()).GetBytes(APIKey);
            byte[] IV = (new System.Text.UTF8Encoding()).GetBytes(IVKey);

            byte[] cipherText = StringToByteArray(cipher);

            if (cipherText == null || cipherText.Length <= 0)

                throw new ArgumentNullException("not a cipherText");

            if (Key == null || Key.Length <= 0)

                throw new ArgumentNullException("Key required ");

            if (IV == null || IV.Length <= 0)

                throw new ArgumentNullException("Init Vector required");
            string plaintext = null;
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();

                        }

                    }

                }
            }
            return plaintext;
        }


        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }




        protected static byte[] HexStringToByte(string hexString)
        {
            try
            {
                int bytesCount = hexString.Length;

                byte[] bytes = new byte[bytesCount];
                bytes = Encoding.UTF8.GetBytes(hexString);
                return bytes;
            }
            catch
            {
                throw;
            }
        }


        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)

                hex.AppendFormat("{0:x2}", b);

            return hex.ToString();
        }


        public string Hash256(string s)
        {
            HashAlgorithm Hasher = SHA256.Create();
            byte[] strBytes = Encoding.UTF8.GetBytes(s);
            byte[] strHash = Hasher.ComputeHash(strBytes);
            string hex = BitConverter.ToString(strHash).Replace("-", "").ToLower();
            return hex;
        }

    }
}
