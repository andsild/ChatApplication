using System;

namespace ChatApplication
{
    public interface IChatUi : IDisposable
    {
       void AddUserToList(string username);
       void RemoveUserFromList(string username);
       void AddChatErrorLine(string message);
       void AppendMessageToChat(Message message);
       void Show();
       string Username { get; }
    }
}