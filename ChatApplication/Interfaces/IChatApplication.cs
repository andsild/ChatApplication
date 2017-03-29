namespace ChatApplication
{
    public interface IChatApplication
    {
        void AppendParsedMessageToChat(string username, Message message);
        void AppendErrorMessageToChat(string username, string errorMessage);
    }
}
