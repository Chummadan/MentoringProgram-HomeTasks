using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProcessingService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare("pdfQueue", true, false, true);
            channel.QueueBind("pdfQueue", "pdfExchange", "pdf.created");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var fileName = Guid.NewGuid().ToString() + ".pdf";
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Results", fileName);
                File.WriteAllBytes(filePath, eventArgs.Body.ToArray());

                Console.WriteLine("Received and saved document {0} from data capture service", fileName);
            };
            channel.BasicConsume("pdfQueue", true, consumer);

            Console.ReadLine();

            channel.Close();
            connection.Close();
        }
    }
}
