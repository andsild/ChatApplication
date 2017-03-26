using System;
using Utilities;

namespace ChatApplication
{
    public class MessageParserClient : IMessageParser
    {
        private readonly string username;
        private readonly IChatApplication chatApplication;
        /// <summary>
        /// A <see cref="Message"/>parser that can read messages and update user interfaces
        /// </summary>
        /// <param name="username"></param>
        /// <param name="chatApplication"></param>
        public MessageParserClient(string username, IChatApplication chatApplication) 
        {
            this.username = username;
            this.chatApplication = chatApplication;
        }

        public void ParseReceivedMessage(string text)
        {
            Message message = null;
            try
            {
                message = Message.ParseReceivedMessage(text);
            }
            catch (ArgumentException ae)
            {
                chatApplication.AddChatErrorLine(username, $@"Command failed :: {ae.Message}");
                return;
            }

            if (message.Recipient == username || message.Recipient == Configuration.BROADCAST_CHANNELNAME)
            {
                chatApplication.AppendParsedMessageToChat(username, message);
            }
            else
            {
                chatApplication.AddChatErrorLine(username, "Received stray message ::" + message);
            }
        }
    }
}
