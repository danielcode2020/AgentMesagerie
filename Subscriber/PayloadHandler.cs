using Common;
using Newtonsoft.Json;
using System.Text;

namespace Subscriber
{
    class PayloadHandler
    {
        public static void handle(byte[] payloadBytes)
        {
            var payloadString = Encoding.UTF8.GetString(payloadBytes);
            var payload = JsonConvert.DeserializeObject<Payload>(payloadString);

            Console.WriteLine(payload.message);
        }
    }
}
