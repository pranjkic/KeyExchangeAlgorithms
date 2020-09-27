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
            ChannelFactory<IKeyExchange> RSAconnection = new ChannelFactory<IKeyExchange>("RSAKeyExchangeService");
            ChannelFactory<IKeyExchange> DHconnection = new ChannelFactory<IKeyExchange>("DiffeHellmanKeyExchangeService");
            IKeyExchange RSAKeyExchangeChannel = RSAconnection.CreateChannel();
            IKeyExchange DiffeHellmanKeyExchangeChannel = DHconnection.CreateChannel();
            try
            {
                RSAEncryption.Encrypt(RSAKeyExchangeChannel);
                DiffeHellmanEncryption.Encrypt(DiffeHellmanKeyExchangeChannel);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("To exit press enter.");
            Console.ReadLine();
        }
    }
}
