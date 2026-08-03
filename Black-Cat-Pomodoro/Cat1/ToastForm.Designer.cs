using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BlackCatPomodoro
{
    partial class ToastForm
    {
        private IContainer components = null;
        private Label lblTitle;
        private Label lblMessage;
        private Panel pnlBorder;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlBorder = new Panel();
            this.lblTitle = new Label();
            this.lblMessage = new Label();
            this.pnlBorder.SuspendLayout();
            this.SuspendLayout();

            // ---- Form ----
            this.ClientSize = new Size(300, 80);
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.StartPosition = FormStartPosition.Manual;

            // ---- pnlBorder (圆角模拟) ----
            this.pnlBorder.BackColor = Color.FromArgb(40, 40, 44);
            this.pnlBorder.Dock = DockStyle.Fill;
            this.pnlBorder.Padding = new Padding(12, 8, 12, 8);

            // ---- lblTitle ----
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(240, 240, 240);
            this.lblTitle.Location = new Point(12, 10);
            this.lblTitle.Text = "Title";

            // ---- lblMessage ----
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new Font("Microsoft YaHei UI", 9F);
            this.lblMessage.ForeColor = Color.FromArgb(180, 180, 180);
            this.lblMessage.Location = new Point(12, 34);
            this.lblMessage.Text = "Message";

            this.pnlBorder.Controls.Add(this.lblTitle);
            this.pnlBorder.Controls.Add(this.lblMessage);
            this.Controls.Add(this.pnlBorder);
            this.Click += (s, e) => this.Close();
            this.pnlBorder.Click += (s, e) => this.Close();

            this.pnlBorder.ResumeLayout(false);
            this.pnlBorder.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
