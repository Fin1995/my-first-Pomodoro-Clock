using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BlackCatPomodoro
{
    partial class MainForm
    {
        private IContainer components = null;

        // ---- 菜单 ----
        private MenuStrip menuStrip;
        private ToolStripMenuItem menuSettings;
        private ToolStripMenuItem menuTheme;
        private ToolStripMenuItem menuThemeDefault;
        private ToolStripMenuItem menuThemeDark;
        private ToolStripMenuItem menuFont;
        private ToolStripMenuItem menuSound;
        private ToolStripMenuItem menuSoundBeep;
        private ToolStripMenuItem menuSoundCustom;
        private ToolStripMenuItem menuSoundSilent;
        private ToolStripMenuItem menuSoundDevice;
        private ToolStripMenuItem menuNotify;
        private ToolStripMenuItem menuNotifyBalloon;
        private ToolStripMenuItem menuNotifyToast;
        private ToolStripMenuItem menuDebug;
        private ToolStripMenuItem menuDebugNotify;
        private ToolStripMenuItem menuDebugSound;
        private ToolStripMenuItem menuReset;
        private ToolStripMenuItem menuHelp;
        private ToolStripMenuItem menuAbout;

        // ---- 选择待办 ----
        private GroupBox grpSelect;
        private ComboBox cmbTask;
        private TableLayoutPanel tlpSelectBtns;
        private Button btnRefresh;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;

        // ---- 待办信息 ----
        private GroupBox grpInfo;
        private Label lblFocus;
        private Label lblRest;
        private Label lblRepeat;
        private Label lblNotes;

        // ---- 计时器 ----
        private Panel pnlTimer;
        private Label lblCountdown;
        private Label lblStatus;
        private Label lblRound;

        // ---- 操作按钮 ----
        private TableLayoutPanel tlpActions;
        private Button btnStart;
        private Button btnPause;
        private Button btnSkip;
        private Button btnStop;

        // ---- 托盘 ----
        private Label lblTrayHint;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip trayMenu;
        private ToolStripMenuItem trayShow;
        private ToolStripMenuItem traySep0;
        private ToolStripMenuItem trayStart;
        private ToolStripMenuItem trayPause;
        private ToolStripMenuItem traySkip;
        private ToolStripMenuItem trayStop;
        private ToolStripMenuItem traySep1;
        private ToolStripMenuItem traySettings;
        private ToolStripMenuItem trayDebug;
        private ToolStripMenuItem trayDebugNotify;
        private ToolStripMenuItem trayDebugSound;
        private ToolStripMenuItem trayAbout;
        private ToolStripSeparator traySep2;
        private ToolStripMenuItem trayExit;

        protected override void Dispose(bool disposing)
        {
            if (disposing) { components?.Dispose(); notifyIcon?.Dispose(); }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();

            // ==================== 菜单栏 ====================
            this.menuStrip = new MenuStrip();
            this.menuSettings     = new ToolStripMenuItem();
            this.menuTheme        = new ToolStripMenuItem();
            this.menuThemeDefault = new ToolStripMenuItem();
            this.menuThemeDark    = new ToolStripMenuItem();
            this.menuFont         = new ToolStripMenuItem();
            this.menuSound        = new ToolStripMenuItem();
            this.menuSoundBeep    = new ToolStripMenuItem();
            this.menuSoundCustom  = new ToolStripMenuItem();
            this.menuSoundSilent  = new ToolStripMenuItem();
            this.menuSoundDevice  = new ToolStripMenuItem();
            this.menuNotify       = new ToolStripMenuItem();
            this.menuNotifyBalloon= new ToolStripMenuItem();
            this.menuNotifyToast  = new ToolStripMenuItem();
            this.menuDebug        = new ToolStripMenuItem();
            this.menuDebugNotify  = new ToolStripMenuItem();
            this.menuDebugSound   = new ToolStripMenuItem();
            this.menuReset        = new ToolStripMenuItem();
            this.menuHelp         = new ToolStripMenuItem();
            this.menuAbout        = new ToolStripMenuItem();

            menuTheme.DropDownItems.AddRange(       new ToolStripItem[] { menuThemeDefault, menuThemeDark });
            menuSound.DropDownItems.AddRange(       new ToolStripItem[] { menuSoundBeep, menuSoundCustom, menuSoundSilent, new ToolStripSeparator(), menuSoundDevice });
            menuNotify.DropDownItems.AddRange(      new ToolStripItem[] { menuNotifyBalloon, menuNotifyToast });
            menuDebug.DropDownItems.AddRange(       new ToolStripItem[] { menuDebugNotify, menuDebugSound });
            menuSettings.DropDownItems.AddRange(    new ToolStripItem[] { menuTheme, menuFont, menuSound, menuNotify, menuDebug, new ToolStripSeparator(), menuReset });
            menuHelp.DropDownItems.AddRange(        new ToolStripItem[] { menuAbout });
            menuStrip.Items.AddRange(               new ToolStripItem[] { menuSettings, menuHelp });

            menuSettings.Text      = "设置(&S)";
            menuTheme.Text         = "主题(&T)";
            menuThemeDefault.Text  = "系统默认";
            menuThemeDark.Text     = "暗色模式";
            menuFont.Text          = "字体(&F)...";
            menuSound.Text         = "提示音(&N)";
            menuSoundBeep.Text     = "系统提示音";
            menuSoundCustom.Text   = "自定义音频文件...";
            menuSoundSilent.Text   = "静音";
            menuSoundDevice.Text   = "播放设备(&D)...";
            menuNotify.Text        = "通知样式(&O)";
            menuNotifyBalloon.Text = "系统托盘气泡";
            menuNotifyToast.Text   = "软件内置弹窗";
            menuDebug.Text         = "调试(&U)";
            menuDebugNotify.Text   = "测试通知";
            menuDebugSound.Text    = "测试提示音";
            menuReset.Text         = "重置所有设置";
            menuHelp.Text          = "帮助(&H)";
            menuAbout.Text         = "关于(&A)";

            // ==================== 选择待办 ====================
            this.grpSelect = new GroupBox();
            this.cmbTask = new ComboBox();
            this.tlpSelectBtns = new TableLayoutPanel();
            this.btnRefresh = new Button();
            this.btnAdd = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();

            grpSelect.SuspendLayout(); tlpSelectBtns.SuspendLayout();

            grpSelect.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpSelect.Location = new Point(12, 28); grpSelect.Size = new Size(436, 84);
            grpSelect.Text = "选择待办";

            cmbTask.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTask.Location = new Point(12, 24); cmbTask.Size = new Size(286, 25);

            tlpSelectBtns.ColumnCount = 2; tlpSelectBtns.RowCount = 2;
            tlpSelectBtns.Location = new Point(304, 19); tlpSelectBtns.Size = new Size(122, 54);
            tlpSelectBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpSelectBtns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpSelectBtns.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpSelectBtns.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpSelectBtns.Margin = new Padding(0); tlpSelectBtns.Padding = new Padding(0);

            btnRefresh.Text = "刷新"; btnRefresh.Dock = DockStyle.Fill;
            btnRefresh.UseVisualStyleBackColor = true; btnRefresh.Margin = new Padding(1);
            btnAdd.Text = "新增"; btnAdd.Dock = DockStyle.Fill;
            btnAdd.UseVisualStyleBackColor = true; btnAdd.Margin = new Padding(1);
            btnEdit.Text = "编辑"; btnEdit.Dock = DockStyle.Fill;
            btnEdit.UseVisualStyleBackColor = true; btnEdit.Margin = new Padding(1);
            btnDelete.Text = "删除"; btnDelete.Dock = DockStyle.Fill;
            btnDelete.UseVisualStyleBackColor = true; btnDelete.Margin = new Padding(1);
            tlpSelectBtns.Controls.Add(btnRefresh, 0, 0); tlpSelectBtns.Controls.Add(btnAdd, 1, 0);
            tlpSelectBtns.Controls.Add(btnEdit, 0, 1);    tlpSelectBtns.Controls.Add(btnDelete, 1, 1);
            grpSelect.Controls.Add(cmbTask); grpSelect.Controls.Add(tlpSelectBtns);

            // ==================== 待办信息 ====================
            this.grpInfo = new GroupBox();
            this.lblFocus = new Label(); this.lblRest = new Label();
            this.lblRepeat = new Label(); this.lblNotes = new Label();
            grpInfo.SuspendLayout();

            grpInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpInfo.Location = new Point(12, 117); grpInfo.Size = new Size(436, 58);
            grpInfo.Text = "待办信息";
            lblFocus.AutoSize = true;  lblFocus.Location = new Point(12, 22);  lblFocus.Text = "专注时间: --";
            lblRest.AutoSize = true;   lblRest.Location = new Point(148, 22);  lblRest.Text = "休息时间: --";
            lblRepeat.AutoSize = true; lblRepeat.Location = new Point(284, 22); lblRepeat.Text = "执行: --";
            lblNotes.AutoSize = true;  lblNotes.Location = new Point(12, 40);  lblNotes.Text = "备注: --";
            lblNotes.ForeColor = SystemColors.GrayText;
            grpInfo.Controls.Add(lblFocus); grpInfo.Controls.Add(lblRest);
            grpInfo.Controls.Add(lblRepeat); grpInfo.Controls.Add(lblNotes);

            // ==================== 计时器 ====================
            this.pnlTimer = new Panel();
            this.lblCountdown = new Label(); this.lblStatus = new Label(); this.lblRound = new Label();
            pnlTimer.SuspendLayout();

            pnlTimer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlTimer.BorderStyle = BorderStyle.FixedSingle;
            pnlTimer.Location = new Point(12, 180); pnlTimer.Size = new Size(436, 122);
            pnlTimer.MinimumSize = new Size(300, 80);

            lblCountdown.AutoSize = false; lblCountdown.TextAlign = ContentAlignment.MiddleCenter;
            lblCountdown.Font = new Font("Consolas", 32F, FontStyle.Bold);
            lblCountdown.Location = new Point(0, 6); lblCountdown.Size = new Size(434, 58);
            lblCountdown.ForeColor = Color.DimGray; lblCountdown.Text = "00:00";

            lblStatus.AutoSize = false; lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            lblStatus.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular);
            lblStatus.Location = new Point(0, 66); lblStatus.Size = new Size(434, 26);
            lblStatus.Text = "就绪";

            lblRound.AutoSize = false; lblRound.TextAlign = ContentAlignment.MiddleCenter;
            lblRound.Location = new Point(0, 96); lblRound.Size = new Size(434, 20);
            lblRound.ForeColor = SystemColors.GrayText;

            pnlTimer.Controls.Add(lblCountdown); pnlTimer.Controls.Add(lblStatus); pnlTimer.Controls.Add(lblRound);

            // ==================== 操作按钮 ====================
            this.tlpActions = new TableLayoutPanel();
            this.btnStart = new Button(); this.btnPause = new Button();
            this.btnSkip = new Button();  this.btnStop = new Button();
            tlpActions.SuspendLayout();

            tlpActions.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tlpActions.ColumnCount = 4; tlpActions.RowCount = 1;
            tlpActions.Location = new Point(12, 309); tlpActions.Size = new Size(436, 34);
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpActions.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpActions.Margin = new Padding(0); tlpActions.Padding = new Padding(0);

            btnStart.Text = "开始专注"; btnStart.Dock = DockStyle.Fill;
            btnStart.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            btnStart.UseVisualStyleBackColor = true; btnStart.Margin = new Padding(2);
            btnPause.Text = "暂停"; btnPause.Dock = DockStyle.Fill;
            btnPause.Enabled = false; btnPause.UseVisualStyleBackColor = true; btnPause.Margin = new Padding(2);
            btnSkip.Text = "跳过"; btnSkip.Dock = DockStyle.Fill;
            btnSkip.Enabled = false; btnSkip.UseVisualStyleBackColor = true; btnSkip.Margin = new Padding(2);
            btnStop.Text = "停止"; btnStop.Dock = DockStyle.Fill;
            btnStop.Enabled = false; btnStop.UseVisualStyleBackColor = true; btnStop.Margin = new Padding(2);
            tlpActions.Controls.Add(btnStart, 0, 0); tlpActions.Controls.Add(btnPause, 1, 0);
            tlpActions.Controls.Add(btnSkip, 2, 0);   tlpActions.Controls.Add(btnStop, 3, 0);

            // ==================== 托盘提示 ====================
            this.lblTrayHint = new Label();
            lblTrayHint.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblTrayHint.AutoSize = true; lblTrayHint.Location = new Point(12, 350);
            lblTrayHint.Text = "提示: 关闭窗口将最小化到系统托盘";
            lblTrayHint.ForeColor = SystemColors.GrayText;

            // ==================== 系统托盘菜单 ====================
            this.trayMenu        = new ContextMenuStrip(this.components);
            this.trayShow        = new ToolStripMenuItem();
            this.traySep0        = new ToolStripMenuItem("-");
            this.trayStart       = new ToolStripMenuItem();
            this.trayPause       = new ToolStripMenuItem();
            this.traySkip        = new ToolStripMenuItem();
            this.trayStop        = new ToolStripMenuItem();
            this.traySep1        = new ToolStripMenuItem("-");
            this.traySettings    = new ToolStripMenuItem();
            this.trayDebug       = new ToolStripMenuItem();
            this.trayDebugNotify = new ToolStripMenuItem();
            this.trayDebugSound  = new ToolStripMenuItem();
            this.trayAbout       = new ToolStripMenuItem();
            this.traySep2        = new ToolStripSeparator();
            this.trayExit        = new ToolStripMenuItem();

            trayShow.Text        = "显示主窗口";
            trayStart.Text       = "开始专注";  trayStart.Enabled = false;
            trayPause.Text       = "暂停";      trayPause.Enabled = false;
            traySkip.Text        = "跳过";      traySkip.Enabled = false;
            trayStop.Text        = "停止";      trayStop.Enabled = false;
            traySettings.Text    = "设置";
            trayDebug.Text       = "调试";
            trayDebugNotify.Text = "测试通知";
            trayDebugSound.Text  = "测试提示音";
            trayAbout.Text       = "关于";
            trayExit.Text        = "退出";

            trayDebug.DropDownItems.AddRange(new ToolStripItem[] { trayDebugNotify, trayDebugSound });
            trayMenu.Items.AddRange(new ToolStripItem[] {
                trayShow, traySep0, trayStart, trayPause, traySkip, trayStop,
                traySep1, traySettings, trayDebug, trayAbout, traySep2, trayExit });

            this.notifyIcon = new NotifyIcon(this.components);
            notifyIcon.ContextMenuStrip = trayMenu;
            notifyIcon.Text = "Black Cat Pomodoro Clock";
            notifyIcon.Visible = false;

            // ==================== Form ====================
            this.AutoScaleDimensions = new SizeF(6F, 12F);
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new Size(460, 375);
            this.MinimumSize = new Size(380, 340);
            this.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Black Cat Pomodoro Clock";

            this.Controls.Add(menuStrip); this.Controls.Add(grpSelect); this.Controls.Add(grpInfo);
            this.Controls.Add(pnlTimer); this.Controls.Add(tlpActions); this.Controls.Add(lblTrayHint);
            this.MainMenuStrip = menuStrip;

            grpSelect.ResumeLayout(false); tlpSelectBtns.ResumeLayout(false);
            grpInfo.ResumeLayout(false); grpInfo.PerformLayout();
            pnlTimer.ResumeLayout(false); tlpActions.ResumeLayout(false);
            ResumeLayout(false); PerformLayout();
        }
    }
}
