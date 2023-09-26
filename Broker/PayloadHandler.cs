

using Common;
using Newtonsoft.Json;
using System.Text;

namespace Broker
{
    class PayloadHandler
    {
        public static void handle(byte[] payloadBytes, ConnectionInfo connectionInfo)
        {
            var payloadString = Encoding.UTF8.GetString(payloadBytes);
            Console.WriteLine(payloadString);

            if (payloadString.StartsWith("subscribe#"))
            {
                connectionInfo.topic = payloadString.Split("subscribe#").LastOrDefault();
                // adaugam conexiunea in storage
                ConnectionsStorages.add(connectionInfo);
            }
            else
            {
                Payload payload = JsonConvert.DeserializeObject<Payload>(payloadString);
                // adaugam in storage
                PayloadStorage.add(payload);
            }
        }
    }
}
