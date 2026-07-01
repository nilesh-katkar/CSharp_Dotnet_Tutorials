using System;
using System.Text;
using System.Text.RegularExpressions;

namespace AddressSanitizerTest
{
    public class RetailTransactionBaseDomainModel
    {
        private string? _senderAddress;

        public string? SenderAddress
        {
            get => _senderAddress;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    // Replace real and escaped newlines
                    string sanitized = value
                        .Replace("\r\n", " ")
                        .Replace("\n", " ")
                        .Replace("\r", " ")
                        .Replace("\\n", " ")
                        .Replace("\\r", " ");

                    // Remove special characters
                    string[] specialChars = new string[]
                    {
                        "`", "~", "!", "@", "#", "$", "%", "^", "*", "(", ")", "_",
                        "=", "+", "{", "}", "[", "]", "|", "\\", ":", ";", "\"", "'",
                        "<", ">", "?", "/"
                    };

                    foreach (string ch in specialChars)
                    {
                        sanitized = sanitized.Replace(ch, " ");
                    }

                    sanitized = Regex.Replace(sanitized, @"\s+", " ").Trim();

                    _senderAddress = sanitized;
                }
                else
                {
                    _senderAddress = value;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var model = new RetailTransactionBaseDomainModel();

            Console.WriteLine("Paste Sender Address (multi-line). Press Ctrl+Z (then Enter) to finish input:");

            string multilineInput = ReadMultilineInput();

            model.SenderAddress = multilineInput;

            Console.WriteLine("\nSanitized Sender Address:");
            Console.WriteLine(model.SenderAddress);

            // Hardcoded test
            Console.WriteLine("\n--- Testing with hardcoded input ---");
            //string testInput = "Flat #42\nNear Road@Main St.\rOpp Bank!";
            string testInput = "THILLANKERI,P.O.THILLANKERI -,KANNUR";
            
            model.SenderAddress = testInput;

            Console.WriteLine("Original:\n" + testInput);
            Console.WriteLine("Sanitized:\n" + model.SenderAddress);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static string ReadMultilineInput()
        {
            StringBuilder inputBuilder = new StringBuilder();
            string? line;
            while ((line = Console.ReadLine()) != null)
            {
                inputBuilder.AppendLine(line);
            }
            return inputBuilder.ToString();
        }
    }
}
