using System;
using System.ComponentModel;
using System.Windows.Forms;
using Utilities;

namespace ChatApplicationGui
{
    public partial class LoginForm : Form
    {
        private readonly ChatApplicationClientSide.ChatApplicationMain chatApplication;
        public LoginForm(ChatApplicationClientSide.ChatApplicationMain chatApplication)
        {
            this.chatApplication = chatApplication;
            InitializeComponent();
            txtUsername.KeyDown += textBox1_KeyDown;
            this.FormClosing += LoginForm_Closing;
        }

        private void tryToLogin()
        {
            if(!Configuration.IsValidFilename(txtUsername.Text) || !Configuration.UsernameIsTaken(txtUsername.Text))
            {
                txtUsername.Clear();
                invalidUsernameLabel.Show();
                return;
            }

            chatApplication.AddUserToChat(txtUsername.Text);

            txtUsername.Clear();
            invalidUsernameLabel.Hide();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            tryToLogin();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtUsername.Clear();
        }

        private void LoginForm_Closing(object sender, CancelEventArgs e)
        {
            chatApplication.Dispose();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            tryToLogin();
        }
    }
}
