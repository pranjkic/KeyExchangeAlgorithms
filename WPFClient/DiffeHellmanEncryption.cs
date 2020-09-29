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
    public class DiffeHellmanEncryption
    {
        public static void Encrypt(IKeyExchange DiffeHellmanKeyExchangeChannel, string message, out byte[] sessionKey)
        {
            byte[] alicePublicKey;
            using (ECDiffieHellmanCng alice = new ECDiffieHellmanCng())
            {
                alice.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                alice.HashAlgorithm = CngAlgorithm.Sha256;
                alicePublicKey = alice.PublicKey.ToByteArray();
                
                byte[] bobKey = DiffeHellmanKeyExchangeChannel.GetPublicKey();
                CngKey cngKey = CngKey.Import(bobKey, CngKeyBlobFormat.EccPublicBlob);
                byte[] aliceKey = alice.DeriveKeyMaterial(cngKey);

                byte[] encryptedMessage = null;
                byte[] iv = null;

                Send(aliceKey, message, out encryptedMessage, out iv, out sessionKey);
                DiffeHellmanKeyExchangeChannel.Receive(encryptedMessage, iv, alicePublicKey);
            }
        }

        private static void Send(byte[] key, string secretMessage, out byte[] encryptedMessage, out byte[] iv, out byte[] aesKey)
        {
            using (Aes aes = new AesCryptoServiceProvider())
            {
                aes.Key = key;
                iv = aes.IV;

                aesKey = aes.Key;

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
