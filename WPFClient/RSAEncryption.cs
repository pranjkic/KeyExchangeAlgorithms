using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient
{
    public class RSAEncryption
    {
        public static void Encrypt(IKeyExchange RSAKeyExchangeChannel, string message)
        {
            byte[] serverPublicKey = RSAKeyExchangeChannel.GetPublicKey();

            using (RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider())
            {
                rsaKey.ImportCspBlob(serverPublicKey);
                byte[] encryptedSessionKey = null;
                byte[] encryptedMessage = null;
                byte[] iv = null;

                Send(rsaKey, message, out iv, out encryptedSessionKey, out encryptedMessage);
                RSAKeyExchangeChannel.Receive(iv, encryptedSessionKey, encryptedMessage);
            }
        }

        private static void Send(RSA key, string secretMessage, out byte[] iv, out byte[] encryptedSessionKey, out byte[] encryptedMessage)
        {
            using (Aes aes = new AesCryptoServiceProvider())
            {
                iv = aes.IV;

                RSAOAEPKeyExchangeFormatter keyFormatter = new RSAOAEPKeyExchangeFormatter(key);
                encryptedSessionKey = keyFormatter.CreateKeyExchange(aes.Key, typeof(Aes));

                using (MemoryStream ciphertext = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ciphertext, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] plaintextMessage = Encoding.UTF8.GetBytes(secretMessage);
                    cs.Write(plaintextMessage, 0, plaintextMessage.Length);
                    cs.Close();

                    encryptedMessage = ciphertext.ToArray();
                }
            }
        }
    }
}
