using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class KeyExchange : IKeyExchange
    {
        public byte[] GetPublicKey()
        {
            RSACryptoServiceProvider rsaKey = RSAKey.rsaKey;
            return rsaKey.ExportCspBlob(false);
        }

        public void Receive(byte[] iv, byte[] encryptedSessionKey, byte[] encryptedMessage)
        {
            using (Aes aes = new AesCryptoServiceProvider())
            {
                aes.IV = iv;

                RSAOAEPKeyExchangeDeformatter keyDeformatter = new RSAOAEPKeyExchangeDeformatter(RSAKey.rsaKey);

                aes.Key = keyDeformatter.DecryptKeyExchange(encryptedSessionKey);

                using (MemoryStream plaintext = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(plaintext, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(encryptedMessage, 0, encryptedMessage.Length);
                    cs.Close();

                    string message = Encoding.UTF8.GetString(plaintext.ToArray());
                    Console.WriteLine(message);
                }
            }
        }
    }
}
