using Common;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Subscriber
{
    class SubscriberSocket
    {
        private Socket socket;
        private string topic;

        public SubscriberSocket(string topic)
        {
            this.topic = topic.ToLower();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void connect(string ipAddress, int port)
        {
            socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAddress), port), connectedCallback, null);
            Console.WriteLine("Waiting for a connection");
        }

        private void connectedCallback(IAsyncResult asyncResult) 
        {
            if (socket.Connected)
            {
                Console.WriteLine("Subscriber connected to broker");
                subcribe();
                startReceive();
            }
            else
            {
                Console.WriteLine("Error: Subscriber could not connect to broker");
            }
        }

        private void subcribe()
        {
            var data = Encoding.UTF8.GetBytes("subscribe#" + topic);
            send(data);
            Console.WriteLine("got here");
        }

        private void send(byte[] data)
        {
            try
            {
                socket.Send(data);
                Console.WriteLine("sent data");
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
            }

        }

        private void startReceive()
        {
            ConnectionInfo connection = new ConnectionInfo();
            connection.socket = socket;
            socket.BeginReceive(connection.data, 0, connection.data.Length, SocketFlags.None, 
                receiveCallback, connection);
        }

        private void receiveCallback(IAsyncResult asyncResult) 
        {
            ConnectionInfo connectionInfo = asyncResult.AsyncState as ConnectionInfo;
            try
            {
                SocketError response;
                int buffSize = socket.EndReceive(asyncResult, out response);

                if (response == SocketError.Success)
                {
                    byte[] payloadBytes = new byte[buffSize];
                    Array.Copy(connectionInfo.data, payloadBytes, payloadBytes.Length);
                    PayloadHandler.handle(payloadBytes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Cant receive data from broker .{e.Message}");
            }
            finally
            {
                try
                {
                    connectionInfo.socket.BeginReceive(connectionInfo.data, 0, connectionInfo.data.Length, SocketFlags.None,
                    receiveCallback, connectionInfo);

                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                    connectionInfo.socket.Close();
                }
            }
        }
    }
}
