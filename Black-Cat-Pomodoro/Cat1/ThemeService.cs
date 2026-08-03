using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BlackCatPomodoro
{
    public enum AppTheme
    {
        Default,
        Dark
    }

    public static class ThemeService
    {
        // ---- 公开暗色主题色值 ----
        public static readonly Color DarkBackground = Color.FromArgb(32, 32, 32);
        public static readonly Color DarkSurface = Color.FromArgb(45, 45, 48);
        public static readonly Color DarkInput = Color.FromArgb(60, 60, 63);
        public static readonly Color DarkText = Color.FromArgb(240, 240, 240);
        public static readonly Color DarkSubText = Color.FromArgb(160, 160, 160);
        public static readonly Color DarkBorder = Color.FromArgb(80, 80, 85);
        public static readonly Color DarkButtonHover = Color.FromArgb(72, 72, 77);
        public static readonly Color DarkMenuBack = Color.FromArgb(40, 40, 44);
        public static readonly Color DarkMenuHover = Color.FromArgb(62, 62, 66);

        public static AppTheme CurrentTheme { get; private set; } = AppTheme.Default;

        /// <summary>
        /// 将主题应用到整个表单（递归所有子控件）
        /// </summary>
        public static void Apply(Form form, AppTheme theme)
        {
            CurrentTheme = theme;
            ApplyRecursive(form, theme);
        }

        private static void ApplyRecursive(Control control, AppTheme theme)
        {
            if (control == null) return;

            if (theme == AppTheme.Dark)
                ApplyDarkToControl(control);
            else
                ResetControl(control);

            // 递归子控件
            foreach (Control child in control.Controls)
                ApplyRecursive(child, theme);
        }

        // ==================== 暗色模式 ====================
        private static void ApplyDarkToControl(Control control)
        {
            if (control is Form)
            {
                control.BackColor = DarkBackground;
                control.ForeColor = DarkText;
                return;
            }

            if (control is MenuStrip menu)
            {
                menu.BackColor = DarkMenuBack;
                menu.ForeColor = DarkText;
                menu.Renderer = new DarkMenuRenderer();
                return;
            }

            if (control is GroupBox || control is Panel)
            {
                control.BackColor = DarkSurface;
                control.ForeColor = DarkText;
                return;
            }

            if (control is TextBox || control is RichTextBox)
            {
                control.BackColor = DarkInput;
                control.ForeColor = DarkText;
                return;
            }

            if (control is Label || control is LinkLabel)
            {
                control.BackColor = Color.Transparent;
                // 保留原有 ForeColor 如果已被设置（如颜色标签），否则设默认
                if (control.ForeColor == SystemColors.ControlText ||
                    control.ForeColor == SystemColors.GrayText)
                    control.ForeColor = DarkSubText;
                return;
            }

            if (control is ComboBox cb)
            {
                cb.BackColor = DarkInput;
                cb.ForeColor = DarkText;
                cb.FlatStyle = FlatStyle.Flat;
                return;
            }

            if (control is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.BackColor = DarkInput;
                btn.ForeColor = DarkText;
                btn.FlatAppearance.BorderColor = DarkBorder;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.MouseOverBackColor = DarkButtonHover;
                btn.UseVisualStyleBackColor = false;
                return;
            }

            // 默认兜底
            control.BackColor = DarkSurface;
            control.ForeColor = DarkText;
        }

        // ==================== 重置为系统默认 ====================
        private static void ResetControl(Control control)
        {
            if (control is MenuStrip menu)
            {
                menu.BackColor = SystemColors.Control;
                menu.ForeColor = SystemColors.ControlText;
                menu.Renderer = new ToolStripProfessionalRenderer();
                return;
            }

            if (control is Form || control is GroupBox || control is Panel)
            {
                control.BackColor = SystemColors.Control;
                control.ForeColor = SystemColors.ControlText;
                return;
            }

            if (control is TextBox || control is RichTextBox)
            {
                control.BackColor = SystemColors.Window;
                control.ForeColor = SystemColors.WindowText;
                return;
            }

            if (control is Label || control is LinkLabel)
            {
                control.BackColor = Color.Transparent;
                control.ForeColor = SystemColors.ControlText;
                return;
            }

            if (control is ComboBox cb)
            {
                cb.BackColor = SystemColors.Window;
                cb.ForeColor = SystemColors.WindowText;
                cb.FlatStyle = FlatStyle.Standard;
                return;
            }

            if (control is Button btn)
            {
                btn.FlatStyle = FlatStyle.Standard;
                btn.UseVisualStyleBackColor = true;
                btn.ResetBackColor();
                btn.ResetForeColor();
                return;
            }

            control.ResetBackColor();
            control.ResetForeColor();
        }

        // ==================== 暗色菜单渲染器 ====================
        private class DarkMenuRenderer : ToolStripProfessionalRenderer
        {
            public DarkMenuRenderer() : base(new DarkColorTable()) { }

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                if (!e.Item.Selected && !e.Item.Pressed)
                {
                    e.Graphics.FillRectangle(new SolidBrush(DarkMenuBack), e.Item.ContentRectangle);
                    return;
                }
                using (var brush = new SolidBrush(DarkMenuHover))
                    e.Graphics.FillRectangle(brush, e.Item.ContentRectangle);
            }

            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                using (var brush = new SolidBrush(DarkMenuBack))
                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                e.TextColor = DarkText;
                base.OnRenderItemText(e);
            }

            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
            {
                e.ArrowColor = DarkText;
                base.OnRenderArrow(e);
            }

            protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
            {
                using (var pen = new Pen(DarkBorder))
                    e.Graphics.DrawLine(pen, e.Item.ContentRectangle.Left + 2,
                        e.Item.ContentRectangle.Top + e.Item.ContentRectangle.Height / 2,
                        e.Item.ContentRectangle.Right - 2,
                        e.Item.ContentRectangle.Top + e.Item.ContentRectangle.Height / 2);
            }
        }

        private class DarkColorTable : ProfessionalColorTable
        {
            public override Color ToolStripDropDownBackground => DarkMenuBack;
            public override Color MenuBorder => DarkBorder;
            public override Color MenuItemBorder => Color.Transparent;
            public override Color MenuItemSelected => DarkMenuHover;
            public override Color ImageMarginGradientBegin => DarkMenuBack;
            public override Color ImageMarginGradientMiddle => DarkMenuBack;
            public override Color ImageMarginGradientEnd => DarkMenuBack;
            public override Color CheckBackground => DarkMenuHover;
            public override Color CheckPressedBackground => DarkMenuHover;
            public override Color CheckSelectedBackground => DarkMenuHover;
            public override Color SeparatorDark => DarkBorder;
            public override Color SeparatorLight => DarkBorder;
        }
    }
}
