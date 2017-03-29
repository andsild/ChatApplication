using System.Collections.Generic;

namespace ChatApplication
{
    public interface IChatFormRepository
    {
       IChatUi NewInterface(ChatApplicationMain main, IEnumerable<string> users, string username);
    }
}
