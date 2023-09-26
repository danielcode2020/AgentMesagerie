using Newtonsoft.Json;
using System.Text;

namespace Broker
{
     class Worker
    {

        private const int TIME_TO_SLEEP = 500;
        public void doSendMessageWork()
        {
            while (true)
            {
                while (!PayloadStorage.isEmpty())
                {
                    var payload = PayloadStorage.getNext();

                    if (payload != null) 
                    {
                        var connections = ConnectionsStorages.getConnectionsByTopic(payload.topic);
                        foreach(var connection in connections ) 
                        {
                            var payloadString = JsonConvert.SerializeObject(payload);
                            byte[] data = Encoding.UTF8.GetBytes(payloadString);

                            connection.socket.Send(data);
                        }
                    }
                }
                Thread.Sleep(TIME_TO_SLEEP);
            }
        }
    }
}
