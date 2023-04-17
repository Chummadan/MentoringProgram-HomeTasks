namespace Server
{
    class MessageStore
    {
        readonly int maxMessages;
        Queue<string> messageQueue;

        public MessageStore(int maxMessages)
        {
            this.maxMessages = maxMessages;
            messageQueue = new();
        }

        public void AddMessage(string message)
        {
            messageQueue.Enqueue(message);

            while (messageQueue.Count > maxMessages)
                messageQueue.Dequeue();
        }

        public List<string> GetLastMessages()
            => messageQueue.ToList();
    }
}
