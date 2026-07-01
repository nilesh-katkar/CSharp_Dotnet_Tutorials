//using System;
//using System.Security.Cryptography;
//using System.Text;
//using System.Xml.Linq;
//using Newtonsoft.Json.Linq;

//class DecryptionTest
//{
//    public static void Main()
//    {
//        string json = @"{
//            'desc': '',
//            'interfaceId': 'od0001',
//            'timestamp': '20180911191917',
//            'signature': 'hfwppZzhtaF3ChsoxqO61vt8CzS5u1aHhXkWhKoTZpwzDOJLF2w11G6ncJ16mNZ/3hepbaYrzgUC+TqOTzOyKtmk1/4j80KCr0ZguFmqriyPHe8pa/LdLNCfhOgWEMADZYg/3HLLORZ6fZs3o1DrjMHx2V2PcBpPD0qub27Wi2zggO1mc0WVoaY01sVETqx/1CTXq1ufKSMmJ/DD4eyFlKgXHRLl/6HOQlMTeK2OWLsAL/Qso3da01xkWIcFv7QZtH5RZaQ7gYthg27kN5I5t5KrwYB4hQECuL050ZQ3WV7Ygrhhojzb29tWWZ4LlrAvxlFkOteFqH5XHlaJ2wWI6g==',
//            'randomKeyEncrypted': 'TQNgGqG8TVsX8DZ37zyMIKbhIxeufSfsj9VhojKUw97GkPHaNDbvkfYH0SFNiYWjqzsHseUrGS1lIQl0gq3lhTlY1ZwiecSQLIFWxbnqXeriZ4pggetA0phUMAzRc8WRwWywsuiOzvOj/dsaEvVEXO9fVjaJ9L6EqItES02tDb2BMenHrGeycd0NzRUTt5eNZYaCjUEXZsZn/FM7x5rHW5Q9WohxpNGz4+I2/IEqCLPegxvZGstgbbFbJtccy+g72esjCo+pAWpbaqDiczg1BZqDmtWjFRbjSpfxgdjtLHyPe4efDW39Y6Ax7QZGX+wU3nDlP3q+43a8t/D35SHyHw==',
//            'requestId': 'xmshLU201809110000008059',
//            'code': '0000',
//            'bizData': 'myRSzJCkGfEyBRJXz0ycQeyP1uscMWW0zejy4HoLUmbutAstmgievZGkyP9lL4lCqxGeCU/Hequ7hVAgP57ZAA==',
//            'channelId': 'xmsh'
//        }";

//        // Your RSA Keys (use your actual keys)
//        string publicKeyPem = System.IO.File.ReadAllText("C:\\Users\\NileshKatkar\\Downloads\\bank_public.pem");
//        string privateKeyPem = System.IO.File.ReadAllText("C:\\Users\\NileshKatkar\\Downloads\\xm_private.pem");
//        JObject obj = JObject.Parse(json);

//        string interfaceId = obj["interfaceId"].ToString();
//        string requestId = obj["requestId"].ToString();
//        string timestamp = obj["timestamp"].ToString();
//        string bizData = obj["bizData"].ToString();
//        string signature = obj["signature"].ToString();
//        string randomKeyEncrypted = obj["randomKeyEncrypted"].ToString();

//        string dataToVerify = $"bizData={bizData}&interfaceId={interfaceId}&requestId={requestId}&timestamp={timestamp}";

//        // Step 1: Verify Signature
//        bool isValid = VerifySignature(dataToVerify, signature, publicKeyPem);
//        Console.WriteLine($"Signature Valid: {isValid}");

//        if (!isValid) return;

//        // Step 2: Decrypt AES key
//        string aesKey = RSADecrypt(randomKeyEncrypted, privateKeyPem);
//        Console.WriteLine($"Decrypted AES Key (Base64): {aesKey}");

//        // Step 3: Decrypt bizData
//        string decryptedData = AESDecrypt(bizData, aesKey);
//        Console.WriteLine($"Decrypted bizData: {decryptedData}");
//    }

//    static bool VerifySignature(string data, string base64Signature, string publicKeyPem)
//    {
//        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
//        byte[] sigBytes = Convert.FromBase64String(base64Signature);
//        string cleanPem = publicKeyPem
//            .Replace("-----BEGIN PUBLIC KEY-----", "")
//            .Replace("-----END PUBLIC KEY-----", "")
//            .Replace("\r", "").Replace("\n", "").Trim();

//        byte[] keyBytes = Convert.FromBase64String(cleanPem);

//        using var rsa = RSA.Create();
//        rsa.ImportSubjectPublicKeyInfo(keyBytes, out _);

//        return rsa.VerifyData(dataBytes, sigBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
//    }

//    static string RSADecrypt(string base64EncryptedKey, string privateKeyPem)
//    {
//        string cleanPem = privateKeyPem
//            .Replace("-----BEGIN PRIVATE KEY-----", "")
//            .Replace("-----END PRIVATE KEY-----", "")
//            .Replace("\r", "").Replace("\n", "").Trim();

//        byte[] keyBytes = Convert.FromBase64String(cleanPem);
//        byte[] encryptedKeyBytes = Convert.FromBase64String(base64EncryptedKey);

//        using var rsa = RSA.Create();
//        rsa.ImportPkcs8PrivateKey(keyBytes, out _);

//        byte[] decryptedKey = rsa.Decrypt(encryptedKeyBytes, RSAEncryptionPadding.OaepSHA256);
//        return Convert.ToBase64String(decryptedKey);
//    }

//    static string AESDecrypt(string encryptedText, string base64Key)
//    {
//        byte[] keyBytes = Convert.FromBase64String(base64Key);
//        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

