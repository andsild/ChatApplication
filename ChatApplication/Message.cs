using System;
using System.Linq;
using Utilities;

namespace ChatApplication
{
    public class Message
    {
        public readonly string MessageText;
        public readonly string Sender;
        public readonly string Recipient;

        /// <summary>
        /// Textformat used to communicate between users/channels
        /// </summary>
        /// <param name="sender">Who sent the message</param>
        /// <param name="recipient">Who should see/receive the message</param>
        /// <param name="messageText">The actual message text</param>
        public Message(string sender, string recipient, string messageText)
        {
            ValidateUsername(sender);
            ValidateUsername(recipient);
            ValidateMessageText(messageText);

            this.MessageText = messageText;
            this.Sender = sender;
            this.Recipient = recipient;
        }

        public static Message ParseReceivedMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("empty string");
            }

            var inputTextAsList = message.Split();
            if(inputTextAsList.Length < 3)
            {
                throw new ArgumentException("Invalid message format: " + message);
            }

            var sender = inputTextAsList.First();
            var recipient = inputTextAsList.Skip(1).First();
            return new Message(sender, recipient, string.Join(" ", inputTextAsList.Skip(2)));
        }

        public static void ValidateUsername(string username)
        {
            if(!Configuration.IsValidFilename(username))
            {
                throw new ArgumentException(@"\\Invalid username " + username);
            }
        }

        public static void ValidateMessageText(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("empty string");
            }
        }


        public override string ToString()
        {
            return $"{Sender} {Recipient} {MessageText}";
        }
    }
}
