



using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

class Program
{
    static void Main(string[] args)
    {
        string clientId = "2778f49ea5338e0f84535d37a78b63fc"; // Example Client ID
        string clientSecret = "ebf582f617150b3e"; // Example Client Secret

        // Updated request data structure
        var requestData = new
        {
            Request = new
            {
                Header = new
                {
                    Timestamp = "20250117120624983",
                    ChannelDetails = new
                    {
                        ChannelID = "XPRESS",
                        ChannelType = "WEB",
                        ChannelSubClass = "Retail",
                        BranchCode = "",
                        ChannelCusHdr = new
                        {
                            ChannelProtocol = "Mobile"
                        }
                    },
                    DeviceDetails = new
                    {
                        DeviceID = "Device1",
                        IMEINumber = "",
                        ClientIP = "",
                        OS = "",
                        BrowserType = "",
                        MobileNumber = "",
                        GeoLocation = new
                        {
                            Latitude = "13.072090",
                            Longitude = "80.201859"
                        }
                    }
                },
                Body = new
                {
                    SenderCD = "XPRESS_EX_WS",
                    TransactionID = "100000000000",
                    TransactionDate = "20190930",
                    Signature = "",
                    CheckSum = "60-45-BD-14-8D-99"
                }
            }
        };

        string encryptedRequest = EncryptRequest(requestData, clientId, clientSecret);
        Console.WriteLine("Encrypted Request:");
        Console.WriteLine(encryptedRequest);
    }

    static string EncryptRequest(object data, string clientId, string clientSecret)
    {
        // Extract the AES key and IV
        string key = clientId.Substring(0, 32);
        string iv = clientSecret.Substring(0, 16);

        // Convert the request data to JSON string
        string jsonData = JsonConvert.SerializeObject(data);
        byte[] dataBytes = Encoding.UTF8.GetBytes(jsonData);

        // Create AES instance
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataBytes, 0, dataBytes.Length);
                    cs.FlushFinalBlock();
                }

                // Convert encrypted data to Base64 string
                byte[] encryptedData = ms.ToArray();
                return Convert.ToBase64String(encryptedData);
            }
        }
    }
}




//using System;
//using System.Security.Cryptography;
//using System.Text;

//public class AesDecryption
//{
//    public static string DecryptHexString(string hexString, string key, string iv)
//    {
//        // Convert hex string to byte array
//        byte[] encryptedBytes = ConvertHexStringToByteArray(hexString);

//        // Convert key and IV to byte arrays
//        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
//        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);

//        using (Aes aes = Aes.Create())
//        {
//            aes.Key = keyBytes;
//            aes.IV = ivBytes;
//            aes.Mode = CipherMode.CBC;
//            aes.Padding = PaddingMode.PKCS7;

//            using (var decryptor = aes.CreateDecryptor())
//            {
//                byte[] plainBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
//                return Encoding.UTF8.GetString(plainBytes);
//            }
//        }
//    }

//    private static byte[] ConvertHexStringToByteArray(string hexString)
//    {
//        int length = hexString.Length;
//        byte[] bytes = new byte[length / 2];
//        for (int i = 0; i < length; i += 2)
//        {
//            bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
//        }
//        return bytes;
//    }
//}

//// Usage Example
//public class Program
//{
//    public static void Main()
//    {
//        string hexString = "7906f328a2d42584b2577b08b9281ed050fb40e216f5acd350364a57782e4274b370bc0ec6a964e5\r\n 989aaf0bae14b7f22b4c8ec599ec63ccd91ca8a1bb707a761301ced23d77a1d53c1ac538ea86f934ab1\r\n 5e1a77eb9a4611799a0a6ec3384deb685b679ded2d1cb853984b6c62250127fc9d7e84b4b949e389\r\n 839ea62e02461f3581e8db02e77dca6db663c21e9328d8af0fa03508fdf0a6d5025dac162bd6028eb\r\n 6a956be47d8b0062bd82c44e77e68e0bda924290917f6889ebb3155ee58ae0f714de6ff9d2eeb65b\r\n 4aa6a5441b4d6f8e58db203ab83c6dea030a30d106e9a00c869215b9bef89724fd18bf7ab1cb0414f\r\n 94f1f807afd03214add61cdf1d7cc9f07f198e4f704ef6f7d6d261ea2f7685fdb90bc5c11df4178f62ae0\r\n edfc5999a642baf30968ce8c2943af810aadce7b64f777891d3836cce428e3d87c98d10b4f13e33507\r\n 21458d403e37a3b332f35ebb33e946021cdca9cdb447b780e99067ed0d97d16760d855012ce6e31b\r\n 8fae97247ab5170c085ed6c54e3379243fffc90239d22066d3361856531e7036c077923aa9d0c4078e\r\n 18926b69580d323c6f249bdf147e42ae103ae1fcaba2afca6dc902213e5f28057683d8d4fcf4933a3ea\r\n 89203c1d62dc901ed5dfcde751144c5f35d5bdc669981f513e30cc78ee56c18fb30c169ec434dc78012f\r\n 13fde9f8dc04e1fa3cf1bbc3daceb55c361a75f4fa1"; // Your hex data
//        string key = "12345678901234567890123456789012"; // Dummy 32-character key
//        string iv = "1234567890123456"; // Dummy 16-character IV

//        try
//        {
//            string decryptedText = AesDecryption.DecryptHexString(hexString, key, iv);
//            Console.WriteLine($"Decrypted Text: {decryptedText}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Decryption failed: {ex.Message}");
//        }
//    }
//}
