using ChatApplication;
using System;
using System.Collections.Generic;
using Utilities;
using System.Diagnostics;

namespace ChatApplication
{
    using ChatInstance = Tuple<IChatUi, ChatChannel>;

    public sealed class ChatApplicationMain : IChatApplication, IDisposable
    {
        private readonly UserMonitor directoryMonitor;
        private readonly IDictionary<string, ChatInstance> chatClients = new Dictionary<string, ChatInstance>();
        private readonly ChatChannel serverChannel;
        private readonly IChatFormRepository formRepository;

        /// <summary>
        /// The core logic for the application. Maintains a list of users and their interfaces and a/the server
        /// </summary>
        public ChatApplicationMain(IChatFormRepository formRepository)
        {
            this.formRepository = formRepository;

            MessageParserServer.CreateServerDirectory();
            directoryMonitor = new UserMonitor(Configuration.FIFO_FOLDER);
            var messageParserServer = new MessageParserServer(Configuration.BROADCAST_CHANNELNAME);
            serverChannel = new ChatChannel(Configuration.BROADCAST_CHANNEL, messageParserServer);
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
            var chatForm = formRepository.NewInterface(this, directoryMonitor.GetSignedInUsers(), username);
            var fifoPath = directoryMonitor.AddChatUi(chatForm);
            var clientMessageParser = new MessageParserClient(username, chatForm);
            var chatChannel = new ChatChannel(fifoPath, clientMessageParser);

            chatClients.Add(username, new ChatInstance(chatForm, chatChannel));
        }

        public void AppendParsedMessageToChat(string username, Message message)
        {
            IChatUi chatClient = getChatClient(username);
            chatClient.AppendMessageToChat(message);
        }

        public void SendMessage(string username, string recipient, string messageText)
        {
            var chatClient = getChatClient(username);

            try
            {
                var message = new Message(chatClient.Username, recipient, messageText);
                Configuration.SendMessageToUser(recipient, message.ToString());
                chatClient.AppendMessageToChat(message);
            }
            catch (ArgumentException ae)
            {
                chatClient.AddChatErrorLine($"Message could not be sent :: {ae.Message}");
            }
        }

        public void AppendErrorMessageToChat(string username, string errorMessage)
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
