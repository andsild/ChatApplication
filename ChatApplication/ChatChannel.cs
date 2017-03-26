using System;
using System.IO;
using System.Threading;

namespace ChatApplication
{
    public class ChatChannel : IDisposable
    {
        protected readonly Thread thread;
        /// <summary>
        /// Pathname to a FirstInFirstOut file which is read continously
        /// </summary>
        protected readonly string fifoPath;
        public readonly string Username;
        protected volatile bool continueMonitoringFile = true;
        protected readonly IMessageParser messageParser;

        /// <summary>
        /// A class for creating and monitoring a channel (i.e. file) for chatting.
        /// A separate thread is created to read the tail of a file and parse that output further to a parser
        /// </summary>
        /// <param name="username"></param>
        /// <param name="messageParser"></param>
        public ChatChannel(string username, IMessageParser messageParser)
        {
            this.messageParser = messageParser;
            fifoPath = Path.Combine(Utilities.Configuration.FIFO_FOLDER, username);
            Username = username;

            var fileStream = File.Create(fifoPath);
            fileStream.Dispose(); // release process handle

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

        protected void MonitorTailOfFile()
        {
            var message = string.Empty;

            using (FileStream fs = new FileStream(fifoPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                                messageParser.ParseReceivedMessage(message);
                            }
                        }
                        while (sr.EndOfStream && continueMonitoringFile)
                        {
                            Thread.Sleep(100);
                        }
                        message = sr.ReadLine();
                        if (!string.IsNullOrEmpty(message))
                        {
                            messageParser.ParseReceivedMessage(message);
                        }
                    }
                }
            }
        }
    }
}
