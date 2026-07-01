
namespace _00_ZCBB.Utils
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Engines;
    using Org.BouncyCastle.Crypto.Modes;
    using Org.BouncyCastle.Crypto.Paddings;
    using Org.BouncyCastle.Crypto.Parameters;

    public static class AESUtil
    {
        public const string DEFAULT_ENCODING = "UTF-8";
        public const string KEY_ALGORITHM = "AES";
        public const string SECURE_ALGORITHM = "SHA1PRNG"; // not directly used in C#
        public const string SECRYPT_ALGORITHM = "AES/ECB/PKCS7Padding";

        public static string Encrypt(string plainData, string randomKey)
        {
            var key = GenerateKey(randomKey);

            using Aes aesAlg = Aes.Create();
            aesAlg.Mode = CipherMode.ECB;
            aesAlg.Padding = PaddingMode.PKCS7;
            aesAlg.Key = key;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor();
            byte[] inputBytes = Encoding.UTF8.GetBytes(plainData);
            byte[] encrypted = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string aesBase64, string randomKey)
        {
            byte[] encrypted = Convert.FromBase64String(aesBase64);
            byte[] key = GenerateKey(randomKey);

            using Aes aesAlg = Aes.Create();
            aesAlg.Mode = CipherMode.ECB;
            aesAlg.Padding = PaddingMode.PKCS7;
            aesAlg.Key = key;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor();
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public static string EncryptWithBC(string? data, string key)
        {
            //byte[] keyBytes = new byte[32];
            //Encoding.UTF8.GetBytes(key).CopyTo(keyBytes, 0);

            //byte[] inputBytes = Encoding.UTF8.GetBytes(data);
            //BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()), new Pkcs7Padding());
            //cipher.Init(true, new KeyParameter(keyBytes));

            //byte[] outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
            //int len = cipher.ProcessBytes(inputBytes, 0, inputBytes.Length, outputBytes, 0);
            //cipher.DoFinal(outputBytes, len);

            //return Convert.ToBase64String(outputBytes);

            byte[] keyBytes = GenerateKey(key); // ✅ Consistent 32 bytes

            byte[] inputBytes = Encoding.UTF8.GetBytes(data);
            BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()), new Pkcs7Padding());
            cipher.Init(true, new KeyParameter(keyBytes));

            byte[] outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
            int len = cipher.ProcessBytes(inputBytes, 0, inputBytes.Length, outputBytes, 0);
            cipher.DoFinal(outputBytes, len);

            return Convert.ToBase64String(outputBytes);
        }

        public static string DecryptWithBC(string? data, string key)
        {
            //byte[] keyBytes = new byte[32];
            //Encoding.UTF8.GetBytes(key).CopyTo(keyBytes, 0);

            //byte[] inputBytes = Convert.FromBase64String(data);
            //BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()), new Pkcs7Padding());
            //cipher.Init(false, new KeyParameter(keyBytes));

            //byte[] outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
            //int len = cipher.ProcessBytes(inputBytes, 0, inputBytes.Length, outputBytes, 0);
            //int finalLen = cipher.DoFinal(outputBytes, len);

            //byte[] finalBytes = new byte[len + finalLen];
            //Array.Copy(outputBytes, 0, finalBytes, 0, finalBytes.Length);
            //return Encoding.UTF8.GetString(finalBytes);
            byte[] keyBytes = GenerateKey(key); // ✅ Consistent 32 bytes

            byte[] inputBytes = Convert.FromBase64String(data);
            BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new AesEngine()), new Pkcs7Padding());
            cipher.Init(false, new KeyParameter(keyBytes));

            byte[] outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
            int len = cipher.ProcessBytes(inputBytes, 0, inputBytes.Length, outputBytes, 0);
            int finalLen = cipher.DoFinal(outputBytes, len);

            byte[] finalBytes = new byte[len + finalLen];
            Array.Copy(outputBytes, 0, finalBytes, 0, finalBytes.Length);
            return Encoding.UTF8.GetString(finalBytes);
        }

        public static string GetRandomKey()
        {
            byte[] bytes = new byte[8];
            RandomNumberGenerator.Fill(bytes);
            return BitConverter.ToInt64(bytes, 0).ToString();
        }

        private static byte[] GenerateKey(string randomKey)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(randomKey));
            byte[] key = new byte[16];
            Array.Copy(hash, key, 16); // still AES-128
            return key;
        }
    }

}