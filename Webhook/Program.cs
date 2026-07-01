
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

class Program
{
    static void Main()
    {
        string token = "abc123token";
        string secret = "346oj4nPLumsY41CvBlDIrNNyDV6TpZCzxMnFj3F";
        string timestamp = "1720691145";

        //string requestBody = "{\"type\":\"TransferEventNotification\",\"data\":{\"message\":\"Firing TransferRejected Webhook to [https://*****/Webhook2/MoneyMatch]https://*****/Webhook2/MoneyMatch, UUID: #*****\",\"context\":{\"json\":{\"type\":\"TransferRejected\",\"data\":{\"id\":123456,\"ref\":\"RFX3IHWMI9DVOT7\",\"payout_method\":\"ACCOUNT_CREDIT\",\"partner_transaction_id\":\"993165159823\",\"status\":\"Processing\",\"status_remark\":\"ACCOUNT_NAME_NOT_MATCH (refund with fee)\",\"is_supporting_doc_required\":\"0\"}}}}}";

        //string requestBody = "{\"type\":\"TransferEventNotification\",\"data\":{\"message\":\"Firing TransferRejected Webhook to https://example.com/Webhook2/MoneyMatch. UUID: #123e4567-e89b-12d3-a456-426614174000\",\"context\":{\"json\":{\"type\":\"TransferRejected\",\"data\":{\"id\":123456,\"ref\":\"RFX3IHWMI9DVOT7\",\"payout_method\":\"ACCOUNT_CREDIT\",\"partner_transaction_id\":\"993165159823\",\"status\":\"Processing\",\"status_remark\":\"ACCOUNT_NAME_NOT_MATCH (refund with fee)\",\"is_supporting_doc_required\":\"0\"}}}}}";

        var payload = new
        {
            type = "TransferRejected",
            data = new
            {
                id = 123456,
                @ref = "RFX3IHWMI9DVOT7",  // <-- escaped ref keyword
                payout_method = "ACCOUNT_CREDIT",
                partner_transaction_id = "991865141117",
                status = "Rejected",
                status_remark = "Rejected due to accno error",
                is_supporting_doc_required = "0"
            }
        };


        var options = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        string requestBody = JsonSerializer.Serialize(payload, options);


        string hashedSecret = Sha256Hex(secret);
        string hashedRequestBody = Sha256Hex(requestBody);

        string signatureString = token + hashedSecret + timestamp + hashedRequestBody;
        string signature = Sha256Base64(signatureString, hashedSecret);

        Console.WriteLine("Computed Signature:");
        Console.WriteLine(signature);
    }

    static string Sha256Hex(string input)
    {
        using var sha = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(input.Normalize());
        return BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", "").ToLower();
    }

    static string Sha256Base64(string input, string hashedSecret)
    {
        //using var sha = SHA256.Create();
        //byte[] bytes = Encoding.UTF8.GetBytes(input.Normalize());
        //return Convert.ToBase64String(sha.ComputeHash(bytes));

        // HMAC-SHA256 with hashedSecretKey
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(hashedSecret));
        byte[] bytes = Encoding.UTF8.GetBytes(input.Normalize());
        return Convert.ToBase64String(hmac.ComputeHash(bytes));
    }
}
