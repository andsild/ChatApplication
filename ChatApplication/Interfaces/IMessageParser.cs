namespace ChatApplication
{
    public interface IMessageParser
    {
        void ParseReceivedMessage(string text);
    }
}