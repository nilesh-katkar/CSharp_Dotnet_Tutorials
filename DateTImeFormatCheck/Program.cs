using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter time function (e.g., GETUTCDATEIN, GETUTCDATEUS, GETUTCDATE, GETUTCDATEBD, GETUTCDATEPH etc.):");
        string input = Console.ReadLine()?.Trim().ToUpper();

        DateTime utcNow = DateTime.UtcNow;

        if (input == "GETUTCDATE")
        {
            Console.WriteLine($"UTC Time: {utcNow}");
        }
        else if (input.StartsWith("GETUTCDATE"))
        {
            string countryCode = input.Replace("GETUTCDATE", "");
            if (TimeZoneMapping.TryGetValue(countryCode, out string timeZoneId))
            {
                try
                {
                    TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                    DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, tz);
                    Console.WriteLine($"{countryCode} Time ({timeZoneId}): {localTime}");
                }
                catch (TimeZoneNotFoundException)
                {
                    Console.WriteLine($"Time zone '{timeZoneId}' not found on this system.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Country code '{countryCode}' not supported.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input format.");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static readonly Dictionary<string, string> TimeZoneMapping = new()
    {
        { "IN", "India Standard Time" },             // UTC+5:30
        { "US", "Eastern Standard Time" },           // UTC-5:00 / UTC-4:00 DST
        { "GB", "GMT Standard Time" },               // UTC+0:00 / UTC+1:00 DST (UK)
        { "JP", "Tokyo Standard Time" },             // UTC+9:00
        { "AU", "AUS Eastern Standard Time" },       // UTC+10:00 / UTC+11:00 DST
        { "DE", "W. Europe Standard Time" },         // UTC+1:00 / UTC+2:00 DST
        { "CN", "China Standard Time" },             // UTC+8:00
        { "RU", "Russian Standard Time" },           // UTC+3:00
        { "BR", "E. South America Standard Time" },  // UTC-3:00
        { "BD", "Bangladesh Standard Time" },        // UTC+6:00 (Bangladesh)
        { "PH", "Singapore Standard Time" }          // UTC+8:00 (Philippines shares with Singapore)
    };

}
