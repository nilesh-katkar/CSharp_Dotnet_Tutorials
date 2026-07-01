using System;

namespace _00_ZCBB.Sign
{
    /// <summary>
    /// Abstract base class for signature handling (sign, verify, encrypt, decrypt).
    /// </summary>
    public abstract class SignatureHandlerClient
    {
        protected SignatureHandlerClient apiHttpSignatureHandler;

        protected SignatureHandlerClient() { }

        /// <summary>
        /// Sign the data.
        /// </summary>
        /// <param name="data">Data to sign</param>
        /// <returns>Signed data as string</returns>
        /// <exception cref="TranFailException"></exception>
        public abstract string Sign(string data);

        /// <summary>
        /// Decrypt the data.
        /// </summary>
        /// <param name="data">Encrypted data</param>
        /// <returns>Decrypted JSON string</returns>
        /// <exception cref="TranFailException"></exception>
        public abstract string Decrypt(string data);

        /// <summary>
        /// Encrypt the data.
        /// </summary>
        /// <param name="data">Plain JSON string</param>
        /// <returns>Encrypted data string</returns>
        /// <exception cref="TranFailException"></exception>
        public abstract string Encrypt(string data);

        /// <summary>
        /// Verify the signature of incoming data.
        /// </summary>
        /// <param name="data">Data with signature</param>
        /// <returns>True if verified, otherwise false</returns>
        public abstract bool Verify(string data);

        public void SetApiHttpSignatureHandler(SignatureHandlerClient apiHttpSignatureHandler)
        {
            this.apiHttpSignatureHandler = apiHttpSignatureHandler;
        }
    }
}
