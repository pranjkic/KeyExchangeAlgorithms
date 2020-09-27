using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class DiffeHellmanEncryption
    {
        public static void Encrypt(IKeyExchange DiffeHellmanKeyExchangeChannel)
        {
            byte[] alicePublicKey;
            using (ECDiffieHellmanCng alice = new ECDiffieHellmanCng())
            {
                alice.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                alice.HashAlgorithm = CngAlgorithm.Sha256;
                alicePublicKey = alice.PublicKey.ToByteArray();
                //Bob bob = new Bob();
                byte[] bobKey = DiffeHellmanKeyExchangeChannel.GetPublicKey();
                CngKey k = CngKey.Import(/*bob.bobPublicKey*/bobKey, CngKeyBlobFormat.EccPublicBlob);
                byte[] aliceKey = alice.DeriveKeyMaterial(CngKey.Import(/*bob.bobPublicKey*/bobKey, CngKeyBlobFormat.EccPublicBlob));
                byte[] encryptedMessage = null;
                byte[] iv = null;

                Console.WriteLine("Insert message: ");
                string message = Console.ReadLine();

                Send(aliceKey, message, out encryptedMessage, out iv);
                DiffeHellmanKeyExchangeChannel.Receive(encryptedMessage, iv, alicePublicKey);
            }
        }

        private static void Send(byte[] key, string secretMessage, out byte[] encryptedMessage, out byte[] iv)
        {
            using (Aes aes = new AesCryptoServiceProvider())
            {
                aes.Key = key;
                iv = aes.IV;

                // Encrypt the message
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
