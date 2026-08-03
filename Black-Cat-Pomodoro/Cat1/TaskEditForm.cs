using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlackCatPomodoro
{
    /// <summary>
    /// 新增 / 编辑待办弹窗 -- 通过传入 null 或已有任务区分模式
    /// </summary>
    public partial class TaskEditForm : Form
    {
        public PomodoroTask Result { get; private set; }

        /// <param name="task">null = 新增模式, 非 null = 编辑模式</param>
        public TaskEditForm(PomodoroTask task = null)
        {
            InitializeComponent();
            ThemeService.Apply(this, ThemeService.CurrentTheme);
            _editingTask = task;

            WireEvents();
            if (task != null)
                LoadTask(task);
            else
                SetDefaults();
        }

        private void WireEvents()
        {
            // 名称输入 + 字数统计
            txtName.TextChanged += (s, e) =>
            {
                int len = txtName.Text.Length;
                lblCharCount.Text = $"{len}/20";
                lblCharCount.ForeColor = len > 20 ? Color.Red : SystemColors.GrayText;
            };

            // 备注输入 + 字数统计
            txtNotes.TextChanged += (s, e) =>
            {
                int len = txtNotes.Text.Length;
                lblNotesCount.Text = $"{len}/200";
            };

            // 专注时间 -- 预设选项
            rdoFocus15.CheckedChanged += (s, e) => { if (rdoFocus15.Checked) { txtFocusCustom.Enabled = false; txtFocusCustom.Text = "15"; } };
            rdoFocus25.CheckedChanged += (s, e) => { if (rdoFocus25.Checked) { txtFocusCustom.Enabled = false; txtFocusCustom.Text = "25"; } };
            rdoFocusCustom.CheckedChanged += (s, e) => { if (rdoFocusCustom.Checked) { txtFocusCustom.Enabled = true; txtFocusCustom.Focus(); } };

            // 休息时间 -- 预设选项
            rdoRest5.CheckedChanged += (s, e) => { if (rdoRest5.Checked) { txtRestCustom.Enabled = false; txtRestCustom.Text = "5"; } };
            rdoRestCustom.CheckedChanged += (s, e) => { if (rdoRestCustom.Checked) { txtRestCustom.Enabled = true; txtRestCustom.Focus(); } };

            // 自定义编辑框获得焦点 -> 自动切到自定义单选
            txtFocusCustom.Enter += (s, e) => { rdoFocusCustom.Checked = true; };
            txtRestCustom.Enter += (s, e) => { rdoRestCustom.Checked = true; };

            // 按钮
            btnConfirm.Click += BtnConfirm_Click;
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        }

        private void SetDefaults()
        {
            Text = "新增待办";
            btnConfirm.Text = "新增";

            // 先切 false 再切 true，确保 CheckedChanged 触发
            rdoFocus25.Checked = false;
            rdoFocus25.Checked = true;
            txtFocusCustom.Text = "25";
            txtFocusCustom.Enabled = false;

            rdoRest5.Checked = false;
            rdoRest5.Checked = true;
            txtRestCustom.Text = "5";
            txtRestCustom.Enabled = false;

            txtRepeat.Text = "1";
        }

        private void LoadTask(PomodoroTask task)
        {
            Text = "编辑待办";
            btnConfirm.Text = "保存";
            txtName.Text = task.Name;

            // 专注时间
            if (task.FocusMinutes == 15)
            {
                rdoFocus15.Checked = false; rdoFocus15.Checked = true;
                txtFocusCustom.Text = "15"; txtFocusCustom.Enabled = false;
            }
            else if (task.FocusMinutes == 25)
            {
                rdoFocus25.Checked = false; rdoFocus25.Checked = true;
                txtFocusCustom.Text = "25"; txtFocusCustom.Enabled = false;
            }
            else
            {
                rdoFocusCustom.Checked = false; rdoFocusCustom.Checked = true;
                txtFocusCustom.Text = task.FocusMinutes.ToString();
                txtFocusCustom.Enabled = true;
            }

            // 休息时间
            if (task.RestMinutes == 5)
            {
                rdoRest5.Checked = false; rdoRest5.Checked = true;
                txtRestCustom.Text = "5"; txtRestCustom.Enabled = false;
            }
            else
            {
                rdoRestCustom.Checked = false; rdoRestCustom.Checked = true;
                txtRestCustom.Text = task.RestMinutes.ToString();
                txtRestCustom.Enabled = true;
            }

            // 执行次数
            txtRepeat.Text = task.RepeatCount.ToString();

            // 备注
            txtNotes.Text = task.Notes;
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            // ---- 校验 ----
            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("事件名称不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }
            if (name.Length > 20)
            {
                MessageBox.Show("事件名称最多 20 个字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (!int.TryParse(txtFocusCustom.Text, out int focusMin) || focusMin <= 0)
            {
                MessageBox.Show("请设置有效的专注时间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFocusCustom.Focus();
                return;
            }

            if (!int.TryParse(txtRestCustom.Text, out int restMin) || restMin < 0)
            {
                restMin = 0;
            }

            if (!int.TryParse(txtRepeat.Text, out int repeat) || repeat <= 0)
            {
                MessageBox.Show("请设置有效的执行次数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRepeat.Focus();
                return;
            }

            // ---- 构建结果 ----
            if (_editingTask != null)
            {
                // 编辑模式 -- 就地修改
                _editingTask.Name = name;
                _editingTask.FocusMinutes = focusMin;
                _editingTask.RestMinutes = restMin;
                _editingTask.RepeatCount = repeat;
                _editingTask.Notes = txtNotes.Text.Trim();
                Result = _editingTask;
            }
            else
            {
                // 新增模式 -- 创建新对象
                Result = new PomodoroTask
                {
                    Name = name,
                    FocusMinutes = focusMin,
                    RestMinutes = restMin,
                    RepeatCount = repeat,
                    Notes = txtNotes.Text.Trim()
                };
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
