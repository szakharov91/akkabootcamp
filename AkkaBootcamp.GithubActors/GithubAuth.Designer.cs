namespace AkkaBootcamp.GithubActors
{
    partial class GithubAuth
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
            label1 = new Label();
            tbOAuth = new TextBox();
            lblAuthStatus = new Label();
            linkGhLabel = new LinkLabel();
            btnAuthenticate = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(14, 10);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(172, 18);
            label1.TabIndex = 0;
            label1.Text = "GitHub Access Token";
            // 
            // tbOAuth
            // 
            tbOAuth.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tbOAuth.Location = new Point(222, 7);
            tbOAuth.Margin = new Padding(4, 3, 4, 3);
            tbOAuth.Name = "tbOAuth";
            tbOAuth.Size = new Size(442, 24);
            tbOAuth.TabIndex = 1;
            // 
            // lblAuthStatus
            // 
            lblAuthStatus.AutoSize = true;
            lblAuthStatus.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblAuthStatus.Location = new Point(218, 38);
            lblAuthStatus.Margin = new Padding(4, 0, 4, 0);
            lblAuthStatus.Name = "lblAuthStatus";
            lblAuthStatus.Size = new Size(87, 18);
            lblAuthStatus.TabIndex = 2;
            lblAuthStatus.Text = "lblGHStatus";
            lblAuthStatus.Visible = false;
            // 
            // linkGhLabel
            // 
            linkGhLabel.AutoSize = true;
            linkGhLabel.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            linkGhLabel.Location = new Point(13, 152);
            linkGhLabel.Margin = new Padding(4, 0, 4, 0);
            linkGhLabel.Name = "linkGhLabel";
            linkGhLabel.Size = new Size(273, 18);
            linkGhLabel.TabIndex = 3;
            linkGhLabel.TabStop = true;
            linkGhLabel.Text = "How to get a GitHub Access Token";
            linkGhLabel.LinkClicked += linkGhLabel_LinkClicked;
            // 
            // btnAuthenticate
            // 
            btnAuthenticate.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAuthenticate.Location = new Point(250, 93);
            btnAuthenticate.Margin = new Padding(4, 3, 4, 3);
            btnAuthenticate.Name = "btnAuthenticate";
            btnAuthenticate.Size = new Size(159, 37);
            btnAuthenticate.TabIndex = 4;
            btnAuthenticate.Text = "Authenticate";
            btnAuthenticate.UseVisualStyleBackColor = true;
            btnAuthenticate.Click += btnAuthenticate_Click;
            // 
            // GithubAuth
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(679, 179);
            Controls.Add(btnAuthenticate);
            Controls.Add(linkGhLabel);
            Controls.Add(lblAuthStatus);
            Controls.Add(tbOAuth);
            Controls.Add(label1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "GithubAuth";
            Text = "Sign in to GitHub";
            Load += GithubAuth_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbOAuth;
        private System.Windows.Forms.Label lblAuthStatus;
        private System.Windows.Forms.LinkLabel linkGhLabel;
        private System.Windows.Forms.Button btnAuthenticate;
    }
}