using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PfxToPemConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter full path to the .pfx file:");
            string pfxPath = "C:\\Users\\NileshKatkar\\Downloads\\Certificates 2025 (1)\\Certificates 2025\\xpressmoney.pfx";

            if (!File.Exists(pfxPath))
            {
                Console.WriteLine("File not found.");
                return;
            }

            Console.WriteLine("Enter password for the .pfx file:");
            string? password = Console.ReadLine();

            try
            {
                // Load the certificate
                var certificate = new X509Certificate2(pfxPath, password, X509KeyStorageFlags.Exportable);

                Console.WriteLine("\nSubject: " + certificate.Subject);
                Console.WriteLine("Issuer: " + certificate.Issuer);
                Console.WriteLine("Valid From: " + certificate.NotBefore);
                Console.WriteLine("Valid Until: " + certificate.NotAfter);
                Console.WriteLine("Thumbprint: " + certificate.Thumbprint);
                Console.WriteLine("Has Private Key: " + certificate.HasPrivateKey);

                // Convert to PEM
                string pem = ConvertToPemFile(certificate.RawData);

                // Write PEM to file
                string pemPath = Path.ChangeExtension(pfxPath, ".pem");
                File.WriteAllText(pemPath, pem);

                Console.WriteLine($"\nPEM file generated at: {pemPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading or converting certificate: " + ex.Message);
            }
        }

        public static string ConvertToPemFile(byte[] derBytes)
        {
            string base64 = Convert.ToBase64String(derBytes);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-----BEGIN CERTIFICATE-----");

            int lineLength = 64;
            for (int i = 0; i < base64.Length; i += lineLength)
            {
                sb.AppendLine(base64.Substring(i, Math.Min(lineLength, base64.Length - i)));
            }

            sb.AppendLine("-----END CERTIFICATE-----");
            return sb.ToString();
        }
    }
}


