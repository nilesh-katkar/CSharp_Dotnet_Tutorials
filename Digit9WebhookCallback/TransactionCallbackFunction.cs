using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

public class TransactionCallbackFunction
{
    private readonly ILogger _logger;
    private readonly string _secret;

    public TransactionCallbackFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<TransactionCallbackFunction>();

        // Store in Azure App Settings
        _secret = Environment.GetEnvironmentVariable("PAYMENT_CALLBACK_SECRET");
    }

    [Function("TransactionStatusCallback")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post",
        Route = "transaction-status-callback")] HttpRequestData req)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        _logger.LogInformation($"Callback received: {requestBody}");

        // 1. Read hash header
        if (!req.Headers.TryGetValues("hash", out var hashValues))
        {
            return await BadResponse(req, "Missing hash header");
        }

        string receivedHash = hashValues.First();

        // 2. Validate payload
        TransactionRequest? payload;
        try
        {
            payload = JsonSerializer.Deserialize<TransactionRequest>(requestBody);
        }
        catch
        {
            return await BadResponse(req, "Invalid JSON");
        }

        if (payload == null)
        {
            return await BadResponse(req, "Empty payload");
        }

        // 3. Compute hash (payload + secret)
        string computedHash = ComputeSha512(requestBody + _secret);

        // 4. Validate integrity
        if (!string.Equals(receivedHash, computedHash, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Hash validation failed");
            return await BadResponse(req, "Invalid hash");
        }

        // 5. Business logic (update DB / queue / etc.)
        _logger.LogInformation(
            $"Transaction {payload.transaction_ref_number} => {payload.state}/{payload.sub_state}");

        // TODO: update database / publish event / queue message

        // 6. Return ACKNOWLEDGED (IMPORTANT as per spec)
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new
        {
            state = "ACKNOWLEDGED"
        });

        return response;
    }

    private static string ComputeSha512(string input)
    {
        using var sha = SHA512.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));

        return Convert.ToHexString(bytes).ToLower();
    }

    private static async Task<HttpResponseData> BadResponse(HttpRequestData req, string message)
    {
        var res = req.CreateResponse(HttpStatusCode.BadRequest);
        await res.WriteStringAsync(message);
        return res;
    }
}

public class TransactionRequest
{
    public string transaction_ref_number { get; set; }
    public string state { get; set; }
    public string sub_state { get; set; }
}