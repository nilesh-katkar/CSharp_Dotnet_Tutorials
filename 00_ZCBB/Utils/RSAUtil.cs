
namespace _00_ZCBB.Utils
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class RSAUtil
    {
        public static byte[] DecryptBASE64(string key)
        {
            return Convert.FromBase64String(key);
        }

        public static string EncryptBASE64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static string EncryptByPublicKey(string data, string base64PublicKey)
        {
            string cleanedKey = CleanBase64Key(base64PublicKey);
            byte[] publicKeyBytes = Convert.FromBase64String(cleanedKey);
            using (var rsa = RSA.Create())
            {
                rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
                byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(data), RSAEncryptionPadding.Pkcs1);
                return Convert.ToBase64String(encryptedData);
            }
        }

        public static byte[] DecryptByPrivateKey(string base64Data, string base64PrivateKey)
        {
            byte[] encryptedData = Convert.FromBase64String(base64Data);
            byte[] privateKeyBytes = Convert.FromBase64String(base64PrivateKey);
            using (var rsa = RSA.Create())
            {
                rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
                return rsa.Decrypt(encryptedData, RSAEncryptionPadding.Pkcs1);
            }
        }

        public static string Sign(byte[] data, string base64PrivateKey)
        {
            string cleanedKey = CleanBase64Key(base64PrivateKey);

            byte[] privateKeyBytes = Convert.FromBase64String(cleanedKey);
            using (var rsa = RSA.Create())
            {
                rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
                byte[] signatureBytes = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                return Convert.ToBase64String(signatureBytes);
            }
        }

        public static bool Verify(byte[] data, string base64PublicKey, string base64Signature)
        {
            byte[] publicKeyBytes = Convert.FromBase64String(base64PublicKey);
            byte[] signatureBytes = Convert.FromBase64String(base64Signature);
            using (var rsa = RSA.Create())
            {
                rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
                return rsa.VerifyData(data, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

        public static string CleanBase64Key(string pem)
        {
            return pem
                .Replace("-----BEGIN PUBLIC KEY-----", "")
                .Replace("-----END PUBLIC KEY-----", "")
                .Replace("-----BEGIN RSA PUBLIC KEY-----", "")
                .Replace("-----END RSA PUBLIC KEY-----", "")
                .Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "")
                .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                .Replace("-----END RSA PRIVATE KEY-----", "")
                .Replace("\r", "")
                .Replace("\n", "")
                .Trim();
        }

    }

}
