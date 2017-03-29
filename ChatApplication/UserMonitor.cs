using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Utilities;

namespace ChatApplication
{
    public sealed class UserMonitor : IDisposable
    {
        private readonly FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
        /// <summary>
        /// List of User Interfaces that has lists of users that can be updated
        /// </summary>
        private readonly IList<IChatUi> chatUiList = new List<IChatUi>();
        /// <summary>
        ///We maintain a list of users in addition to <see cref="ChatApplicationGui"/> in case someone has created a user from outside the program.
        /// </summary>
        private readonly ISet<string> users = new HashSet<string>();

        /// <summary>
        /// Continously monitor a directory for changes, remembering each file created and deleted as "users"
        /// </summary>
        /// <param name="path"></param>
        public UserMonitor(string path)
        {
            fileSystemWatcher.Path = Configuration.FIFO_FOLDER;
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime;

            fileSystemWatcher.Filter = "*.*";
            fileSystemWatcher.Deleted += new FileSystemEventHandler(DirectoryChanged);
            fileSystemWatcher.Created += new FileSystemEventHandler(DirectoryChanged);
            fileSystemWatcher.EnableRaisingEvents = true;

            var fileStream = File.Create(Configuration.BROADCAST_CHANNEL);
            fileStream.Dispose(); // release process handle
        }

        public IEnumerable<string> GetSignedInUsers()
        {
            return users.AsEnumerable();
        }

        public string AddChatUi(IChatUi chatInterface)
        {
            var fifoPath = Path.Combine(Utilities.Configuration.FIFO_FOLDER, chatInterface.Username);

            var fileStream = File.Create(fifoPath);
            fileStream.Dispose(); // release process handle

            Thread.Sleep(100); // give time for DirectoryChanged to fire
                               // ideally we would have some way to prevent directorychanged to fire
            chatUiList.Add(chatInterface);

            return fifoPath;
        }

        public void RemoveChatUi(IChatUi chatInterface)
        {
            chatUiList.Remove(chatInterface);
        }

        private void DirectoryChanged(object source, FileSystemEventArgs e)
        {
            /** New user entered chat */
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                users.Add(e.Name);
                // Need to call ToList since we modify each element in-place
                foreach (var chatUi in chatUiList.ToList())
                {
                    chatUi.AddUserToList(e.Name);
                }
            }

            /** User left chat */
            else if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                IChatUi disposedChatUi = null;
                // Need to call ToList since we modify each element in-place
                foreach (var userListManager in chatUiList.ToList())
                {
                    users.Remove(e.Name);
                    if (userListManager.Username != e.Name)
                    {
                        userListManager.RemoveUserFromList(e.Name);
                    }
                    else
                    {
                        disposedChatUi = userListManager;
                    }
                }
                chatUiList.Remove(disposedChatUi);
            }
        }

        public void Dispose()
        {
            fileSystemWatcher.Dispose();
        }
    }
}
