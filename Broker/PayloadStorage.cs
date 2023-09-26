using Common;
using System.Collections.Concurrent;

namespace Broker
{
    static class PayloadStorage
    {
        private static ConcurrentQueue<Payload> payloadQueue;

        static PayloadStorage()
        {
            payloadQueue = new ConcurrentQueue<Payload>();
        }

        public static void add(Payload payload)
        {
            payloadQueue.Enqueue(payload);
        }

        public static Payload getNext()
        {
            Payload payload = null;
            payloadQueue.TryDequeue(out payload);
            return payload;
        }

        public static bool isEmpty()
        {
            return payloadQueue.IsEmpty;
        }

    }
}
