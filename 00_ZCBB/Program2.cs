//using System;
//using System.Net.Http;
//using System.Text;
//using System.Security.Cryptography;
//using Newtonsoft.Json;
//using System.Threading.Tasks;

//public class Program2
//{
//    private static readonly string apiUrl = "https://124.160.111.102:9000/psp/skywalking/api/transactions";

//    // Replace with actual keys
//    private static readonly string rsaPublicKeyXml = "<RSAKeyValue>...</RSAKeyValue>";
//    private static readonly string rsaPrivateKeyXml = "<RSAKeyValue>...</RSAKeyValue>";

//    public static async Task SendEncryptedRequestAsync()
//    {
//        var requestId = Guid.NewGuid().ToString();
//        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
//        var channelId = "yourChannelId";
//        var interfaceId = "od0001";

//        // Sample bizData
//        var bizDataObject = new
//        {
//            publicData = new
//            {
//                ip = "192.168.1.10",
//                osType = "Windows 10",
//                uuid = "abc-uuid-123",
//                imei = "123456789012345",
//                phoneType = "Dell XPS",
//                mac = "00-14-22-01-23-45",
//                address = "123 Main St"
//            },
//            columm1 = "Value1",
//            columm2 = "Value2"
//        };

//        string bizDataJson = JsonConvert.SerializeObject(bizDataObject);

//        // 1. Generate random AES key
//        byte[] aesKey = GenerateRandomBytes(16); // 128-bit
//        byte[] encryptedBizData = EncryptWithAes(bizDataJson, aesKey);
//        string bizDataBase64 = Convert.ToBase64String(encryptedBizData);

//        // 2. Encrypt AES key with bank’s RSA public key
//        string randomKeyEncrypted = EncryptRsa(aesKey, rsaPublicKeyXml);

//        // 3. Construct data to sign
//        var dataToSign = new
//        {
//            randomKeyEncrypted,
//            requestId,
//            channelId,
//            interfaceId,
//            timestamp,
//            bizData = bizDataBase64
//        };

//        string signString = $"{randomKeyEncrypted}{requestId}{channelId}{interfaceId}{timestamp}{bizDataBase64}";
//        string signature = SignData(signString, rsaPrivateKeyXml);

//        // 4. Construct final payload
//        var finalPayload = new
//        {
//            randomKeyEncrypted,
//            requestId,
//            channelId,
//            interfaceId,
//            timestamp,
//            signature,
//            bizData = bizDataBase64
//        };

//        string jsonPayload = JsonConvert.SerializeObject(finalPayload);

//        using var httpClient = new HttpClient();
//        var response = await httpClient.PostAsync(apiUrl, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
//        string result = await response.Content.ReadAsStringAsync();

//        Console.WriteLine("Response from Bank:\n" + result);
//    }

//    private static byte[] GenerateRandomBytes(int length)
//    {
//        using var rng = new RNGCryptoServiceProvider();
//        byte[] key = new byte[length];
//        rng.GetBytes(key);
//        return key;
//    }

//    private static byte[] EncryptWithAes(string plainText, byte[] key)
//    {
//        using var aes = Aes.Create();
//        aes.Key = key;
//        aes.GenerateIV();

//        using var encryptor = aes.CreateEncryptor();
//        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
//        byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

//        // Combine IV and ciphertext
//        byte[] combined = new byte[aes.IV.Length + cipherBytes.Length];
//        Buffer.BlockCopy(aes.IV, 0, combined, 0, aes.IV.Length);
//        Buffer.BlockCopy(cipherBytes, 0, combined, aes.IV.Length, cipherBytes.Length);

//        return combined;
//    }

//    private static string EncryptRsa(byte[] data, string publicKeyXml)
//    {
//        using var rsa = new RSACryptoServiceProvider();
//        rsa.FromXmlString(publicKeyXml);
//        byte[] encrypted = rsa.Encrypt(data, false);
//        return Convert.ToBase64String(encrypted);
//    }

//    private static string SignData(string data, string privateKeyXml)
//    {
//        using var rsa = new RSACryptoServiceProvider();
//        rsa.FromXmlString(privateKeyXml);
//        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
//        byte[] signedBytes = rsa.SignData(dataBytes, CryptoConfig.MapNameToOID("SHA256"));
//        return Convert.ToBase64String(signedBytes);
//    }
//}
