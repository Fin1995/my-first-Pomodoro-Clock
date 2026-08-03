using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace BlackCatPomodoro
{
    public partial class MainForm : Form
    {
        private readonly DataService _data = new DataService();
        private readonly PomodoroService _pomodoro = new PomodoroService();
        private readonly AudioService _audio = new AudioService();

        private List<PomodoroTask> _tasks;
        private PomodoroTask _selectedTask;
        private AppTheme _currentTheme = AppTheme.Default;
        private SoundMode _soundMode = SoundMode.SystemBeep;
        private bool _notifyUseToast;
        private string _customSoundPath;
        private int _audioDeviceIdx = -1;

        private static readonly Color CFocus  = Color.FromArgb(200, 40, 40);
        private static readonly Color CRest   = Color.FromArgb(40, 140, 40);
        private static readonly Color CIdle   = Color.DimGray;
        private static readonly Color CPaused = Color.FromArgb(200, 140, 0);

        public MainForm()
        {
            InitializeComponent();
            menuThemeDefault.Checked = true;
            menuSoundBeep.Checked = true;
            menuNotifyBalloon.Checked = true;

            LoadIcon();
            _pomodoro.Tick += (r, t) => UpdateCountdown(r);
            _pomodoro.PhaseChanged += () => { UpdateTimerUI(); UpdateTrayState(); };
            _pomodoro.CycleCompleted += () => OnComplete();

            _tasks = _data.Load();
            RefreshTaskList();
            WireAll();
            LoadPrefs();
        }

        // ==================== 图标 ====================
        private void LoadIcon()
        {
            try
            {
                using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("BlackCatPomodoro.黑猫先生64×64.ico"))
                { if (s != null) { var i = new Icon(s); this.Icon = i; notifyIcon.Icon = i; } }
            }
            catch { }
        }

        // ==================== 数据 ====================
        private void RefreshTaskList()
        {
            int prev = cmbTask.SelectedIndex;
            cmbTask.DataSource = null; cmbTask.DataSource = _tasks;
            cmbTask.DisplayMember = "Name"; cmbTask.ValueMember = "Id";
            if (_tasks.Count > 0) { cmbTask.SelectedIndex = Math.Max(0, Math.Min(prev, _tasks.Count - 1)); SelectTask(_tasks[cmbTask.SelectedIndex]); }
            else { _selectedTask = null; ShowEmptyInfo(); }
        }
        private void SelectTask(PomodoroTask t)
        {
            _selectedTask = t;
            if (t == null) { ShowEmptyInfo(); return; }
            lblFocus.Text = $"专注时间: {t.FocusMinutes} 分钟"; lblRest.Text = $"休息时间: {t.RestMinutes} 分钟";
            lblRepeat.Text = $"执行: {t.RepeatCount} 次"; lblNotes.Text = string.IsNullOrEmpty(t.Notes) ? "备注: --" : $"备注: {t.Notes}";
        }
        private void ShowEmptyInfo() { lblFocus.Text = "专注时间: --"; lblRest.Text = "休息时间: --"; lblRepeat.Text = "执行: --"; lblNotes.Text = "备注: --"; }
        private void SaveAndRefresh() { if (!_data.Save(_tasks)) Msg("保存数据失败！"); RefreshTaskList(); }
        private static void Msg(string s) => MessageBox.Show(s, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        // ==================== 事件绑定 ====================
        private void WireAll()
        {
            FormClosing += (s, e) =>
            {
                if (_pomodoro.IsRunning)
                { if (MessageBox.Show("番茄钟运行中，关闭将最小化到托盘。确定？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) { e.Cancel = true; return; } }
                if (e.CloseReason == CloseReason.UserClosing) { e.Cancel = true; HideToTray(); }
            };
            cmbTask.SelectedIndexChanged += (s, e) => { if (cmbTask.SelectedItem is PomodoroTask t) SelectTask(t); };

            // 按钮
            btnRefresh.Click += (s, e) => RefreshTaskList();
            btnAdd.Click     += (s, e) => { using (var d = new TaskEditForm()) if (d.ShowDialog(this) == DialogResult.OK && d.Result != null) { _tasks.Add(d.Result); SaveAndRefresh(); } };
            btnEdit.Click    += (s, e) => { if (_selectedTask == null) { Msg("请先选择一个待办事项。"); return; } using (var d = new TaskEditForm(_selectedTask)) if (d.ShowDialog(this) == DialogResult.OK) SaveAndRefresh(); };
            btnDelete.Click  += (s, e) =>
            {
                if (_selectedTask == null) { Msg("请先选择一个待办事项。"); return; }
                if (_pomodoro.IsRunning && _pomodoro.CurrentTask?.Id == _selectedTask.Id) { Msg("该待办正在运行中，请先停止计时。"); return; }
                if (MessageBox.Show($"确定删除 \"{_selectedTask.Name}\"？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                { _tasks.Remove(_selectedTask); _selectedTask = null; SaveAndRefresh(); }
            };
            btnStart.Click += (s, e) => StartPomodoro();
            btnPause.Click += (s, e) => TogglePause();
            btnSkip.Click  += (s, e) => SkipPhase();
            btnStop.Click  += (s, e) => StopPomodoro();

            // 托盘
            notifyIcon.MouseClick += (s, e) => { if (e.Button == MouseButtons.Left) ShowFromTray(); };
            trayShow.Click    += (s, e) => ShowFromTray();
            trayStart.Click   += (s, e) => StartPomodoro();
            trayPause.Click   += (s, e) => TogglePause();
            traySkip.Click    += (s, e) => SkipPhase();
            trayStop.Click    += (s, e) => StopPomodoro();
            traySettings.Click+= (s, e) => ShowFromTray();
            trayDebugNotify.Click += (s, e) => DoTestNotify();
            trayDebugSound.Click  += (s, e) => DoTestSound();
            trayAbout.Click   += (s, e) => ShowAbout();
            trayExit.Click    += (s, e) => ExitApp();

            // 菜单
            menuThemeDefault.Click += (s, e) => SwitchTheme(AppTheme.Default);
            menuThemeDark.Click    += (s, e) => SwitchTheme(AppTheme.Dark);
            menuFont.Click         += (s, e) => ChangeFont();
            menuSoundBeep.Click    += (s, e) => SetSound(SoundMode.SystemBeep);
            menuSoundCustom.Click  += (s, e) => SetSoundCustom();
            menuSoundSilent.Click  += (s, e) => SetSound(SoundMode.Silent);
            menuSoundDevice.Click  += (s, e) => ChooseDevice();
            menuNotifyBalloon.Click+= (s, e) => SetNotify(false);
            menuNotifyToast.Click  += (s, e) => SetNotify(true);
            menuDebugNotify.Click  += (s, e) => DoTestNotify();
            menuDebugSound.Click   += (s, e) => DoTestSound();
            menuReset.Click        += (s, e) => ResetAllSettings();
            menuAbout.Click        += (s, e) => ShowAbout();
        }

        // ==================== 托盘 ====================
        private void HideToTray() { Hide(); notifyIcon.Visible = true; notifyIcon.ShowBalloonTip(1500, "Black Cat Pomodoro Clock", "黑猫番茄钟在托盘运行中", ToolTipIcon.Info); }
        private void ShowFromTray() { Show(); WindowState = FormWindowState.Normal; Activate(); notifyIcon.Visible = false; }
        private void ExitApp() { _pomodoro.Stop(); _ = _data.Save(_tasks); _audio.Dispose(); notifyIcon.Visible = false; notifyIcon.Dispose(); Application.Exit(); }

        // ==================== 番茄钟 ====================
        private void StartPomodoro()
        {
            if (_selectedTask == null) { Msg("请先选择一个待办事项。"); return; }
            _pomodoro.Start(_selectedTask); SetRunning(true); UpdateTimerUI(); UpdateTrayState();
        }
        private void TogglePause() { _pomodoro.TogglePause(); UpdateTimerUI(); UpdateTrayState(); }
        private void SkipPhase()   { _pomodoro.Skip(); UpdateTimerUI(); }
        private void StopPomodoro()
        {
            if (MessageBox.Show("确定停止当前番茄钟？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            { _pomodoro.Stop(); SetRunning(false); ResetTimerUI(); UpdateTrayState(); }
        }

        private void UpdateTrayState()
        {
            bool run = _pomodoro.IsRunning, paused = _pomodoro.IsPaused;
            trayStart.Enabled = !run;
            trayPause.Enabled = run;
            traySkip.Enabled = run;
            trayStop.Enabled = run;
            trayPause.Text = paused ? "继续" : "暂停";
        }

        private void OnComplete()
        {
            if (_soundMode == SoundMode.SystemBeep) AudioService.PlaySystemBeep();
            else if (_soundMode == SoundMode.CustomFile) PlayCustomSound();

            SetRunning(false);
            lblCountdown.Text = "00:00"; lblCountdown.ForeColor = CIdle;
            lblStatus.Text = "已完成";   lblStatus.ForeColor = CRest;
            lblRound.Text = $"{_selectedTask?.Name ?? ""} -- {_pomodoro.TotalRounds} 轮完成!";

            if (_notifyUseToast) ShowToast(); else ShowBalloon();

            MessageBox.Show($"\"{_selectedTask?.Name ?? ""}\" {_pomodoro.TotalRounds} 轮番茄钟全部完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ResetTimerUI(); UpdateTrayState();
        }

        private void PlayCustomSound()
        {
            try { if (!string.IsNullOrEmpty(_customSoundPath) && File.Exists(_customSoundPath)) _audio.Play(_customSoundPath, _audioDeviceIdx); else AudioService.PlaySystemBeep(); }
            catch { AudioService.PlaySystemBeep(); }
        }

        // ==================== 通知 ====================
        private void ShowToast() { try { new ToastForm("番茄钟完成", $"\"{_selectedTask?.Name ?? ""}\" {_pomodoro.TotalRounds} 轮完成！").Pop(); } catch { } }
        private void ShowBalloon()
        {
            try { notifyIcon.Visible = true; notifyIcon.ShowBalloonTip(5000, "番茄钟完成", $"\"{_selectedTask?.Name ?? ""}\" {_pomodoro.TotalRounds} 轮完成！", ToolTipIcon.Info); }
            catch { }
        }
        private void DoTestNotify()
        {
            if (_notifyUseToast) { try { new ToastForm("测试通知", "软件内置弹窗正常！").Pop(); } catch { Msg("弹窗失败。"); } }
            else { notifyIcon.Visible = true; notifyIcon.ShowBalloonTip(3000, "测试通知", "系统托盘气泡正常！", ToolTipIcon.Info); }
        }
        private void DoTestSound()
        {
            if (_soundMode == SoundMode.Silent) { Msg("当前为静音模式，不会播放提示音。\n\n请到 设置 -> 提示音 切换模式。"); return; }
            if (_soundMode == SoundMode.CustomFile && (string.IsNullOrEmpty(_customSoundPath) || !File.Exists(_customSoundPath)))
            { Msg("自定义音频文件不存在，已恢复为系统提示音。\n\n请到 设置 -> 提示音 重新选择文件。"); SetSound(SoundMode.SystemBeep); return; }
            if (_soundMode == SoundMode.SystemBeep) AudioService.PlaySystemBeep();
            else PlayCustomSound();
        }

        // ==================== 提示音 ====================
        private void SetSound(SoundMode m)
        {
            _soundMode = m;
            menuSoundBeep.Checked   = m == SoundMode.SystemBeep;
            menuSoundCustom.Checked = m == SoundMode.CustomFile;
            menuSoundSilent.Checked = m == SoundMode.Silent;
            SaveAllPrefs();
        }
        private void SetSoundCustom()
        {
            using (var d = new OpenFileDialog { Filter = "音频文件|*.wav;*.mp3|WAV|*.wav|MP3|*.mp3|所有|*.*", Title = "选择提示音" })
            {
                if (d.ShowDialog(this) == DialogResult.OK) { _customSoundPath = d.FileName; SetSound(SoundMode.CustomFile); }
            }
        }
        private void ChooseDevice()
        {
            var devs = AudioService.GetDevices();
            if (devs.Count == 0) { Msg("未检测到音频输出设备。"); return; }
            var items = new List<string> { "系统默认设备" }; items.AddRange(devs.Select(d => d.Name));
            using (var f = new Form { Text = "选择播放设备", ClientSize = new Size(380, 110), FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, StartPosition = FormStartPosition.CenterParent })
            {
                var cb = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(12, 12), Size = new Size(356, 25) };
                cb.Items.AddRange(items.ToArray());
                cb.SelectedIndex = (_audioDeviceIdx < 0 || _audioDeviceIdx >= devs.Count) ? 0 : _audioDeviceIdx + 1;
                var btn = new Button { Text = "确定", Location = new Point(290, 48), Size = new Size(75, 28) };
                btn.Click += (s, e) => { _audioDeviceIdx = cb.SelectedIndex - 1; SaveAllPrefs(); f.DialogResult = DialogResult.OK; f.Close(); };
                f.Controls.Add(cb); f.Controls.Add(btn); f.ShowDialog(this);
            }
        }
        private void SetNotify(bool toast) { _notifyUseToast = toast; menuNotifyBalloon.Checked = !toast; menuNotifyToast.Checked = toast; SaveAllPrefs(); }

        // ==================== UI ====================
        private void UpdateCountdown(int s) { lblCountdown.Text = $"{s / 60:D2}:{s % 60:D2}"; }
        private void UpdateTimerUI()
        {
            if (_pomodoro.Phase == PomodoroPhase.Idle) { lblCountdown.Text = "00:00"; lblCountdown.ForeColor = CIdle; lblStatus.Text = "就绪"; lblStatus.ForeColor = CIdle; lblRound.Text = ""; lblCountdown.Font = new Font("Consolas", 32F, FontStyle.Bold); }
            else if (_pomodoro.Phase == PomodoroPhase.Focusing)
            {
                lblStatus.Text = _pomodoro.IsPaused ? "[||] 专注 -- 已暂停" : "[>] 专注中"; lblStatus.ForeColor = _pomodoro.IsPaused ? CPaused : CFocus;
                lblCountdown.ForeColor = _pomodoro.IsPaused ? CPaused : CFocus;
                lblRound.Text = $"第 {_pomodoro.CurrentRound}/{_pomodoro.TotalRounds} 轮  |  任务: {_pomodoro.CurrentTask?.Name}";
                UpdateCountdown(_pomodoro.RemainingSeconds);
            }
            else
            {
                lblStatus.Text = _pomodoro.IsPaused ? "[||] 休息 -- 已暂停" : "[.] 休息中"; lblStatus.ForeColor = _pomodoro.IsPaused ? CPaused : CRest;
                lblCountdown.ForeColor = _pomodoro.IsPaused ? CPaused : CRest;
                lblRound.Text = $"第 {_pomodoro.CurrentRound}/{_pomodoro.TotalRounds} 轮  |  任务: {_pomodoro.CurrentTask?.Name}";
                UpdateCountdown(_pomodoro.RemainingSeconds);
            }
            btnPause.Text = _pomodoro.IsPaused ? "继续" : "暂停";
        }
        private void ResetTimerUI() { lblCountdown.Text = "00:00"; lblCountdown.ForeColor = CIdle; lblStatus.Text = "就绪"; lblStatus.ForeColor = CIdle; lblRound.Text = ""; btnPause.Text = "暂停"; }
        private void SetRunning(bool r)
        {
            btnStart.Enabled = !r; btnPause.Enabled = r; btnSkip.Enabled = r; btnStop.Enabled = r;
            cmbTask.Enabled = !r; btnRefresh.Enabled = !r; btnAdd.Enabled = !r; btnEdit.Enabled = !r; btnDelete.Enabled = !r;
        }
        private void ShowAbout() { MessageBox.Show("Black Cat Pomodoro Clock\n\n作者: 黑猫先生\nC# WinForms 重构版\n基于番茄工作法 (Pomodoro Technique)", "关于", MessageBoxButtons.OK, MessageBoxIcon.Information); }

        // ==================== 主题 ====================
        private void SwitchTheme(AppTheme t)
        {
            _currentTheme = t; menuThemeDefault.Checked = t == AppTheme.Default; menuThemeDark.Checked = t == AppTheme.Dark;
            BackColor = t == AppTheme.Dark ? ThemeService.DarkBackground : SystemColors.Control;
            pnlTimer.BackColor = t == AppTheme.Dark ? ThemeService.DarkSurface : SystemColors.Control;
            ThemeService.Apply(this, t);
            lblNotes.ForeColor = lblTrayHint.ForeColor = t == AppTheme.Dark ? ThemeService.DarkSubText : SystemColors.GrayText;
            SaveAllPrefs();
        }

        // ==================== 字体 ====================
        private void ChangeFont()
        {
            using (var d = new FontDialog { Font = Font, FontMustExist = true, ShowColor = false, ShowEffects = false, AllowScriptChange = false, FixedPitchOnly = false })
            {
                if (d.ShowDialog(this) == DialogResult.OK) { ApplyGlobalFont(d.Font); SaveAllPrefs(); }
            }
        }
        private void ApplyGlobalFont(Font f) { Font = f; lblCountdown.Font = new Font(IsMono(f.Name) ? f.Name : "Consolas", Math.Max(28F, f.Size * 3.2F), FontStyle.Bold); }
        private static bool IsMono(string n) { var l = n.ToLower(); return l.Contains("console") || l.Contains("mono") || l.Contains("courier") || l.Contains("fixed") || l.Contains("coding") || l.Contains("typewriter"); }

        // ==================== 设置持久化 ====================
        private void LoadPrefs()
        {
            try { if (Properties.Settings.Default.Theme == "Dark") SwitchTheme(AppTheme.Dark); } catch { }
            try { _soundMode = (SoundMode)Properties.Settings.Default.SoundMode; _customSoundPath = Properties.Settings.Default.SoundFilePath; _audioDeviceIdx = Properties.Settings.Default.SoundDevice; _notifyUseToast = Properties.Settings.Default.NotifyToast;
                menuSoundBeep.Checked = _soundMode == SoundMode.SystemBeep; menuSoundCustom.Checked = _soundMode == SoundMode.CustomFile; menuSoundSilent.Checked = _soundMode == SoundMode.Silent;
                menuNotifyBalloon.Checked = !_notifyUseToast; menuNotifyToast.Checked = _notifyUseToast;
            } catch { }
            try { var fs = Properties.Settings.Default.AppFont; if (!string.IsNullOrEmpty(fs)) { var f = new FontConverter().ConvertFromString(fs) as Font; if (f != null) ApplyGlobalFont(f); } } catch { }
        }
        private void SaveAllPrefs()
        {
            try { var s = Properties.Settings.Default; s.Theme = _currentTheme == AppTheme.Dark ? "Dark" : "Default"; s.SoundMode = (int)_soundMode; s.SoundFilePath = _customSoundPath ?? ""; s.SoundDevice = _audioDeviceIdx; s.NotifyToast = _notifyUseToast; s.AppFont = new FontConverter().ConvertToString(Font); s.Save(); } catch { }
        }
        private void ResetAllSettings()
        {
            if (MessageBox.Show("确定重置所有设置吗？\n\n这将清除: 主题、字体、提示音、通知、播放设备\n所有偏好将恢复默认值。", "确认重置", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try { Properties.Settings.Default.Reset(); } catch { }
            _soundMode = SoundMode.SystemBeep; _customSoundPath = null; _audioDeviceIdx = -1; _notifyUseToast = false;
            menuSoundBeep.Checked = true; menuSoundCustom.Checked = false; menuSoundSilent.Checked = false;
            menuNotifyBalloon.Checked = true; menuNotifyToast.Checked = false;
            SwitchTheme(AppTheme.Default); ApplyGlobalFont(new Font("Microsoft YaHei UI", 9F));
            SaveAllPrefs();
        }

        protected override void OnFormClosed(FormClosedEventArgs e) { _ = _data.Save(_tasks); _audio.Dispose(); _pomodoro.Dispose(); base.OnFormClosed(e); }
    }
}
