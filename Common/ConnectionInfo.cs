
using System.Net.Sockets;

namespace Common
{
    public class ConnectionInfo
    {

        public const int BUFF_SIZE = 1024;

        public byte[] data {  get; set; }
        public Socket socket {get;set;}
        public string address { get;set;}
        public string topic { get;set;}

        public ConnectionInfo()
        {
            data = new byte[BUFF_SIZE];
        }
    }
}
