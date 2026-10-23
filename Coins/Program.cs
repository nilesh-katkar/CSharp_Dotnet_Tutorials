
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class WithdrawRequestSigner
{
    static async Task Main(string[] args)
    {
        var signer = new WithdrawRequestSigner(
            apiKey: "Z6Ir90UWNtQMnW5RbpvfVjPyFCyQVZzlAi1YcEIyNoW16Ye6tTvsDt5W4pAzCJl4",
            secret: "uRUeHOV8BzHv9BjVmg1PsQm3HFep5l2YAflQ9EfCcCNhbNrJIuwJeikzHwAhr5Ga" // <-- put your actual secret here
        );

        var queryParams = new Dictionary<string, string>
        {
            ["coin"] = "USDC",
            ["network"] = "ETH",
            ["address"] = "0x1dce8f2494b77bb4e1c5851a109360135ba84ac2",
            ["addressTag"] = "",
            ["amount"] = "1",
            ["withdrawOrderId"] = ""
        };

        var result = await signer.SendWithdrawRequestAsync(queryParams);

        Console.WriteLine($"Status: {(int)result.StatusCode} {result.StatusCode}");
        Console.WriteLine($"Body: {result.Body}");
    }

    private readonly string _apiKey;
    private readonly string _secret;
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.9001.pl-qa.coinsxyz.me";
    private const string Path = "openapi/wallet/v1/withdraw/apply";

    public WithdrawRequestSigner(string apiKey, string secret)
    {
        _apiKey = apiKey;
        _secret = secret;
        _httpClient = new HttpClient();
    }

    public async Task<(System.Net.HttpStatusCode StatusCode, string Body)> SendWithdrawRequestAsync(
        Dictionary<string, string> queryParams)
    {
        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

        var paramsForSigning = new Dictionary<string, string>(queryParams)
        {
            ["timestamp"] = timestamp
        };

        // Preserve insertion order to match the JS behavior (no alphabetical sort)
        string rawQueryString = string.Join("&",
            paramsForSigning.Select(kv => $"{kv.Key}={kv.Value}"));

        string signature = ComputeHmacSha256Hex(rawQueryString, _secret);

        string fullUrl = $"{BaseUrl}/{Path}?{rawQueryString}&signature={signature}";

        using var request = new HttpRequestMessage(HttpMethod.Post, fullUrl);
        request.Headers.Add("X-COINS-APIKEY", _apiKey);
        // Empty body, matching Content-Length: 0 in the captured request
        request.Content = new StringContent(string.Empty, Encoding.UTF8);
        request.Content.Headers.ContentType =
            new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

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



//using System;
//using System.Security.Cryptography;
//using System.Text;
//using System.Collections.Generic;
//using System.Linq;

//public class WithdrawRequestSigner
//{
//    static void Main(string[] args)
//    {
//        var signer = new WithdrawRequestSigner(secret: "your-api-secret");

//        var queryParams = new Dictionary<string, string>
//        {
//            ["appId"] = "your-app-id",
//            ["nonce"] = Guid.NewGuid().ToString("N")
//            // add whatever other query params your API requires
//        };

//        var (path, timestamp, queryString, signature) = signer.BuildSignedRequest(queryParams);

//        // Now build the final URL / headers
//        string finalUrl = $"https://your-host/{path}?{queryString}&signature={signature}&timestamp={timestamp}";
//    }

//    private readonly string _secret;

//    public WithdrawRequestSigner(string secret)
//    {
//        _secret = secret;
//    }

//    /// <summary>
//    /// Builds the query string, computes the HMAC-SHA256 signature,
//    /// and returns everything needed to fire the request.
//    /// </summary>
//    public (string Path, string Timestamp, string QueryString, string Signature) BuildSignedRequest(
//        Dictionary<string, string> queryParams)
//    {
//        const string path = "openapi/wallet/v1/withdraw/apply";
//        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

//        // Ensure timestamp is part of the params used for signing
//        var paramsForSigning = new Dictionary<string, string>(queryParams)
//        {
//            ["timestamp"] = timestamp
//        };

//        // Build the raw query string (order matters if your API expects a specific order -
//        // if it needs alphabetical sorting, sort here; otherwise preserve insertion order)
//        string rawQueryString = string.Join("&",
//            paramsForSigning.Select(kv => $"{kv.Key}={kv.Value}"));

//        // Mirror the JS: strip out any signature placeholder before signing
//        string stringToSign = rawQueryString.Replace("&signature={{sign}}", "");

//        string signature = ComputeHmacSha256Hex(stringToSign, _secret);

//        return (path, timestamp, rawQueryString, signature);
//    }

//    private static string ComputeHmacSha256Hex(string message, string secret)
//    {
//        var keyBytes = Encoding.UTF8.GetBytes(secret);
//        var messageBytes = Encoding.UTF8.GetBytes(message);

//        using var hmac = new HMACSHA256(keyBytes);
//        byte[] hashBytes = hmac.ComputeHash(messageBytes);

//        // Hex string, lowercase, no dashes - matches CryptoJS.enc.Hex.stringify
//        var sb = new StringBuilder(hashBytes.Length * 2);
//        foreach (byte b in hashBytes)
//            sb.Append(b.ToString("x2"));

//        return sb.ToString();
//    }
//}