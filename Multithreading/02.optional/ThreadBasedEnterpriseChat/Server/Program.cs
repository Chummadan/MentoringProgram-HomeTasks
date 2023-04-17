using Server;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    const int maxMessages = 25;
    static readonly object _lock = new();
    static readonly Dictionary<string, TcpClient> clientsList = new();

    static void Main(string[] args)
    {
        var messageStore = new MessageStore(maxMessages);
        var ServerSocket = new TcpListener(IPAddress.Any, 5000);
        ServerSocket.Start();

        while (true)
        {
            var client = ServerSocket.AcceptTcpClient();
            var clientName = GetMessage(client);

            lock (_lock) clientsList.Add(clientName, client);
            lock (_lock) Announcer.Broadcast(clientName, clientsList);
            Console.WriteLine($"{clientName} connected!");

            var t = new Thread(Handle_clients);
            t.Start(clientName);
        }
    }

    public static void Handle_clients(object parameter)
    {
        var clientName = (string)parameter;
        TcpClient client;

        lock (_lock) client = clientsList[clientName];

        while (true)
        {
            var data = GetMessage(client);

            if (string.IsNullOrEmpty(data))
                break;

            lock (_lock) Announcer.Broadcast(clientName, data, clientsList);
            Console.WriteLine($"{clientName} send: {data}");
        }

        lock (_lock) clientsList.Remove(clientName);
        client.Client.Shutdown(SocketShutdown.Both);
        client.Close();
    }

    public static string GetMessage(TcpClient client)
    {
        var buffer = new byte[1024];
        var byte_data = client.GetStream().Read(buffer, 0, buffer.Length);
        return Encoding.ASCII.GetString(buffer, 0, byte_data);
    }
}