//        using Aes aes = Aes.Create();
//        aes.Key = keyBytes;
//        aes.Mode = CipherMode.ECB;
//        aes.Padding = PaddingMode.PKCS7;

//        using var decryptor = aes.CreateDecryptor();
//        byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

//        return Encoding.UTF8.GetString(decryptedBytes);
//    }
//}


using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

class DecryptionTest
{
    public static void Main()
    {
        string? json = @"{
            'desc': '',
            'interfaceId': 'od0001',
            'timestamp': '20180911191917',
            'signature': 'hfwppZzhtaF3ChsoxqO61vt8CzS5u1aHhXkWhKoTZpwzDOJLF2w11G6ncJ16mNZ/3hepbaYrzgUC+TqOTzOyKtmk1/4j80KCr0ZguFmqriyPHe8pa/LdLNCfhOgWEMADZYg/3HLLORZ6fZs3o1DrjMHx2V2PcBpPD0qub27Wi2zggO1mc0WVoaY01sVETqx/1CTXq1ufKSMmJ/DD4eyFlKgXHRLl/6HOQlMTeK2OWLsAL/Qso3da01xkWIcFv7QZtH5RZaQ7gYthg27kN5I5t5KrwYB4hQECuL050ZQ3WV7Ygrhhojzb29tWWZ4LlrAvxlFkOteFqH5XHlaJ2wWI6g==',
            'randomKeyEncrypted': 'TQNgGqG8TVsX8DZ37zyMIKbhIxeufSfsj9VhojKUw97GkPHaNDbvkfYH0SFNiYWjqzsHseUrGS1lIQl0gq3lhTlY1ZwiecSQLIFWxbnqXeriZ4pggetA0phUMAzRc8WRwWywsuiOzvOj/dsaEvVEXO9fVjaJ9L6EqItES02tDb2BMenHrGeycd0NzRUTt5eNZYaCjUEXZsZn/FM7x5rHW5Q9WohxpNGz4+I2/IEqCLPegxvZGstgbbFbJtccy+g72esjCo+pAWpbaqDiczg1BZqDmtWjFRbjSpfxgdjtLHyPe4efDW39Y6Ax7QZGX+wU3nDlP3q+43a8t/D35SHyHw==',
            'requestId': 'xmshLU201809110000008059',
            'code': '0000',
            'bizData': 'myRSzJCkGfEyBRJXz0ycQeyP1uscMWW0zejy4HoLUmbutAstmgievZGkyP9lL4lCqxGeCU/Hequ7hVAgP57ZAA==',
            'channelId': 'xmsh'
        }";

        // Load keys
        string? publicKeyPem = System.IO.File.ReadAllText("C:\\Users\\NileshKatkar\\Downloads\\bank_public.pem");
        string? privateKeyPem = System.IO.File.ReadAllText("C:\\Users\\NileshKatkar\\Downloads\\xm_private.pem");

        // Parse JSON
        JObject obj = JObject.Parse(json);
        string? interfaceId = obj["interfaceId"]?.ToString();
        string? requestId = obj["requestId"]?.ToString();
        string? timestamp = obj["timestamp"]?.ToString();
        string? bizData = obj["bizData"]?.ToString();
        string? signature = obj["signature"]?.ToString();
        string? randomKeyEncrypted = obj["randomKeyEncrypted"]?.ToString();

        string dataToVerify = $"bizData={bizData}&interfaceId={interfaceId}&requestId={requestId}&timestamp={timestamp}";

        // Step 1: Verify Signature
        bool isValid = VerifySignature(dataToVerify, signature, publicKeyPem);
        Console.WriteLine($"Signature Valid: {isValid}");
        if (!isValid) return;

        // Step 2: Decrypt AES Key
        byte[] aesKeyBytes = RSADecryptToBytes(randomKeyEncrypted, privateKeyPem);
        Console.WriteLine("AES Key (Base64): " + Convert.ToBase64String(aesKeyBytes));

        // Step 3: Decrypt bizData using AES
        string? decryptedBizData = AESDecrypt(bizData, aesKeyBytes);
        Console.WriteLine("Decrypted bizData: " + decryptedBizData);
    }

    static bool VerifySignature(string data, string base64Signature, string publicKeyPem)
    {
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        byte[] signatureBytes = Convert.FromBase64String(base64Signature);

        string? cleanPem = publicKeyPem
            .Replace("-----BEGIN PUBLIC KEY-----", "")
            .Replace("-----END PUBLIC KEY-----", "")
            .Replace("\r", "").Replace("\n", "").Trim();

        byte[] keyBytes = Convert.FromBase64String(cleanPem);

        using var rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(keyBytes, out _);

        return rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    static byte[] RSADecryptToBytes(string base64Encrypted, string privateKeyPem)
    {
        byte[] encryptedBytes = Convert.FromBase64String(base64Encrypted);

        string? cleanPem = privateKeyPem
            .Replace("-----BEGIN PRIVATE KEY-----", "")
            .Replace("-----END PRIVATE KEY-----", "")
            .Replace("\r", "").Replace("\n", "").Trim();

        byte[] keyBytes = Convert.FromBase64String(cleanPem);

        using var rsa = RSA.Create();
        rsa.ImportPkcs8PrivateKey(keyBytes, out _);

        return rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);
    }

    static string AESDecrypt(string base64CipherText, byte[] aesKey)
    {
        byte[] cipherBytes = Convert.FromBase64String(base64CipherText);

        using var aes = Aes.Create();
        aes.Key = aesKey;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor();
        byte[] decrypted = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

        return Encoding.UTF8.GetString(decrypted);
    }
}

