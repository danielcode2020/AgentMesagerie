using Common;

namespace Broker
{
    static class ConnectionsStorages
    {
        private static List<ConnectionInfo> connections;
        private static object locker;

        static ConnectionsStorages()
        {
            connections = new List<ConnectionInfo>();
            locker = new object();
        }

        // doar un fir de executie executa acest cod
        public static void add(ConnectionInfo connectionInfo)
        {
            lock (locker)
            {
                connections.Add(connectionInfo);
            }
        }

        public static void remove(string address)
        {
            lock (locker)
            {
                connections.RemoveAll( x => x.address == address);
            }
        }

        public static List<ConnectionInfo> getConnectionsByTopic(String topic)
        {
            List<ConnectionInfo> selectedConnections;
            lock (locker) 
            {
                selectedConnections = connections.Where(x =>  x.topic == topic).ToList();
            }
            return selectedConnections;
        }
    }
}
