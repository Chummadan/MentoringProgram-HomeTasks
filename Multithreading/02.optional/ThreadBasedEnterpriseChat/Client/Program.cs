﻿using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var ip = IPAddress.Parse("127.0.0.1");
        var port = 5000;
        var client = new TcpClient();
        client.Connect(ip, port);
        Console.WriteLine("client connected!!");
        var ns = client.GetStream();
        var thread = new Thread(o => ReceiveData((TcpClient)o));

        thread.Start(client);

        string s;
        while (!string.IsNullOrEmpty((s = Console.ReadLine())))
        {
            byte[] buffer = Encoding.ASCII.GetBytes(s);
            ns.Write(buffer, 0, buffer.Length);
        }

        client.Client.Shutdown(SocketShutdown.Send);
        thread.Join();
        ns.Close();
        client.Close();
        Console.WriteLine("disconnect from server!!");
        Console.ReadKey();
    }

    static void ReceiveData(TcpClient client)
    {
        var ns = client.GetStream();
        var receivedBytes = new byte[1024];
        int byte_count;

        while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            Console.Write(Encoding.ASCII.GetString(receivedBytes, 0, byte_count));
    }
}