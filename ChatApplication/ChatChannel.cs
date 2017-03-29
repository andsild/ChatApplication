using System;
using System.IO;
using System.Threading;

namespace ChatApplication
{
    public sealed class ChatChannel : IDisposable
    {
        private readonly Thread thread;
        /// <summary>
        /// Pathname to a FirstInFirstOut file which is read continously
        /// </summary>
        private readonly string fifoPath;
        private volatile bool continueMonitoringFile = true;
        private readonly IMessageParser messageParser;

        /// <summary>
        /// A class for creating and monitoring a channel (i.e. file) for chatting.
        /// A separate thread is created to read the tail of a file and parse that output further to a parser
        /// </summary>
        /// <param name="username"></param>
        /// <param name="messageParser"></param>
        public ChatChannel(string fifoPath, IMessageParser messageParser)
        {
            this.fifoPath = fifoPath;
            this.messageParser = messageParser;

            ThreadStart threadStart = MonitorTailOfFile;
            thread = new Thread(threadStart);
            thread.Start();
        }

        public void Dispose()
        {
            continueMonitoringFile = false;
            thread.Join();
            File.Delete(fifoPath);
        }

        public void ParseTextToMessage(string text)
        {
            Message message = null;
            try
            {
                message = Message.ParseTextToMessage(text);
            }
            catch (ArgumentException ae)
            {
                messageParser.OnMessageParserError(ae);
                return;
            }

            messageParser.OnMessageParseSuccess(message);
        }

        private void MonitorTailOfFile()
        {
            var message = string.Empty;

            // fs is disposed when sr goes out of scope
            FileStream fs = new FileStream(fifoPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (continueMonitoringFile)
                    {
                        while (!sr.EndOfStream && continueMonitoringFile)
                        {
                            message = sr.ReadLine();
                            if (!string.IsNullOrEmpty(message))
                            {
                                ParseTextToMessage(message);
                            }
                        }
                        while (sr.EndOfStream && continueMonitoringFile)
                        {
                            Thread.Sleep(100);
                        }
                        message = sr.ReadLine();
                        if (!string.IsNullOrEmpty(message))
                        {
                            ParseTextToMessage(message);
                        }
                    }
                }
            }
        }
    }
}
