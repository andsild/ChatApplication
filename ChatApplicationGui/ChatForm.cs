using ChatApplication;
using ChatApplicationClientSide;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Utilities;

namespace ChatApplicationGui
{
    public partial class ChatForm : Form, IChatUi
    {
        private readonly ChatApplicationMain chatApplication;

        private readonly string username;
        public string Username { get { return this.username; } }

        public ChatForm(ChatApplicationMain chatApplication, IEnumerable<string> users, string username)
        {
            Show();

            this.username = username;
            this.chatApplication = chatApplication;
            InitializeComponent();
            txtInput.KeyDown += txtInput_KeyDown;

            foreach (var user in users)
            {
                if (user == username)
                {
                    continue;
                }
                AddUserToList(user);
            }

            txtRecipient.Text = Configuration.BROADCAST_CHANNELNAME;
            Text += ": " + this.username;
            Update();
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                chatApplication.SendMessage(this, txtRecipient.Text, txtInput.Text);
                txtInput.Clear();
            }
        }

        public string GetRecipient()
        {
            return txtRecipient.Text;
        }

        public void RemoveUserFromList(string username)
        {
            Invoke(new Action(() =>
            {
                var listItem = userList.FindItemWithText(username, false, 0, false);
                if (listItem != null)
                {
                    userList.Items.Remove(listItem);
                }
            }));
        }

        public void AddUserToList(string username)
        {
            Invoke(new Action(() =>
            {
                userList.Items.Add(username);
            }));
        }

        public void AddChatLine(ChatApplication.Message message)
        {
            Invoke(new Action(() =>
            {
                chatOutput.AppendText($"{message.Sender} @ {message.Recipient}: {message.MessageText}" + Environment.NewLine);
            }));
        }

        public void AddChatErrorLine(string errorMessage)
        {

            Invoke(new Action(() =>
            {
                chatOutput.SelectionStart = chatOutput.TextLength;
                chatOutput.SelectionLength = 0;

                chatOutput.SelectionColor = Color.Red;
                chatOutput.AppendText(errorMessage + Environment.NewLine);
                chatOutput.SelectionColor = chatOutput.ForeColor;
            }));
        }

        private void userList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userList.SelectedItems.Count > 0)
            {
                txtRecipient.Text = userList.SelectedItems[0].Text;
            }
        }

        private void signoutButton_Click(object sender, EventArgs e)
        {
            chatApplication.RemoveUserFromChat(username);
            Close();
        }

        private void txtRecipient_Click(object sender, EventArgs e)
        {

        }
    }
}
