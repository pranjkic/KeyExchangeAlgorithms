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
    public class DiffeHellmanKeyExchange : IKeyExchange
    {
        public byte[] GetPublicKey()
        {
            ECDiffieHellmanCng key = DiffeHellmanKey.key;
            return key.PublicKey.ToByteArray();
        }

        public void Receive(byte[] encryptedMessage, byte[] iv, byte[] alicePublicKey)
        {
            using (Aes aes = new AesCryptoServiceProvider())
            {
                aes.Key = DiffeHellmanKey.key.DeriveKeyMaterial(CngKey.Import(/*Alice.alicePublicKey*/alicePublicKey, CngKeyBlobFormat.EccPublicBlob)); ;
                aes.IV = iv;
                // Decrypt the message
                using (MemoryStream plaintext = new MemoryStream())
                {
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
}
