using System;
using System.Text;
using System.Security.Cryptography;

public class ValidateCallbackHmacMain
{
    public static void Main(string[] args)
    {
        string hmac = GenerateHmac("usernameKey", "callbackUrl");
        Console.WriteLine(hmac);
    }

    public static string GenerateHmac(string username, string url)
    {
        string nonce = "1764159595464"; // Replace with nonce from header
        string date = "2025-11-26T12:19:55.464701479"; // Replace with date from header

        // Convert secret key
        byte[] secret = Encoding.UTF8.GetBytes(username);

        // Create message
        string message = url + nonce + date;
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);

        // Compute HMAC SHA256
        using (var hmac = new HMACSHA256(secret))
        {
            byte[] hash = hmac.ComputeHash(messageBytes);

            // Convert to Base64
            return Convert.ToBase64String(hash);
        }
    }
}
