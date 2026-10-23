using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coins
{
    using System;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    public class AccountDetailsRequestSigner
    {
        static async Task Main4(string[] args)
        {
            var signer = new AccountDetailsRequestSigner(
                apiKey: "Z6Ir90UWNtQMnW5RbpvfVjPyFCyQVZzlAi1YcEIyNoW16Ye6tTvsDt5W4pAzCJl4",
                secret: "uRUeHOV8BzHv9BjVmg1PsQm3HFep5l2YAflQ9EfCcCNhbNrJIuwJeikzHwAhr5Ga" // <-- put your real secret here
            );

            var result = await signer.GetAccountDetailsAsync();

            Console.WriteLine($"Status: {(int)result.StatusCode} {result.StatusCode}");
            Console.WriteLine($"Body: {result.Body}");
        }

        private readonly string _apiKey;
        private readonly string _secret;
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.9001.pl-qa.coinsxyz.me";
        private const string Path = "openapi/v1/account";

        public AccountDetailsRequestSigner(string apiKey, string secret)
        {
            _apiKey = apiKey;
            _secret = secret;
            _httpClient = new HttpClient();
        }

        public async Task<(System.Net.HttpStatusCode StatusCode, string Body)> GetAccountDetailsAsync()
        {
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

            // Matches the captured query string exactly: bare "recvWindow" (no value),
            // then timestamp. Signed string excludes the signature param.
            string rawQueryString = $"recvWindow&timestamp={timestamp}";

            string signature = ComputeHmacSha256Hex(rawQueryString, _secret);

            string fullUrl = $"{BaseUrl}/{Path}?{rawQueryString}&signature={signature}";

            using var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
            request.Headers.Add("X-COINS-APIKEY", _apiKey);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            string body = await response.Content.ReadAsStringAsync();

            return (response.StatusCode, body);
        }

        private static string ComputeHmacSha256Hex(string message, string secret)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using var hmac = new HMACSHA256(keyBytes);
            byte[] hashBytes = hmac.ComputeHash(messageBytes);

            var sb = new StringBuilder(hashBytes.Length * 2);
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }
}
