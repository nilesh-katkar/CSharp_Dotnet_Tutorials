//using System;
//using System.Net.Http;
//using System.Text;
//using System.Text.Json;
//using System.Security.Cryptography;
//using System.Threading.Tasks;

//public class PublicData
//{
//    public string ip { get; set; }
//    public string osType { get; set; }
//    public string uuid { get; set; }
//    public string imei { get; set; }
//    public string phoneType { get; set; }
//    public string mac { get; set; }
//    public string address { get; set; }
//}

//public class BizData
//{
//    public PublicData publicData { get; set; }
//    public string openId { get; set; }
//    public string acctNo { get; set; }
//}

//public class ApiRequestFinal
//{
//    public string requestId { get; set; }
//    public string channelId { get; set; }
//    public string interfaceId { get; set; }
//    public string timestamp { get; set; }
//    public string signature { get; set; }
//    public string bizData { get; set; } // Encrypted JSON string
//    public string randomKeyEncrypted { get; set; } // RSA Encrypted AES Key
//}

//class Program
//{
//    static async Task Main(string[] args)
//    {
//        var bizData = new BizData
//        {
//            publicData = new PublicData
//            {
//                ip = "223.104.247.109",
//                osType = "Microsoft Windows NT 10.0.26100.0",
//                uuid = "b1bf3c92-2a65-41fc-89cf-6197bc4ef25c",
//                imei = "7f295d52-5f2f-4d2c-b5e4-97bf46eb05ca",
//                phoneType = "HUAWEI META 10",
//                mac = "005056BD1488",
//                address = "??,??,???"
//            },
//            openId = "5650401403990002",
//            acctNo = "NRA56504142010500000437"
//        };

//        string bizDataJson = JsonSerializer.Serialize(bizData);
//        string aesKey = GenerateRandomAESKey(); // Base64 encoded key

//        string encryptedBizData = AESEncrypt(bizDataJson, aesKey);

//        string bankPublicKeyPem = @"
//            -----BEGIN PUBLIC KEY-----
//            MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAnU1rM+mKcFLOtqzMyomG
//            eLmJHe4ceWL2i/jdPLzS1lkmjTIUKVoOksFqPPXEjwTI8um7bWktNq1oG5o3EyW+
//            8/xxGuWQWWG7t10QrV5kaXoEqMdT2GiXShDYkX3pKE6fK4i6RYf1p45dXtaQYP5+
//            ZG/qjKj+y6C+pCCsT0F/qcFRDdKBI3uZVh7ZbOPLqeqMPSOP8T4plVQbC8XieUdu
//            3PQ78bCIbbxwGcIbLoaP2U0EF3TytkF62XHwXGZBkCKQlHFox2wF/X+SAvb5e7Ll
//            Dp+0AMVHKc8YbIKeZ+yhPLhFtvMZYvN5H/k1QalMTJbmbmF9r+uNQH9OLsBxNCrx
//            yQIDAQAB
//            -----END PUBLIC KEY-----";

//        string encryptedKey = RSAEncrypt(aesKey, bankPublicKeyPem);

//        string requestId = "20221744106340305";
//        string channelId = "Tangopay";
//        string interfaceId = "od0001";
//        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

//        string toSign = requestId + channelId + interfaceId + timestamp + encryptedBizData + encryptedKey;
//        string signature = GenerateMD5(toSign);

//        var finalPayload = new ApiRequestFinal
//        {
//            requestId = requestId,
//            channelId = channelId,
//            interfaceId = interfaceId,
//            timestamp = timestamp,
//            bizData = encryptedBizData,
//            randomKeyEncrypted = encryptedKey,
//            signature = signature
//        };

//        string jsonPayload = JsonSerializer.Serialize(finalPayload);
//        Console.WriteLine("📦 Final Encrypted Payload:");
//        Console.WriteLine(jsonPayload);

//        using var httpClient = new HttpClient();
//        string apiUrl = "https://124.160.111.102:9000/psp/skywalking/api/transactions";
//        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

//        try
//        {
//            var response = await httpClient.PostAsync(apiUrl, content);
//            var responseBody = await response.Content.ReadAsStringAsync();

//            Console.WriteLine("\n✅ API Response:");
//            Console.WriteLine(responseBody);
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine("\n❌ Error:");
//            Console.WriteLine(ex.Message);
//        }
//    }

//    static string AESEncrypt(string plainText, string base64Key)
//    {
//        byte[] keyBytes = Convert.FromBase64String(base64Key);
//        using Aes aes = Aes.Create();
//        aes.Key = keyBytes;
//        aes.Mode = CipherMode.ECB;
//        aes.Padding = PaddingMode.PKCS7;

