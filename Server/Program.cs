﻿using System;
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
            using (ServiceHost host = new ServiceHost(typeof(KeyExchange)))
            {
                host.Open();
                Console.WriteLine("Service is ready.");
                Console.ReadKey();
                host.Close();
            }
        }
    }
}
