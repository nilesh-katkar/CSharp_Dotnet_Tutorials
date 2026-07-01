using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AesEncryptApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //string timestamp = AESEncrypt(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), "GdH2DtUXx3OinsqQ", "18dnBvZxdlOlpSj2");

            //string encodedTimestamp = Base64Encode(timestamp);

            //string key = "GdH2DtUXx3OinsqQ";
            //byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            //Console.WriteLine("Key as byte array:");
            //foreach (byte b in keyBytes)
            //{
            //    Console.Write(b + " ");
            //}

            //Console.WriteLine("\n\nKey as hexadecimal string:");
            //Console.WriteLine(BitConverter.ToString(keyBytes).Replace("-", " "));

            string keyString = "18dnBvZxdlOlpSj2"; // Must be 16 bytes for AES-128
            string ivString = "GdH2DtUXx3OinsqQ"; // Must be 16 bytes

            byte[] keyBytes = Encoding.UTF8.GetBytes(keyString); // AES Key
            byte[] ivBytes = Encoding.UTF8.GetBytes(ivString);   // IV

            string encryptedHex = AESEncryptByteKEYIV(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"), keyBytes, ivBytes);
            Console.WriteLine("Encrypted (Hex): " + encryptedHex);

            string encodedTimestamp = Base64Encode(encryptedHex);
            
            // Replace this with your Base64-encoded key (must decode to 16/24/32 bytes)
            //string base64Key = "uWv3a8nD96a+6AKl1TjG1g=="; // Example: AES-128 key
            ////string json = GetJsonPayload();
            //string json = "2025-06-17T09:47:48.312Z";

            //try
            //{
            //    string encrypted = Encrypt(json, base64Key);
            //    Console.WriteLine("Encrypted (Base64):");
            //    Console.WriteLine(encrypted);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Error: " + ex.Message);
            //}

            //Console.WriteLine("\nPress any key to exit...");
            //Console.ReadKey();
        }

        static string AESEncryptByteKEYIV(string plainText, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                    // Return as lowercase hex string
                    return BitConverter.ToString(encryptedBytes).Replace("-", "").ToLower();
                }
            }
        }

        public static string AESEncrypt(string? plainText, string? key, string? iv)
        {
            //this.logger.LogInformation("AESEncrypt() called. TransactionBaseId : {TransactionBaseId}", transactionBaseId);

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    return BitConverter.ToString(encryptedBytes).Replace("-", string.Empty).ToLower();
                }
            }
        }

        static string Encrypt(string plainText, string base64Key)
        {
            byte[] keyBytes = Convert.FromBase64String(base64Key);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.GenerateIV();
                byte[] iv = aes.IV;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                using (MemoryStream ms = new MemoryStream())
                {
                    // Prepend IV to the output
                    ms.Write(iv, 0, iv.Length);

                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
                        cs.Write(inputBytes, 0, inputBytes.Length);
                        cs.FlushFinalBlock();
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Base64Encode(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("Input cannot be null or empty", nameof(plainText));

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
                throw new ArgumentException("Input cannot be null or empty", nameof(base64EncodedData));

            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        static string GetJsonPayload()
        {
            // You can load this from a file or another source if needed
            return @"{
  ""partnerCode"": ""XMUAE"",
  ""transactionDate"": ""06/16/2025"",
  ""applicationNumber"": ""XMUAE_0529AA1"",
  ""remitterId"": ""1234567890"",
  ""remitterFirstName"": ""JUAN"",
  ""remitterMiddleName"": ""G"",
  ""remitterLastName"": ""DELA CRUZ"",
  ""remitterCustomerType"": ""I"",
  ""remitterNationality"": ""FILIPINO"",
  ""remitterAddress1"": ""PH"",
  ""remitterMobileNumber"": ""09076492310"",
  ""remitterBirthDate"": ""01/01/1987"",
  ""remitterIdType1"": ""PASSPORT"",
  ""remitterIdNumber1"": ""1234567890"",
  ""remitterIdIssuedAt1"": ""MANILA"",
  ""remitterIdExpiry1"": ""01/01/2027"",
  ""beneficiaryFirstName"": ""DEAN"",
  ""beneficiaryMiddleName"": ""G"",
  ""beneficiaryLastName"": ""DELA CRUZ"",
  ""beneficiaryCustomerType"": ""I"",
  ""beneficiaryAddress1"": ""MAKATI"",
  ""beneficiaryCountry"": ""PH"",
  ""beneficiaryBirthDate"": ""02/02/2001"",
  ""beneficiaryNationality"": ""FILIPINO"",
  ""beneficiaryRelationToRemitter"": ""BROTHER"",
  ""beneficiaryMobileNumber"": ""09957488986"",
  ""fundingCurrency"": ""PHP"",
  ""fundingAmount"": ""47000"",
  ""settlementCurrency"": ""PHP"",
  ""settlementMode"": ""09""
}";
        }
    }
}
