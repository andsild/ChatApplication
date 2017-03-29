using ChatApplication;
using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ChatApplicationGui
{
    static class Program
    {
        private class FormRepository : IChatFormRepository
        {
            public IChatUi NewInterface(ChatApplicationMain main, IEnumerable<string> users, string username)
            {
                return new ChatForm(main, users, username);
            }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var chatApplicationMain = new ChatApplicationMain(new FormRepository());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm(chatApplicationMain));
        }
    }
}
