using System;

class Program
{
    static void Main()
    {
        string[] testAddresses = new[]
        {
            "THILLANKERI,P.O.THILLANKERI-,KANNUR", // Exactly 35 chars
            "12345 Long Street Name That Exceeds Thirty-Five Characters", // >35 chars, spaces
            "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", // 42 chars, no spaces
            "Short Addr", // < 35 chars
            "", // Empty
            null, // Null
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890", // Exactly 36 chars
            "NoSpaceButMoreThanThirtyFiveCharactersExactlyAsYouSeeItHere" ,// single long word
            "This address is definitely going to be very very long and exceed 140 characters because we are testing all 4 beneficiary detail fields with long input",
            new string('X', 140), // 35 * 4
            new string('Y', 139),
            new string('Z', 141),
            "     A   B     C    D     E     ",
            "#123-Apt! Main@Road$%^&*()_+=-[]{}|;':,./<>?"
        };

        int maxLength = 35;

        foreach (var address in testAddresses)
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Input Address: {(address ?? "NULL")}");
            Console.WriteLine($"Length: {address?.Length ?? 0}");

            for (int i = 0; i < 4; i++)
            {
                string part = GetTrimmedAddressPart(address, i, maxLength);
                Console.WriteLine($"Part {i + 1}: \"{part}\" (Length: {part.Length})");
            }

            Console.WriteLine();
        }

        Console.WriteLine("✅ All tests completed.");
    }

    static string GetTrimmedAddressPart(string? address, int partIndex, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(address))
            return string.Empty;

        int start = partIndex * maxLength;
        if (start >= address.Length)
            return string.Empty;

        int length = Math.Min(maxLength, address.Length - start);
        return address.Substring(start, length);
    }
}
