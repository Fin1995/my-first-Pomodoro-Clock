using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlackCatPomodoro
{
    partial class TaskEditForm
    {
        private System.ComponentModel.IContainer components = null;
        private GroupBox grpName;
        private TextBox txtName;
        private Label lblCharCount;
        private GroupBox grpFocus;
        private RadioButton rdoFocus25;
        private RadioButton rdoFocus15;
        private RadioButton rdoFocusCustom;
        private TextBox txtFocusCustom;
        private Label lblFocusUnit;
        private GroupBox grpRest;
        private RadioButton rdoRest5;
        private RadioButton rdoRestCustom;
        private TextBox txtRestCustom;
        private Label lblRestUnit;
        private GroupBox grpAdvanced;
        private Label lblRepeat;
        private TextBox txtRepeat;
        private Label lblRepeatUnit;
        private GroupBox grpNotes;
        private TextBox txtNotes;
        private Label lblNotesCount;
        private Button btnConfirm;
        private Button btnCancel;
        private Label lblHint;

        private PomodoroTask _editingTask;
        private string _notesData = string.Empty;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.grpName = new GroupBox();
            this.txtName = new TextBox();
            this.lblCharCount = new Label();
            this.grpFocus = new GroupBox();
            this.rdoFocus25 = new RadioButton();
            this.rdoFocus15 = new RadioButton();
            this.rdoFocusCustom = new RadioButton();
            this.txtFocusCustom = new TextBox();
            this.lblFocusUnit = new Label();
            this.grpRest = new GroupBox();
            this.rdoRest5 = new RadioButton();
            this.rdoRestCustom = new RadioButton();
            this.txtRestCustom = new TextBox();
            this.lblRestUnit = new Label();
            this.grpAdvanced = new GroupBox();
            this.lblRepeat = new Label();
            this.txtRepeat = new TextBox();
            this.lblRepeatUnit = new Label();
            this.grpNotes = new GroupBox();
            this.txtNotes = new TextBox();
            this.lblNotesCount = new Label();
            this.btnConfirm = new Button();
            this.btnCancel = new Button();
            this.lblHint = new Label();

            this.grpName.SuspendLayout();
            this.grpFocus.SuspendLayout();
            this.grpRest.SuspendLayout();
            this.grpAdvanced.SuspendLayout();
            this.grpNotes.SuspendLayout();
            this.SuspendLayout();

            // ---- Form ----
            this.AutoScaleDimensions = new SizeF(6F, 12F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(400, 525);
            this.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "新增待办";

            // ---- grpName ----
            this.grpName.Controls.Add(this.txtName);
            this.grpName.Controls.Add(this.lblCharCount);
            this.grpName.Location = new Point(12, 8);
            this.grpName.Size = new Size(376, 52);
            this.grpName.Text = "事件名称";
            // txtName
            this.txtName.Location = new Point(10, 20);
            this.txtName.Size = new Size(290, 23);
            this.txtName.MaxLength = 20;
            // lblCharCount
            this.lblCharCount.Location = new Point(306, 23);
            this.lblCharCount.Size = new Size(60, 18);
            this.lblCharCount.Text = "0/20";
            this.lblCharCount.TextAlign = ContentAlignment.MiddleRight;
            this.lblCharCount.ForeColor = SystemColors.GrayText;

            // ---- grpFocus ----
            this.grpFocus.Controls.Add(this.rdoFocus25);
            this.grpFocus.Controls.Add(this.rdoFocus15);
            this.grpFocus.Controls.Add(this.rdoFocusCustom);
            this.grpFocus.Controls.Add(this.txtFocusCustom);
            this.grpFocus.Controls.Add(this.lblFocusUnit);
            this.grpFocus.Location = new Point(12, 66);
            this.grpFocus.Size = new Size(376, 85);
            this.grpFocus.Text = "专注时间";
            // rdoFocus15
            this.rdoFocus15.Location = new Point(10, 22);
            this.rdoFocus15.Size = new Size(70, 20);
            this.rdoFocus15.Text = "15 分钟";
            // rdoFocus25
            this.rdoFocus25.Location = new Point(85, 22);
            this.rdoFocus25.Size = new Size(70, 20);
            this.rdoFocus25.Text = "25 分钟";
            this.rdoFocus25.Checked = true;
            // rdoFocusCustom
            this.rdoFocusCustom.Location = new Point(10, 50);
            this.rdoFocusCustom.Size = new Size(65, 20);
            this.rdoFocusCustom.Text = "自定义";
            // txtFocusCustom
            this.txtFocusCustom.Location = new Point(80, 49);
            this.txtFocusCustom.Size = new Size(55, 23);
            this.txtFocusCustom.Enabled = false;
            // lblFocusUnit
            this.lblFocusUnit.Location = new Point(138, 52);
            this.lblFocusUnit.Size = new Size(30, 18);
            this.lblFocusUnit.Text = "分钟";

            // ---- grpRest ----
            this.grpRest.Controls.Add(this.rdoRest5);
            this.grpRest.Controls.Add(this.rdoRestCustom);
            this.grpRest.Controls.Add(this.txtRestCustom);
            this.grpRest.Controls.Add(this.lblRestUnit);
            this.grpRest.Location = new Point(12, 157);
            this.grpRest.Size = new Size(376, 80);
            this.grpRest.Text = "休息时间";
            // rdoRest5
            this.rdoRest5.Location = new Point(10, 22);
            this.rdoRest5.Size = new Size(65, 20);
            this.rdoRest5.Text = "5 分钟";
            this.rdoRest5.Checked = true;
            // rdoRestCustom
            this.rdoRestCustom.Location = new Point(10, 48);
            this.rdoRestCustom.Size = new Size(65, 20);
            this.rdoRestCustom.Text = "自定义";
            // txtRestCustom
            this.txtRestCustom.Location = new Point(80, 47);
            this.txtRestCustom.Size = new Size(55, 23);
            this.txtRestCustom.Enabled = false;
            // lblRestUnit
            this.lblRestUnit.Location = new Point(138, 50);
            this.lblRestUnit.Size = new Size(30, 18);
            this.lblRestUnit.Text = "分钟";

            // ---- grpAdvanced ----
            this.grpAdvanced.Controls.Add(this.lblRepeat);
            this.grpAdvanced.Controls.Add(this.txtRepeat);
            this.grpAdvanced.Controls.Add(this.lblRepeatUnit);
            this.grpAdvanced.Location = new Point(12, 243);
            this.grpAdvanced.Size = new Size(376, 52);
            this.grpAdvanced.Text = "高级设置";
            // lblRepeat
            this.lblRepeat.Location = new Point(10, 22);
            this.lblRepeat.Size = new Size(55, 18);
            this.lblRepeat.Text = "执行次数";
            // txtRepeat
            this.txtRepeat.Location = new Point(70, 19);
            this.txtRepeat.Size = new Size(55, 23);
            this.txtRepeat.Text = "1";
            // lblRepeatUnit
            this.lblRepeatUnit.Location = new Point(128, 22);
            this.lblRepeatUnit.Size = new Size(20, 18);
            this.lblRepeatUnit.Text = "次";

            // ---- grpNotes ----
            this.grpNotes.Controls.Add(this.txtNotes);
            this.grpNotes.Controls.Add(this.lblNotesCount);
            this.grpNotes.Location = new Point(12, 301);
            this.grpNotes.Size = new Size(376, 110);
            this.grpNotes.Text = "任务备注";
            // txtNotes
            this.txtNotes.Location = new Point(10, 20);
            this.txtNotes.Size = new Size(356, 60);
            this.txtNotes.Multiline = true;
            this.txtNotes.ScrollBars = ScrollBars.Vertical;
            this.txtNotes.MaxLength = 200;
            // lblNotesCount
            this.lblNotesCount.Location = new Point(306, 84);
            this.lblNotesCount.Size = new Size(60, 18);
            this.lblNotesCount.Text = "0/200";
            this.lblNotesCount.TextAlign = ContentAlignment.MiddleRight;
            this.lblNotesCount.ForeColor = SystemColors.GrayText;

            // ---- lblHint ----
            this.lblHint.Location = new Point(12, 418);
            this.lblHint.Size = new Size(376, 18);
            this.lblHint.ForeColor = SystemColors.GrayText;
            this.lblHint.Text = "* 名称和专注时间为必填项";

            // ---- Buttons ----
            this.btnConfirm.Location = new Point(210, 445);
            this.btnConfirm.Size = new Size(85, 30);
            this.btnConfirm.Text = "确认";
            this.btnCancel.Location = new Point(303, 445);
            this.btnCancel.Size = new Size(85, 30);
            this.btnCancel.Text = "取消";

            // ---- Add controls ----
            this.Controls.Add(this.grpName);
            this.Controls.Add(this.grpFocus);
            this.Controls.Add(this.grpRest);
            this.Controls.Add(this.grpAdvanced);
            this.Controls.Add(this.grpNotes);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnCancel);
            this.AcceptButton = this.btnConfirm;
            this.CancelButton = this.btnCancel;

            this.grpName.ResumeLayout(false);
            this.grpName.PerformLayout();
            this.grpFocus.ResumeLayout(false);
            this.grpFocus.PerformLayout();
            this.grpRest.ResumeLayout(false);
            this.grpRest.PerformLayout();
            this.grpAdvanced.ResumeLayout(false);
            this.grpAdvanced.PerformLayout();
            this.grpNotes.ResumeLayout(false);
            this.grpNotes.PerformLayout();
            this.ResumeLayout(false);
        }

        // ---- Event wirings done in .cs ----
    }
}