//        using var encryptor = aes.CreateEncryptor();
//        byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
//        byte[] encrypted = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
//        return Convert.ToBase64String(encrypted);
//    }

//    static string RSAEncrypt(string base64Key, string publicKeyPem)
//    {
//        string cleanPem = publicKeyPem
//            .Replace("-----BEGIN PUBLIC KEY-----", "")
//            .Replace("-----END PUBLIC KEY-----", "")
//            .Replace("\n", "")
//            .Replace("\r", "")
//            .Trim();

//        byte[] keyBytes = Convert.FromBase64String(cleanPem);
//        using var rsa = RSA.Create();
//        rsa.ImportSubjectPublicKeyInfo(keyBytes, out _);

//        byte[] encrypted = rsa.Encrypt(Convert.FromBase64String(base64Key), RSAEncryptionPadding.OaepSHA256);
//        return Convert.ToBase64String(encrypted);
//    }

//    static string GenerateRandomAESKey()
//    {
//        byte[] key = new byte[16];
//        using var rng = RandomNumberGenerator.Create();
//        rng.GetBytes(key);
//        return Convert.ToBase64String(key); // return Base64 encoded key
//    }

//    static string GenerateMD5(string input)
//    {
//        using var md5 = MD5.Create();
//        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
//        byte[] hashBytes = md5.ComputeHash(inputBytes);
//        StringBuilder sb = new StringBuilder();
//        foreach (var b in hashBytes)
//            sb.Append(b.ToString("x2"));
//        return sb.ToString();
//    }
//}


using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using _00_ZCBB.Utils;

class Program
{
    static readonly string publicKey = @"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA17hrshwP2BkRwiRBWUVo
j32xdYlyI9ICjxOXpr1PSYyuCdNClJZlaNpyxOld+QB64muATAY5IbS4Jiwhv6ng
WIB872uCs5exTc76Su6WQ4WdI/CKhQdSXOY/dlrDKeFtyyT5Y9R1fy2tycYESZYG
80BemZyeXmd/ybiWqDuED96FIqqX55Zi26+l489DS7oUvu3mnEsYLKvp5w9LtbZs
eWQIK/StBOXaxBleHHtKYO/y3H1JVcPEk7cENymmWtHLSqYw+h6ilTrZd2cww0c6
xpEuTF3npN14+Sv3TCTSjQ2EwfZIFop6OMOgNLv0IBMwkFXL/UimCgeocLqbPOvc
/QIDAQAB
-----END PUBLIC KEY-----";

