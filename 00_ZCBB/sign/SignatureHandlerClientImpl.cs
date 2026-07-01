
using _00_ZCBB.Exceptions;
using _00_ZCBB.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _00_ZCBB.Sign
{
    public class SignatureHandlerClientImpl : SignatureHandlerClient
    {
        private readonly string privateKey;
        private readonly string publicKey;

        private const string SIGNATURE_FIELD = "signature";
        private const string ENCRYPT_FIELD = "bizData";
        private const string ENCRYPT_RANDOMKEY_FIELD = "randomKeyEncrypted";

        public SignatureHandlerClientImpl(string privateKey, string publicKey)
        {
            this.privateKey = privateKey;
            this.publicKey = publicKey;
            base.SetApiHttpSignatureHandler(this);
        }

        public override string Sign(string data)
        {
            try
            {
                JObject origData = JObject.Parse(Encrypt(data));
                origData.Remove(SIGNATURE_FIELD);
                string sortedData = GetSortedJson(origData);
                Console.WriteLine("Client signing raw data: " + sortedData);

                string signData = AlgorithmUtil.Sign(sortedData, privateKey);
                origData[SIGNATURE_FIELD] = signData;

                return origData.ToString(Formatting.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Signature failed. Reason: " + ex.Message);
                throw new TranFailException("Signature failed");
            }
        }

        public override bool Verify(string data)
        {
            try
            {
                Console.WriteLine("Verifying signature raw data: " + data);
                JObject request = JObject.Parse(data);

                string? signData = request[SIGNATURE_FIELD]?.ToString();
                request.Remove(SIGNATURE_FIELD);

                return AlgorithmUtil.Verify(GetSortedJson(request), publicKey, signData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Signature verification failed. Reason: " + ex.Message);
                return false;
            }
        }

        public override string Decrypt(string data)
        {
            try
            {
                JObject origData = JObject.Parse(data);

                string? randomKeyEncrypted = origData[ENCRYPT_RANDOMKEY_FIELD]?.ToString();
                Console.WriteLine("Encrypted random key: " + randomKeyEncrypted);
                string randomKey = AlgorithmUtil.DecryptWithRSA(randomKeyEncrypted, privateKey);
                Console.WriteLine("Decrypted random key: " + randomKey);

                string? encryptedBizData = origData[ENCRYPT_FIELD]?.ToString();
                Console.WriteLine("Encrypted business data: " + encryptedBizData);
                string bizData = AlgorithmUtil.DecryptWithAES(encryptedBizData, randomKey);

                Console.WriteLine("Decrypted business data: " + bizData);
                origData[ENCRYPT_FIELD] = bizData;
                origData.Remove(ENCRYPT_RANDOMKEY_FIELD);

                return origData.ToString(Formatting.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Business data decryption failed. Reason: " + ex.Message);
                throw new TranFailException("Business data decryption failed");
            }
        }

        public override string Encrypt(string data)
        {
            try
            {
                JObject origData = JObject.Parse(data);

                string aesRandomKey = AlgorithmUtil.GetAESRandomKey();
                string randomKeyEncrypted = AlgorithmUtil.EncryptWithRSA(aesRandomKey, publicKey);

                string? bizDataJson = origData[ENCRYPT_FIELD]?.ToString();
                string parameterJsonEncrypted = AlgorithmUtil.EncryptWithAES(bizDataJson, aesRandomKey);

                origData[ENCRYPT_RANDOMKEY_FIELD] = randomKeyEncrypted;
                origData[ENCRYPT_FIELD] = parameterJsonEncrypted;

                return origData.ToString(Formatting.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Business data encryption failed. Reason: " + ex.Message);
                throw new TranFailException("Business data encryption failed");
            }
        }

        private string GetSortedJson(JObject jsonObj)
        {
            SortedDictionary<string, object> sorted = new SortedDictionary<string, object>();
            foreach (var pair in jsonObj)
            {
                sorted[pair.Key] = pair.Value;
                //sorted[pair.Key] = pair.Value != null ? pair.Value.ToObject<object>() : null;
            }

            return JsonConvert.SerializeObject(sorted);
        }
    }
}
