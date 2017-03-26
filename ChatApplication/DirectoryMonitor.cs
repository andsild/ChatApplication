using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities;

namespace ChatApplication
{
    public class DirectoryMonitor : IDisposable
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
        public DirectoryMonitor(string path)
        {
            fileSystemWatcher.Path = Configuration.FIFO_FOLDER;
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime;

            fileSystemWatcher.Filter = "*.*";
            fileSystemWatcher.Deleted += new FileSystemEventHandler(DirectoryChanged);
            fileSystemWatcher.Created += new FileSystemEventHandler(DirectoryChanged);
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        public IEnumerable<string> GetUsers()
        {
            return users.AsEnumerable();
        }

        public void AddChatUi(IChatUi chatInterface)
        {
            chatUiList.Add(chatInterface);
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
                IChatUi disposedUserListManager = null;
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
                        disposedUserListManager = userListManager;
                    }
                }
                chatUiList.Remove(disposedUserListManager);
            }
        }

        public void Dispose()
        {
            fileSystemWatcher.Dispose();
        }
    }
}
