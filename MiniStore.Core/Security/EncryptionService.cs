using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace MiniStore.Core.Security
{
    public class EncryptionService
    {
        private readonly string _encrytionKey = "shst789hsgsfb67k";

        private byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    var encrypt = Encoding.Unicode.GetBytes(data);
                    cs.Write(encrypt, 0, encrypt.Length);
                    cs.FlushFinalBlock();
                }

                return ms.ToArray();
            }
        }

        private string DecryptTextToMemory(byte[] data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv), CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs, Encoding.Unicode))
                    {
                        return sr.ReadToEnd();
                    }
                }

            }
        }

        public virtual string EncryptText(string plainText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return plainText;
            }

            if (string.IsNullOrEmpty(encryptionPrivateKey))
            {
                encryptionPrivateKey = _encrytionKey;
            }

            using (var provider = new TripleDESCryptoServiceProvider())
            {
                provider.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16));
                provider.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8));

                var binary = EncryptTextToMemory(plainText, provider.Key, provider.IV);

                return Convert.ToBase64String(binary);
            }

        }

        public virtual string DecryptText(string cipherText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                return cipherText;
            }

            if (string.IsNullOrEmpty(encryptionPrivateKey))
            {
                encryptionPrivateKey = _encrytionKey;
            }

            using (var provider = new TripleDESCryptoServiceProvider())
            {
                provider.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16));
                provider.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8));



                var buffer = Convert.FromBase64String(cipherText);
                return DecryptTextToMemory(buffer, provider.Key, provider.IV);
            }

        }

        public virtual string UrlEncodedEncryptText(string plainText)
        {
            return WebUtility.UrlEncode(EncryptText(plainText));
        }

        public virtual string UrlDecodeEncryptText(string cipherText)
        {
            var decodedString = WebUtility.UrlDecode(cipherText);
            return DecryptText(decodedString);
        }

    }
}
