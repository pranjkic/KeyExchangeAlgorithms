using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost RSAHost = new ServiceHost(typeof(RSAKeyExchange)))
            {
                using (ServiceHost DiffeHellmanHost = new ServiceHost(typeof(DiffeHellmanKeyExchange)))
                {
                    RSAHost.Open();
                    DiffeHellmanHost.Open();
                    Console.WriteLine("Service is ready.");
                    Console.ReadKey();
                    RSAHost.Close();
                    DiffeHellmanHost.Close();
                }
            }
        }
    }
}
