using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Announcer
    {
        public static void Broadcast(string clientName, Dictionary<string, TcpClient> clientsList)
        => BroadcastMessage(GetMessageBytes(MessageGenerator.GenerateWelcomeMessage(clientName)), clientsList);

        public static void Broadcast(string clientName, string message, Dictionary<string, TcpClient> clientsList)
            => BroadcastMessage(GetMessageBytes(MessageGenerator.GenerateUserMessage(clientName, message)), clientsList);

        static byte[] GetMessageBytes(string message)
            => Encoding.ASCII.GetBytes(message + Environment.NewLine);

        static void BroadcastMessage(byte[] buffer, Dictionary<string, TcpClient> clientsList)
        {
            foreach (TcpClient client in clientsList.Values)
                client.GetStream().Write(buffer, 0, buffer.Length);
        }
    }
}
