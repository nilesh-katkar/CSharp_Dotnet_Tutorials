

namespace _00_ZCBB.Utils
{
    using System.Security.Cryptography;
    using System.Text;

    public static class AlgorithmUtil
    {
        private const string DEFAULT_ENCODING = "UTF-8";

        //public static string GetAESRandomKey()
        //{
        //    long randomKey = RandomNumberGenerator.GetInt64(long.MinValue, long.MaxValue);
        //    return randomKey.ToString();
        //}

        public static string GetAESRandomKey()
        {
            byte[] keyBytes = new byte[32]; // 256-bit AES key
            RandomNumberGenerator.Fill(keyBytes);
            return Convert.ToBase64String(keyBytes);
        }

        public static string EncryptWithRSA(string data, string publicKeyPem)
        {
            return RSAUtil.EncryptByPublicKey(data, publicKeyPem);
        }

        public static string EncryptWithAES(string? data, string key)
        {
            return AESUtil.EncryptWithBC(data, key);
        }

        public static string DecryptWithRSA(string? encryptedData, string privateKeyPem)
        {
            byte[] decryptedBytes = RSAUtil.DecryptByPrivateKey(encryptedData, privateKeyPem);
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public static string DecryptWithAES(string? encryptedData, string key)
        {
            return AESUtil.DecryptWithBC(encryptedData, key);
        }

        public static string Sign(string data, string privateKeyPem)
        {
            return RSAUtil.Sign(Encoding.UTF8.GetBytes(data), privateKeyPem);
        }

        public static bool Verify(string data, string publicKeyPem, string? signatureBase64)
        {
            return RSAUtil.Verify(Encoding.UTF8.GetBytes(data), publicKeyPem, signatureBase64);
        }
    }

}
