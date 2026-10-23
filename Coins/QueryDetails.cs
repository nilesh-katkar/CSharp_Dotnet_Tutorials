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
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DetailsRequestSigner
    {
        static async Task Main3(string[] args)
        {
            var signer = new DetailsRequestSigner(
                apiKey: "Z6Ir90UWNtQMnW5RbpvfVjPyFCyQVZzlAi1YcEIyNoW16Ye6tTvsDt5W4pAzCJl4",
                secret: "uRUeHOV8BzHv9BjVmg1PsQm3HFep5l2YAflQ9EfCcCNhbNrJIuwJeikzHwAhr5Ga" // <-- put your real secret here
            );

            var queryParams = new Dictionary<string, string>
            {
                ["internalOrderId"] = "1212323299"
            };

            var result = await signer.SendDetailsRequestAsync(queryParams);

            Console.WriteLine($"Status: {(int)result.StatusCode} {result.StatusCode}");
            Console.WriteLine($"Body: {result.Body}");
        }

        private readonly string _apiKey;
        private readonly string _secret;
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.9001.pl-qa.coinsxyz.me";
        private const string Path = "openapi/fiat/v1/details";

        public DetailsRequestSigner(string apiKey, string secret)
        {
            _apiKey = apiKey;
            _secret = secret;
            _httpClient = new HttpClient();
        }

        public async Task<(System.Net.HttpStatusCode StatusCode, string Body)> SendDetailsRequestAsync(
            Dictionary<string, string> queryParams)
        {
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

            // Note: this endpoint's script sets timestamp on the environment but does NOT
            // append it into the signed query string or the final URL (mirroring the
            // captured request, which has no &timestamp= param at all) — only
            // internalOrderId + signature go on the wire. Add ["timestamp"] = timestamp
            // to queryParams below if your API actually expects it on other calls.
            string rawQueryString = string.Join("&",
                queryParams.Select(kv => $"{kv.Key}={kv.Value}"));

            // Mirror the JS: strip signature placeholder before signing (no-op here since
            // there isn't one in queryParams, but kept for parity with the original script)
            string stringToSign = rawQueryString.Replace("&signature={{sign}}", "");

            string signature = ComputeHmacSha256Hex(stringToSign, _secret);

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
