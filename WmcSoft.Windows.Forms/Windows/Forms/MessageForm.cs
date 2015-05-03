#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************/

#endregion

using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    public partial class MessageForm : Form
    {
        #region Lifecycle

        public MessageForm() {
            SuspendLayout();

            InitializeComponent();
            Height -= details.Height; // collapse

            ResumeLayout();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            SuspendLayout();

            InitializeCaption();
            InitializeMessageBoxIcon();
            InitializeMessageBoxButtons();
            InitializeMessage();
            InitializeMessageBoxOptions();
            InitializeDetails();

            ResumeLayout();

            Beep(MessageBoxIcon);
        }

        private void detailsButton_Click(object sender, EventArgs e) {
            int height = this.details.Height;
            if (details.Visible) {
                height = -height;
            }
            details.Visible = !this.details.Visible;
            detailsButton.ImageIndex = 1 - this.detailsButton.ImageIndex;
            base.Height += height;
        }

        #endregion

        #region Show helper methods

        public static DialogResult ShowMessage(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon) {
            using (MessageForm form = new MessageForm()) {
                form.Message = message;
                form.Text = caption;
                form.MessageBoxIcon = icon;
                form.MessageBoxButtons = buttons;
                form.StartPosition = FormStartPosition.CenterScreen;
                return form.ShowDialog(null);
            }
        }

        #endregion

        #region Paint

        private void buttonsPanel_Paint(object sender, PaintEventArgs e) {
            Control control = (Control)sender;
            ControlPaint.DrawBorder3D(e.Graphics, control.ClientRectangle, Border3DStyle.Etched, Border3DSide.Top);
        }

        #endregion

        #region Properties

        public string Message {
            get { return messageTextBox.Text; }
            set { messageTextBox.Text = value; }
        }

        public string Details {
            get { return detailsTextBox.Text; }
            set { detailsTextBox.Text = value; }
        }

        public MessageBoxIcon MessageBoxIcon { get; set; }

        public MessageBoxButtons MessageBoxButtons { get; set; }

        public MessageBoxDefaultButton MessageBoxDefaultButton { get; set; }

        public MessageBoxOptions MessageBoxOptions { get; set; }

        #endregion

        #region Initializations

        private void InitializeMessageBoxIcon() {
            icon.Image = GetMessageBoxIcon(MessageBoxIcon);
        }

        private void InitializeMessageBoxButtons() {
            var buttons = new Button[] {
                uiButton1,
                uiButton2,
                uiButton3,
            };
            int count = 0;
            int defaultButton = 0;

            switch (MessageBoxButtons) {
            case MessageBoxButtons.AbortRetryIgnore:
                ConfigureButton(buttons[0], DialogResult.Abort);
                ConfigureButton(buttons[1], DialogResult.Retry);
                ConfigureButton(buttons[2], DialogResult.Ignore);
                this.CancelButton = buttons[2];
                count = 3;
                break;
            case MessageBoxButtons.OK:
                ConfigureButton(buttons[0], DialogResult.OK);
                count = 1;
                break;
            case MessageBoxButtons.OKCancel:
                ConfigureButton(buttons[0], DialogResult.OK);
                ConfigureButton(buttons[1], DialogResult.Cancel);
                count = 2;
                break;
            case MessageBoxButtons.RetryCancel:
                ConfigureButton(buttons[0], DialogResult.Retry);
                ConfigureButton(buttons[1], DialogResult.Cancel);
                this.CancelButton = buttons[1];
                count = 2;
                break;
            case MessageBoxButtons.YesNo:
                ConfigureButton(buttons[0], DialogResult.Yes);
                ConfigureButton(buttons[1], DialogResult.No);
                this.CancelButton = buttons[1];
                count = 2;
                break;
            case MessageBoxButtons.YesNoCancel:
                ConfigureButton(buttons[0], DialogResult.Yes);
                ConfigureButton(buttons[1], DialogResult.No);
                ConfigureButton(buttons[2], DialogResult.Cancel);
                this.CancelButton = buttons[2];
                count = 3;
                break;
            }

            switch (MessageBoxDefaultButton) {
            case MessageBoxDefaultButton.Button1:
                defaultButton = 0;
                break;
            case MessageBoxDefaultButton.Button2:
                defaultButton = 1;
                break;
            case MessageBoxDefaultButton.Button3:
                defaultButton = 2;
                break;
            }
            if (defaultButton >= count)
                defaultButton = count - 1;
            AcceptButton = buttons[defaultButton];
            for (int i = count; i != buttons.Length; i++) {
                var control = buttons[i] as Control;
                if (control != null) {
                    control.Visible = false;
                }
            }
        }

        private void InitializeMessage() {
            Graphics graphics = this.messageTextBox.CreateGraphics();
            Size preferredSize = Size.Ceiling(graphics.MeasureString(messageTextBox.Text, messageTextBox.Font, 560));
            graphics.Dispose();
            preferredSize.Height += 6;
            if (preferredSize.Width < 180)
                preferredSize.Width = 180;
            if (preferredSize.Height > 360) {
                preferredSize.Height = 360;
                messageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            } else {
                messageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            }

            int dx = preferredSize.Width - messageTextBox.Width;
            int dy = preferredSize.Height - messageTextBox.Height;

            Size size = this.Size;
            if (dx > 0)
                size.Width += dx;
            if (dy > 0)
                size.Height += dy;
            this.Size = size;
        }

        private void InitializeCaption() {
            if (!String.IsNullOrEmpty(this.Text))
                return;

            switch (MessageBoxIcon) {
            case MessageBoxIcon.Information:
                this.Text = Properties.Resources.InformationCaption;
                break;
            case MessageBoxIcon.Error:
                this.Text = Properties.Resources.ErrorCaption;
                break;
            case MessageBoxIcon.Warning:
                this.Text = Properties.Resources.WarningCaption;
                break;
            case MessageBoxIcon.Question:
                this.Text = Properties.Resources.QuestionCaption;
                break;
            }
            if (String.IsNullOrEmpty(this.Text)) {
                // get the caption of the main window
                Form activeForm = Form.ActiveForm;
                if (activeForm == null) {
                    this.Text = activeForm.Text;
                }
            }
            if (String.IsNullOrEmpty(this.Text)) {
                this.Text = ApplicationTitle;
            }
        }

        private void InitializeMessageBoxOptions() {
            if (MessageBoxOptions.RtlReading == (MessageBoxOptions & MessageBoxOptions.RtlReading)) {
                messageTextBox.RightToLeft = RightToLeft.Yes;
                buttonsPanel.FlowDirection = FlowDirection.LeftToRight;
            }

            if (MessageBoxOptions.RightAlign == (MessageBoxOptions & MessageBoxOptions.RightAlign)) {
                if (messageTextBox.RightToLeft == RightToLeft.Yes)
                    messageTextBox.RightToLeft = RightToLeft.No;
                else
                    messageTextBox.RightToLeft = RightToLeft.Yes;
            }
        }

        private void InitializeDetails() {
            this.detailsButton.Visible = !String.IsNullOrEmpty(this.detailsTextBox.Text);
        }

        private IButtonControl ConfigureButton(Button button, DialogResult dialogResult) {
            switch (dialogResult) {
            case DialogResult.Abort:
                button.Text = Properties.Resources.AbortButton;
                break;
            case DialogResult.Cancel:
                button.Text = Properties.Resources.CancelButton;
                break;
            case DialogResult.Ignore:
                button.Text = Properties.Resources.IgnoreButton;
                break;
            case DialogResult.No:
                button.Text = Properties.Resources.NoButton;
                break;
            case DialogResult.OK:
                button.Text = MessageBoxButtons == (MessageBoxButtons.OK)
                    ? Properties.Resources.CloseButton
                    : Properties.Resources.OKButton;
                break;
            case DialogResult.Retry:
                button.Text = Properties.Resources.RetryButton;
                break;
            case DialogResult.Yes:
                button.Text = Properties.Resources.YesButton;
                break;
            }
            button.DialogResult = dialogResult;
            return button;
        }

        #endregion

        #region Beep

        [System.Runtime.InteropServices.DllImport("User32.dll", ExactSpelling = true, EntryPoint = "MessageBeep")]
        static extern bool ApiMessageBeep(uint type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageBoxIcon"></param>
        public static void Beep(System.Windows.Forms.MessageBoxIcon messageBoxIcon) {
            if (messageBoxIcon != MessageBoxIcon.None) {
                ApiMessageBeep((uint)messageBoxIcon);
            }
        }

        #endregion

        #region Associated Image

        public static Image GetMessageBoxIcon(System.Windows.Forms.MessageBoxIcon messageBoxIcon) {
            switch (messageBoxIcon) {
            case MessageBoxIcon.Information:
                return Properties.Resources.Info48;
            case MessageBoxIcon.Error:
                return Properties.Resources.Error48;
            case MessageBoxIcon.Warning:
                return Properties.Resources.Warning48;
            case MessageBoxIcon.Question:
                return Properties.Resources.Question48;
            case MessageBoxIcon.None:
            default:
                return null;
            }
        }

        #endregion

        #region Application

        public static string ApplicationTitle {
            get {
                var assembly = Assembly.GetEntryAssembly();
                var titleAttribute = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
                if (titleAttribute != null && !String.IsNullOrEmpty(titleAttribute.Title))
                    return titleAttribute.Title;
                return Path.GetFileNameWithoutExtension(assembly.CodeBase);
            }
        }

        #endregion
    }
}