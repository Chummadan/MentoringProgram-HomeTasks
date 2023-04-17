namespace Server
{
    class MessageGenerator
    {
        public static string GenerateUserMessage(string clientName, string message)
            => $"{clientName}: {message}";

        public static string GenerateWelcomeMessage(string clientName)
            => $"{clientName} joined the club =)";
    }
}
