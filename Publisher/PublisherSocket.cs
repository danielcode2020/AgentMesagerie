using System.Net;
using System.Net.Sockets;

namespace Publisher
{
    class PublisherSocket
    {
        private Socket socket;

        public bool isConnected;

        public PublisherSocket()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void connect(string ipAddress, int port)
        {
            // BeginConnect e async
            socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAddress), port), connectedCallback, null);
            Thread.Sleep(2000);
        }

        // verificam daca sa efectuat conexiunea
        private void connectedCallback(IAsyncResult asyncResult)
        {
            if (socket.Connected)
            {
                Console.WriteLine("Sender connected to Broker.");
            }
            else
            {
                Console.WriteLine("Error: Sender not connected to Broker");
            }

            isConnected = socket.Connected;
        }

        public void send(byte[] data)
        {
            try
            {
                socket.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
