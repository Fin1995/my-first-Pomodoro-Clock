using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlackCatPomodoro
{
    /// <summary>
    /// 右下角通知弹窗 -- 无边框、自动消失、可点击关闭
    /// </summary>
    public partial class ToastForm : Form
    {
        private readonly Timer _closeTimer;

        public ToastForm(string title, string message)
        {
            InitializeComponent();
            lblTitle.Text = title;
            lblMessage.Text = message;

            _closeTimer = new Timer { Interval = 6000 };
            _closeTimer.Tick += (s, e) => { _closeTimer.Stop(); Close(); };
        }

        public void Pop()
        {
            // 定位到屏幕右下角（工作区）
            var area = Screen.PrimaryScreen.WorkingArea;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(
                area.Right - this.Width - 16,
                area.Bottom - this.Height - 16);

            // 淡入效果：从透明到不透明
            this.Opacity = 0;
            this.Show();
            FadeIn();
            _closeTimer.Start();
        }

        private async void FadeIn()
        {
            for (double o = 0; o <= 1.0; o += 0.1)
            {
                this.Opacity = o;
                await System.Threading.Tasks.Task.Delay(20);
            }
            this.Opacity = 1.0;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            _closeTimer.Stop();
            Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _closeTimer?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
