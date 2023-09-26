using Common;
using System.Net;
using System.Net.Sockets;


// accept in fiecare metoda ca sa nu blocam conexiunea
// fiecare prelucrare se face pe thread aparte

namespace Broker
{
    class BrokerSocket
    {
        private Socket socket;

        private const int CONNECTIONS_LIMIT = 8;

        public BrokerSocket()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void start(string ip, int port)
        {
            socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            socket.Listen(CONNECTIONS_LIMIT);
            accept();
        }

        private void accept()
        {
            socket.BeginAccept(acceptedCallback, null);
        }

        private void acceptedCallback(IAsyncResult asyncResult)
        {
            ConnectionInfo connection = new ConnectionInfo();

            try
            {
                connection.socket = socket.EndAccept(asyncResult);
                connection.address = connection.socket.RemoteEndPoint.ToString();
                connection.socket.BeginReceive(connection.data, 0, connection.data.Length, SocketFlags.None,
                    receiveCallBack, connection);
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString()); 
            }
            finally
            {
                accept();
            }
        }

        private void receiveCallBack(IAsyncResult asyncResult)
        {
            ConnectionInfo connection = asyncResult.AsyncState as ConnectionInfo;
            try
            {
                Socket senderSocket = connection.socket;
                SocketError response;
                int buffSize = senderSocket.EndReceive(asyncResult, out response);

                if (response == SocketError.Success)
                {
                    byte[] payload = new byte[buffSize];
                    Array.Copy(connection.data, payload, payload.Length);

                    // handluim payloadul
                    PayloadHandler.handle(payload, connection);
                }
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString); 
            }
            finally 
            {
                try
                {
                    // incercam sa citim date noi
                    connection.socket.BeginReceive(connection.data, 0, connection.data.Length,
                        SocketFlags.None, receiveCallBack, connection);
                }
                catch (Exception e) // ne deconectam de la socket
                { 
                    Console.WriteLine(e.ToString());
                    var address = connection.socket.RemoteEndPoint.ToString();

                    // stergem din storage
                    ConnectionsStorages.remove(address);

                    // mai intai remove pentru ca socketul e inchis dar el trimite
                    // si sa nu primim exceptie
                    connection.socket.Close();
                }
            }
        }
    }
}
