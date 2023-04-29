using RabbitMQ.Client;

namespace DataCaptureService
{
    internal class Program
    {
        const int ReadFileRetryAttempts = 10;

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("pdfExchange", ExchangeType.Direct, true);

            var watchFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles");
            using var watcher = new FileSystemWatcher(watchFolder, "*.pdf");
            watcher.EnableRaisingEvents = true;
            watcher.Created += (sender, e) =>
            {
                try
                {
                    var readTask = Task.Factory.StartNew(() =>
                    {
                        using var stream = WaitForFile(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer);

                        return buffer;
                    });

                    readTask.ContinueWith(task =>
                    {
                        channel.BasicPublish(exchange: "pdfExchange", routingKey: "pdf.created", null, task.Result);
                        Console.WriteLine("Sent document {0} to processing service", e.Name);
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error processing document {0}: {1}", e.Name, ex.Message);
                }
            };

            Console.ReadLine();

            channel.Close();
            connection.Close();
        }

        static FileStream WaitForFile(string fullPath, FileMode mode, FileAccess access, FileShare share)
        {
            for (int numTries = 0; numTries < ReadFileRetryAttempts; numTries++)
            {
                FileStream? stream = null;
                try
                {
                    stream = new FileStream(fullPath, mode, access, share);
                    return stream;
                }
                catch (IOException)
                {
                    stream?.Dispose();
                    Thread.Sleep(50);
                }
            }

            return null;
        }
    }
}
