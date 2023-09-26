using Broker;
using Common;

Console.WriteLine("Broker");

BrokerSocket socket = new BrokerSocket();
socket.start(Settings.BROKER_IP, Settings.BROKER_PORT);

// cu thread running facem un thread nou
// threadpool pentru taskuri scurte

var worker = new Worker();
Task.Factory.StartNew(worker.doSendMessageWork, TaskCreationOptions.LongRunning);

Console.ReadLine();