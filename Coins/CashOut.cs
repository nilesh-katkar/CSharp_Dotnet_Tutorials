using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coins
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    public class CashOutRequestSigner
    {
        static async Task Main2(string[] args)
        {
            var signer = new CashOutRequestSigner(
                apiKey: "Z6Ir90UWNtQMnW5RbpvfVjPyFCyQVZzlAi1YcEIyNoW16Ye6tTvsDt5W4pAzCJl4",
                secret: "uRUeHOV8BzHv9BjVmg1PsQm3HFep5l2YAflQ9EfCcCNhbNrJIuwJeikzHwAhr5Ga" // <-- put your real secret here
            );

            // Replace with your actual cash-out payload fields
            string jsonBody = "{\"internalOrderId\":\"1212323299\",\"currency\":\"PHP\",\"amount\":\"50\",\"channelName\":\"SWIFTPAY_PESONET\",\"channelSubject\":\"gcash\",\"extendInfo\":{\"recipientName\":\"sandy openapitest\",\"recipientAccountNumber\":\"123456789\"}}";

            var result = await signer.SendCashOutRequestAsync(jsonBody);

            Console.WriteLine($"Status: {(int)result.StatusCode} {result.StatusCode}");
            Console.WriteLine($"Body: {result.Body}");
        }

        private readonly string _apiKey;
        private readonly string _secret;
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.9001.pl-qa.coinsxyz.me";
        private const string Path = "openapi/fiat/v1/cash-out";

        public CashOutRequestSigner(string apiKey, string secret)
        {
            _apiKey = apiKey;
            _secret = secret;
            _httpClient = new HttpClient();
        }

        public async Task<(System.Net.HttpStatusCode StatusCode, string Body)> SendCashOutRequestAsync(string jsonBody)
        {
            string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

            // --- CHOOSE THE SIGNING PATTERN THAT MATCHES YOUR API DOCS ---

            // Pattern A: sign "timestamp + jsonBody"
            //string stringToSignA = $"{timestamp}{jsonBody}";

            //// Pattern B: sign just the raw jsonBody
            //string stringToSignB = jsonBody;

            //// Pick one:
            //string stringToSign = stringToSignA;

            string signature = ComputeHmacSha256Hex(jsonBody, _secret);

            string fullUrl = $"{BaseUrl}/{Path}";

            using var request = new HttpRequestMessage(HttpMethod.Post, fullUrl);
            request.Headers.Add("X-COINS-APIKEY", _apiKey);
            request.Headers.Add("timestamp", timestamp);
            request.Headers.Add("signature", signature);

            request.Content = new StringContent(jsonBody, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

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
