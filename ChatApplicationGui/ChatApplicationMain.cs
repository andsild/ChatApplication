using ChatApplication;
using System;
using System.Collections.Generic;
using Utilities;
using System.Diagnostics;
using ChatApplicationGui;

namespace ChatApplicationClientSide
{
    using ChatInstance = Tuple<IChatUi, ChatChannel>;

    public class ChatApplicationMain : IChatApplication, IDisposable
    {
        private readonly DirectoryMonitor directoryMonitor;
        private readonly IDictionary<string, ChatInstance> chatClients = new Dictionary<string, ChatInstance>();
        private readonly ChatChannel serverChannel;

        /// <summary>
        /// The core logic for the application. Maintains a list of users and their interfaces and a/the server
        /// </summary>
        public ChatApplicationMain()
        {
            MessageParserServer.CreateServerDirectory();
            directoryMonitor = new DirectoryMonitor(Configuration.FIFO_FOLDER);
            var messageParserServer = new MessageParserServer(Configuration.BROADCAST_CHANNELNAME);
            serverChannel = new ChatChannel(Configuration.BROADCAST_CHANNELNAME, messageParserServer);
        }

        /// <summary>
        /// Given a username, get its user interface
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private IChatUi getChatClient(string username)
        {
            if (chatClients.TryGetValue(username, out ChatInstance chatUiAndChannel))
            {
                IChatUi chatUi = chatUiAndChannel.Item1;
                Debug.Assert(username == chatUi.Username);
                return chatUi;
            }
            throw new InvalidOperationException($"Username {username} not previously registered");
        }

        /// <summary>
        /// Forget a user
        /// </summary>
        /// <param name="username"></param>
        public void RemoveUserFromChat(string username)
        {
            if (chatClients.TryGetValue(username, out ChatInstance chatUiAndChannel))
            {
                directoryMonitor.RemoveChatUi(chatUiAndChannel.Item1);
                chatUiAndChannel.Item1.Dispose();
                chatUiAndChannel.Item2.Dispose();
                chatClients.Remove(username);
            }
        }

        public void AddUserToChat(string username)
        {
            var chatClient = new MessageParserClient(username, this);
            var chatForm = new ChatForm(this, directoryMonitor.GetUsers(), username);
            var clientMessageParser = new MessageParserClient(username, this);
            var chatChannel = new ChatChannel(username, clientMessageParser);
            chatClients.Add(username, new Tuple<IChatUi, ChatChannel>(chatForm, chatChannel));
            directoryMonitor.AddChatUi(chatForm);
        }

        public void AppendParsedMessageToChat(string username, Message message)
        {
            IChatUi chatClient = getChatClient(username);
            chatClient.AddChatLine(message);
        }

        public void SendMessage(IChatUi chatClient, string recipient, string messageText)
        {
            try
            {
                var message = new Message(chatClient.Username, recipient, messageText);
                Configuration.AppendToFile(recipient, message.ToString());
                chatClient.AddChatLine(message);
            }
            catch (ArgumentException ae)
            {
                chatClient.AddChatErrorLine($"Message could not be sent :: {ae.Message}");
            }
        }

        public void AddChatErrorLine(string username, string errorMessage)
        {
            IChatUi chatClient = getChatClient(username);
            chatClient.AddChatErrorLine(errorMessage);
        }

        public void Dispose()
        {
            directoryMonitor.Dispose();
            foreach(var chatChannelAndUi in chatClients.Values)
            {
                chatChannelAndUi.Item1.Dispose();
                chatChannelAndUi.Item2.Dispose();
            }
            serverChannel.Dispose();
        }
    }
}