    static readonly string privateKey = @"-----BEGIN PRIVATE KEY-----
MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCmWZQ2bM6sOCvC
x4eC1Uq5VTN/vNbFZ/6ZUK4SvkS9iyBln6nrZYYKFgU8oF/c+QbIOwou134r4JTF
CCNmOfrV/ubK4cSbv6qMw6RyU37NBZ3uD6HnDlqKpEmYSsq5gdLVGwM1iS/72lZv
gInoHZJcJ+ltEjhRlQR+t5ACJSii2bqF6+6h6/pKg2FR4MjWMqK3fdKNgj4AbpIL
aiyYEo/NH/iI5U9/STUMyLayI6akXKca3h5SuV+zR9CBu19vWEtNp2n9p4ZXyr31
CaNAWZu1lRM2XP306SPfVD+EyjPazCL5r3vzqo04eR3i1xcuzgLjp1upXsM2Htp/
YjemMz0nAgMBAAECggEAPisXMXnbiVBaxmoEUlh9GyNk4jXEPDVf2JNm4tlZ3k56
9o/DlJY2EFnnbh9vhClLKBSsexIM9L/zSWl/tUow3L3tKeBTNnPcfMv3DsT2jA+v
1JLiCnR6UPsSCAjXl5mVnhAA1IanOKQCDpzGrUkz9B30Zk9bXd3ntqXKe4rAk49a
HgI+5SZujIgK3kmvsXLIjKd1xXUStAQzSm4+vFHmqIDmqkYYNS9nqBBGkc6ZrBDK
5mGLJGo0pyFECh9UdqcrGQQJ+qy4O+eCyEfMq9JwnDqh2Sj/2MLxkI3gc5s6HvOI
rswzxEWyx1HNPYpQMuilE2wzFOLj7HjmtK4N7l0wNQKBgQDQKpzdMQ/db5PbtMkz
l4AyMd1KGd6rwbjUTDS3UDGA/fR2Rt3wU/u9dpKY0WIIRp+qXcDjiKnW7VFX0p3K
6AAgx0Wuv5+Kih2dPKEgrOXiShgoYXJKnIH/pjL+vqgDXGf6Ruk+WXEpDaVTTddh
ogi6E1XzBX9AjbpNiCxz0l9XWwKBgQDMkxv36vAXCKMHWQ2XRW8HOjWTnuhJlfCJ
Zqmt7PmgPJUvDoc3wEVnqcdRJnchoiM9xTcX49ECNVuULKh1GAxX6AeiOQhwcXbv
wQp3kuRGKXQrA/axwlPVOcH0iTNhgeFypjZJjgWJmKsdtbldDzXSvXBAylEF5ZOR
Ytavf7JnJQKBgEE7aUajTj5s7Z+mynohWqngGGFVNAMMxNe6PbbIKYsricTatkfO
eY/IrW24B4liYDxp/r0OoXJZkej8djgUdZ3P9FUrdVpbp6YTTsbHh45e+60DxGX/
H3m2YjqUEmBjP64dNJA2RRDBVvKA7SKKPUPaNU/fhB39i8kqCAwPe5MLAoGBAJP3
S2F0Epf2bUShSs1BMV8+4QWXCFhBFSusj8n2/07Awv64OWE/tnOWP04Yzk4LYgYJ
oSGsa753LXftHSMkc7kZYHwbWFlMGEStkY6PNoDw9s8uHL7R2aT+E6V1ny5FrPUp
91YYXTT6sbnZeOswVzPIRWr3ascjGCsuHC3v5O/pAoGAb98FUkLQjYh4SmpjUWAT
QuiUROD1wJk9mkqxqBWHHrZpcV8xkbxqgMpnTPm9qaiK4jPMTg8DLwKxmbLhkDzN
GX5jpopNCgRSquZhRC3tsGsRXfbnuO/7+FXnn6+eD/1sPHtcuioSSPp221jJAGFS
QieTC5Ato29UQbVw+hcrseY=
-----END PRIVATE KEY-----
";

    static async System.Threading.Tasks.Task Main(string[] args)
    {
        var url = "http://124.160.24.126:9000/psp/skywalking/api/transaction";

        // 1. Prepare original bizData
        var publicData = new Dictionary<string, object>
        {
            { "ip", "223.104.247.109" },
            { "osType", Environment.OSVersion.ToString() },
            { "uuid", Guid.NewGuid().ToString() },
            { "imei", Guid.NewGuid().ToString() },
            { "phoneType", "HUAWEI META 10" },
            { "mac", "005056BD1488" },
            { "address", "浙江,杭州,上城区" }
        };

        var bizData = new Dictionary<string, object>
        {
            { "publicData", publicData },
            { "openId", "5650401403990002" },
            { "acctNo", "NRA56504142010500000437" }
        };

        // 2. Generate AES random key
        string aesKey = AlgorithmUtil.GetAESRandomKey();

        // 3. Encrypt bizData with AES
        string encryptedBizData = AlgorithmUtil.EncryptWithAES(JsonConvert.SerializeObject(bizData), aesKey);

        // 4. Encrypt AES key with RSA public key
        string randomKeyEncrypted = AlgorithmUtil.EncryptWithRSA(aesKey, publicKey);

        // 5. Build top-level JSON
        var topMap = new SortedDictionary<string, object>
        {
            { "requestId", "2022" + DateTimeOffset.Now.ToUnixTimeMilliseconds() },
            { "channelId", "xpress" },
            { "interfaceId", "od0026" },
            { "timestamp", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() },
            { "randomKeyEncrypted", randomKeyEncrypted },
            { "bizData", encryptedBizData }
        };

        // 6. Create string to sign (sorted JSON without "signature")
        string sortedJson = JsonConvert.SerializeObject(topMap);

        // 7. Sign the sorted JSON
        string signature = AlgorithmUtil.Sign(sortedJson, privateKey);

        // 8. Add signature to the final request
        topMap.Add("signature", signature);

        // 9. Serialize final request JSON
        string finalRequest = JsonConvert.SerializeObject(topMap);
        Console.WriteLine("==== Encrypted & Signed Request ====");
        Console.WriteLine(finalRequest);

        // 10. Send HTTP POST request
        using var httpClient = new HttpClient();
        var content = new StringContent(finalRequest, Encoding.UTF8, "application/json");

        Console.WriteLine("==== Sending Request... ====");
        var response = await httpClient.PostAsync(url, content);
        string responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine("==== Server Response ====");
        Console.WriteLine(responseBody);
    }
}

