using ChatApplicationClientSide;
using System;
using System.Windows.Forms;

namespace ChatApplicationGui
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var chatApplicationMain = new ChatApplicationMain();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm(chatApplicationMain));
        }
    }
}
