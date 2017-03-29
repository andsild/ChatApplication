namespace ChatApplicationGui
{
    partial class ChatForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtInput = new System.Windows.Forms.TextBox();
            this.chatOutput = new System.Windows.Forms.RichTextBox();
            this.userList = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtRecipient = new System.Windows.Forms.Label();
            this.signoutButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInput.Location = new System.Drawing.Point(161, 6);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(297, 20);
            this.txtInput.TabIndex = 0;
            // 
            // chatOutput
            // 
            this.chatOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chatOutput.BackColor = System.Drawing.SystemColors.Window;
            this.chatOutput.Location = new System.Drawing.Point(161, 18);
            this.chatOutput.Name = "chatOutput";
            this.chatOutput.ReadOnly = true;
            this.chatOutput.Size = new System.Drawing.Size(297, 323);
            this.chatOutput.TabIndex = 2;
            this.chatOutput.TabStop = false;
            this.chatOutput.Text = "";
            // 
            // userList
            // 
            this.userList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.userList.LabelWrap = false;
            this.userList.Location = new System.Drawing.Point(13, 47);
            this.userList.MultiSelect = false;
            this.userList.Name = "userList";
            this.userList.Size = new System.Drawing.Size(142, 294);
            this.userList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.userList.TabIndex = 3;
            this.userList.UseCompatibleStateImageBehavior = false;
            this.userList.View = System.Windows.Forms.View.List;
            this.userList.SelectedIndexChanged += new System.EventHandler(this.userList_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtRecipient);
            this.panel1.Controls.Add(this.txtInput);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 347);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(479, 33);
            this.panel1.TabIndex = 1;
            this.panel1.TabStop = true;
            // 
            // txtRecipient
            // 
            this.txtRecipient.Location = new System.Drawing.Point(14, 9);
            this.txtRecipient.Name = "txtRecipient";
            this.txtRecipient.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtRecipient.Size = new System.Drawing.Size(141, 15);
            this.txtRecipient.TabIndex = 1;
            this.txtRecipient.Text = "DONT LOOK EEEK NOOO";
            this.txtRecipient.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtRecipient.Click += new System.EventHandler(this.txtRecipient_Click);
            // 
            // signoutButton
            // 
            this.signoutButton.Location = new System.Drawing.Point(47, 18);
            this.signoutButton.Name = "signoutButton";
            this.signoutButton.Size = new System.Drawing.Size(75, 23);
            this.signoutButton.TabIndex = 5;
            this.signoutButton.Text = "Sign out";
            this.signoutButton.UseVisualStyleBackColor = true;
            this.signoutButton.Click += new System.EventHandler(this.signoutButton_Click);
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 380);
            this.Controls.Add(this.signoutButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.userList);
            this.Controls.Add(this.chatOutput);
            this.Name = "ChatForm";
            this.Text = "ChatApplication";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.RichTextBox chatOutput;
        private System.Windows.Forms.ListView userList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label txtRecipient;
        private System.Windows.Forms.Button signoutButton;
    }
}

