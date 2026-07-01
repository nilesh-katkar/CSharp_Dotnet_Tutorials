//using System;
//using System.Security.Cryptography;
//using System.Text;

//class ProgramProd
//{
//    static void Main()
//    {
//        // Key and IV
//        string key = "18d32e222087fc8e50eff30b628e5639";
//        string iv = "bb755e6e551fb379";

//        // Encrypted Hex String (from the encryption output)
//        string encryptedHex = "171718313425945ab7413110f6123c3cb7da21ce9ccf8f9d62438d32f650e430f2e29882f7af75cc2bc731c6252d0c8adfdd4b66244a8d9a366462f9ea0f04e4afde43636afe2df69ef11ec19698adf20d460a1e20fe92ca069e2d93fb5cd74f586e282943133f50bf3c30bb1002c9c5031cf01bda63434b93c4dcaf57bb9505e1a671738d51193bd4ed93fa13a809bb8924207497bd01d3235fae7b88c3bb53b4ee09cc275b9ad2f6cb140dd9ff5630bf44721d5ef60e7aef273185db3dcc10a7e7db4773cb8a21137b4e049e2f369925d6ddf2875f4452d0fb5f01efff33eca3cc8a927ccf96d8f4dfbef5c6a01688";

//        string decryptedRequest = Decrypt(encryptedHex, key, iv);
//        Console.WriteLine("Decrypted Request Body:");
//        Console.WriteLine(decryptedRequest);
//    }

//    static string Decrypt(string encryptedHex, string key, string iv)
//    {
//        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
//        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
//        byte[] encryptedBytes = Convert.FromHexString(encryptedHex);

//        using (Aes aes = Aes.Create())
//        {
//            aes.Key = keyBytes;
//            aes.IV = ivBytes;
//            aes.Mode = CipherMode.CBC;
//            aes.Padding = PaddingMode.PKCS7;

//            using (ICryptoTransform decryptor = aes.CreateDecryptor())
//            {
//                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
//                return Encoding.UTF8.GetString(decryptedBytes);
//            }
//        }
//    }
//}
