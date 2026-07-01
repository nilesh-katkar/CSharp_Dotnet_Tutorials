using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace WebhookCallBack
{
    public class VerifyWebhook
    {
        private static readonly string WebhookSecret = "my-top-secret"; // Replace this

        private readonly ILogger _logger;

        public VerifyWebhook(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<VerifyWebhook>();
        }

        [Function("VerifyWebhook")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogInformation("Webhook received.");

            string token = req.Headers.TryGetValues("token", out var t) ? t.FirstOrDefault() ?? "" : "";
            string timestamp = req.Headers.TryGetValues("timestamp", out var ts) ? ts.FirstOrDefault() ?? "" : "";
            string incomingSignature = req.Headers.TryGetValues("signature", out var s) ? s.FirstOrDefault() ?? "" : "";

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            string hashedSecretKey = ComputeSha256Hash(WebhookSecret);
            string hashedRequestBody = ComputeSha256Hash(requestBody);

            string signatureString = $"{token}{hashedSecretKey}{timestamp}{hashedRequestBody}";
            string computedSignature = ComputeSha256HashBase64(signatureString);

            if (computedSignature == incomingSignature)
            {
                _logger.LogInformation("? Signature verified.");
                var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
                // STEP: Deserialize the request body
                var jsonDoc = JsonDocument.Parse(requestBody);
                JsonElement root = jsonDoc.RootElement;

                string transactionNumber = root.GetProperty("data").GetProperty("partner_transaction_id").GetString();
                string status = root.GetProperty("data").GetProperty("status").GetString();

                // STEP: Business logic based on status
                _logger.LogInformation($"Transaction ID: {transactionNumber}, Status: {status}");

                switch (status)
                {
                    case "Processing":
                        _logger.LogInformation($"Transaction {transactionNumber} is still processing.");
                        break;

                    case "Completed":
                        _logger.LogInformation($"? Transaction {transactionNumber} has been completed.");
                        // TODO: Mark transaction as successful in DB or send notification
                        break;

                    case "Failed":
                        _logger.LogWarning($"? Transaction {transactionNumber} failed.");
                        // TODO: Log failure or retry logic
                        break;

                    default:
                        _logger.LogInformation($"Unknown status '{status}' received for Transaction {transactionNumber}.");
                        break;
                }

                await response.WriteStringAsync("Signature verified.");
                return response;
            }
            else
            {
                _logger.LogWarning("? Signature verification failed.");
                var response = req.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                await response.WriteStringAsync("Signature verification failed.");
                return response;
            }
        }

        private static string ComputeSha256Hash(string raw)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(raw);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        private static string ComputeSha256HashBase64(string raw)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(raw);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
