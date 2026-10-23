using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HMAC_Auth_Signature
{
    public static class HmacHelper
    {
        public static string GenerateHmacHeader(
            string appId,
            string apiKey,
            string httpMethod,
            string url,
            object requestBodyObj)
        {
            string nonce = Guid.NewGuid().ToString("N");
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            string requestUri = WebUtility.UrlEncode(url.ToLower());

            string requestBody = requestBodyObj != null
                ? JsonSerializer.Serialize(requestBodyObj)
                : "";

            string bodyHash = GetMd5HashBase64(requestBody);

            string signatureRawData = appId +
                                      httpMethod.ToUpper() +
                                      requestUri +
                                      timestamp +
                                      nonce +
                                      bodyHash;

            string signature = ComputeHmacSha256(signatureRawData, apiKey);

            return $"hmacauth {appId}:{signature}:{nonce}:{timestamp}";
        }

        private static string GetMd5HashBase64(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            using (var md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hashBytes);
            }
        }

        private static string ComputeHmacSha256(string data, string base64Key)
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
}
