using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class DiffeHellmanKey
    {
        public static ECDiffieHellmanCng key = new ECDiffieHellmanCng()
        {
            KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash,
            HashAlgorithm = CngAlgorithm.Sha256
        };
    }
}
