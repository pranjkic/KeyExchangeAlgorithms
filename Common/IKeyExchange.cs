using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IKeyExchange
    {
        [OperationContract]
        byte[] GetPublicKey();

        [OperationContract]
        void Receive(byte[] iv, byte[] encryptedSessionKey, byte[] encryptedMessage);
    }
}
