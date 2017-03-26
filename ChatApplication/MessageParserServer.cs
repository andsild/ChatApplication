using System;
using System.IO;
using Utilities;

namespace ChatApplication
{
    public class MessageParserServer : IMessageParser
    {
        private readonly string username;

        /// <summary>
        /// A <see cref="Message"/>parser that can forward parsed messages
        /// </summary>
        /// <param name="username"></param>
        public MessageParserServer(string username) 
        {
            this.username = username;
        }

        public static void CreateServerDirectory()
        {
            if(Directory.Exists(Configuration.FIFO_FOLDER))
            {
                Directory.Delete(Configuration.FIFO_FOLDER, true);
            }
            Directory.CreateDirectory(Configuration.FIFO_FOLDER);
        }

        public void ParseReceivedMessage(string text)
        {
            Message message = null;
            try
            {
                message = Message.ParseReceivedMessage(text);
            }
            catch(ArgumentException ae)
            {
                /** shh! */
                return;
            }

            if(message.Recipient != username)
            {
                throw new InvalidOperationException("Recieved stray message ::" + message);
            }

            // To make the code cleaner this would be delegated to an interface
            foreach (string fileNameWithPath in Directory.EnumerateFiles(Utilities.Configuration.FIFO_FOLDER))
            {
                if (fileNameWithPath == Path.Combine(Configuration.FIFO_FOLDER, message.Sender) || fileNameWithPath == Utilities.Configuration.BROADCAST_CHANNEL)
                {
                    continue;
                }

                using (StreamWriter sw = File.AppendText(fileNameWithPath))
                {
                    sw.WriteLine(message);
                }
            }
        }
    }
}
