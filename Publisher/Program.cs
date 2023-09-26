// See https://aka.ms/new-console-template for more information
using Common;
using Newtonsoft.Json;
using System.Text;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Publisher");

            var publisherSocket = new PublisherSocket();
            publisherSocket.connect(Settings.BROKER_IP, Settings.BROKER_PORT);

            if (publisherSocket.isConnected)
            {
                while (true)
                {
                    var payload = new Payload();

                    Console.Write("Enter the topic :");
                    payload.topic = Console.ReadLine().ToLower();

                    Console.Write("Enter the message :");
                    payload.message = Console.ReadLine().ToLower();

                    // serializam obiectul in bytes

                    var payloadString = JsonConvert.SerializeObject(payload);

                    byte[] data = Encoding.UTF8.GetBytes(payloadString);

                    publisherSocket.send(data);


                }
            }
            Console.ReadLine();
        }
    }
}
