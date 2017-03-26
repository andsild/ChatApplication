namespace ChatApplication
{
    public interface IChatApplication
    {
        void AppendParsedMessageToChat(string username, Message message);
        void AddChatErrorLine(string username, string errorMessage);
    }
}
