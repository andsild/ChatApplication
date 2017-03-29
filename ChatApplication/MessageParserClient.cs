using System;
using Utilities;

namespace ChatApplication
{
    public class MessageParserClient : IMessageParser
    {
        private readonly string username;
        private readonly IChatUi chatUi;
        /// <summary>
        /// A <see cref="Message"/>parser that can read messages and update user interfaces
        /// </summary>
        /// <param name="username"></param>
        /// <param name="chatApplication"></param>
        public MessageParserClient(string username, IChatUi chatUi) 
        {
            this.username = username;
            this.chatUi = chatUi;
        }

        public void OnMessageParserError(Exception e)
        {
            if(e is ArgumentException)
            {
                chatUi.AddChatErrorLine($@"Command failed :: {e.Message}");
            }
        }
        public void OnMessageParseSuccess(Message message)
        {
            if (message.Recipient == username || message.Recipient == Configuration.BROADCAST_CHANNELNAME)
            {
                chatUi.AppendMessageToChat(message);
            }
            else
            {
                chatUi.AddChatErrorLine("Received stray message ::" + message);
            }
        }
    }
}
