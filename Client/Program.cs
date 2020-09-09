using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IKeyExchange> connection = new ChannelFactory<IKeyExchange>("KeyExchangeService");
            IKeyExchange keyExchangeChannel = connection.CreateChannel();
            try
            {
                byte[] serverPublicKey = keyExchangeChannel.GetPublicKey();

                using (RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider())
                { 
                    rsaKey.ImportCspBlob(serverPublicKey);
                    byte[] encryptedSessionKey = null;
                    byte[] encryptedMessage = null;
                    byte[] iv = null;
                    
                    Console.WriteLine("Insert message: ");
                    string message = Console.ReadLine();
                    Send(rsaKey, message, out iv, out encryptedSessionKey, out encryptedMessage);
                    keyExchangeChannel.Receive(iv, encryptedSessionKey, encryptedMessage);                    
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("To exit press enter.");
            Console.ReadLine();
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
