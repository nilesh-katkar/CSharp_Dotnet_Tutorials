using HMAC_Auth_Signature;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var requestObj = new
        {
            agentSessionId = "1784636867"
        };

        string header = HmacHelper.GenerateHmacHeader(
            "wizz.api",
            "ZGE4YWIyNzU0ZjFlNDNlMWJmNzU0MzAyOGI4ODhjMWE=",
            "POST",
            "https://staging-api-lqn.lightnet.io/api/WebService/GetEcho",
            requestObj
        );

        Console.WriteLine(header);
    }

    static void Main1()
    {
        string appId = "wizz.api";
        string apiKey = "ZGE4YWIyNzU0ZjFlNDNlMWJmNzU0MzAyOGI4ODhjMWE="; // base64 encoded secret
        string httpMethod = "POST";
        string url = "https://staging-api-lqn.lightnet.io/api/WebService/GetEcho";

        var requestObj = new
        {
            agentSessionId = "1784636867"
        };

        string requestBody = System.Text.Json.JsonSerializer.Serialize(requestObj);

        //string requestBody = "{ \"amount\": 1000 }";

        string nonce = Guid.NewGuid().ToString("N");
        string timestamp = GetUnixTimestamp();

        string requestUri = WebUtility.UrlEncode(url.ToLower());

        string bodyHash = GetMd5HashBase64(requestBody);

        // Build raw signature string (NO delimiters)
        string signatureRawData = appId + httpMethod.ToUpper() + requestUri + timestamp + nonce + bodyHash;

        // Generate HMACSHA256 signature
        string signature = ComputeHmacSha256(signatureRawData, apiKey);

        // Final Authorization header
        string authorizationHeader = $"hmacauth {appId}:{signature}:{nonce}:{timestamp}";

        Console.WriteLine("Authorization Header:");
        Console.WriteLine(authorizationHeader);
    }

    // Generate UNIX timestamp
    static string GetUnixTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
    }

    // MD5 hash → Base64
    static string GetMd5HashBase64(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";

        using (var md5 = MD5.Create())
        {
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hashBytes);
        }
    }

    // HMAC SHA256
    static string ComputeHmacSha256(string data, string base64Key)
    {
        byte[] keyBytes = Convert.FromBase64String(base64Key);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);

        using (var hmac = new HMACSHA256(keyBytes))
        {
            byte[] hash = hmac.ComputeHash(dataBytes);
            return Convert.ToBase64String(hash);
        }
    }
}