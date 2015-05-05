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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using WmcSoft.Properties;

namespace WmcSoft.Windows.Forms
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [UIPermission(SecurityAction.Assert, Window = UIPermissionWindow.AllWindows)]
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    [SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public partial class ExceptionForm : Form
    {
        public ExceptionForm(System.Exception exception)
            : this(exception, null) {
        }

        public ExceptionForm(System.Exception exception, string message)
            : this() {
            SuspendLayout();

            Text = MessageForm.ApplicationTitle;

            if (exception is WarningException) {
                messageBoxIcon = MessageBoxIcon.Warning;
            } else if (exception is SecurityException) {
                messageBoxIcon = MessageBoxIcon.Information;
            } else {
                messageBoxIcon = MessageBoxIcon.Error;
            }

            // Set the message
            //
            string msg = exception.Message;
            if (String.IsNullOrEmpty(msg)) {
                msg = exception.GetType().Name;
            } else {
                msg = msg.TrimEnd('.');
            }

            if (!String.IsNullOrEmpty(message))
                msg = String.Format("{0}\r\n\r\n{1}.", message, msg);
            else if (exception is WarningException) {
                WarningException warning = (WarningException)exception;
                msg = String.Format(Resources.ExceptionFormWarningMessage, msg);
            } else if (exception is SecurityException) {
                SecurityException security = (SecurityException)exception;
                string messageFormat = Application.AllowQuit
                    ? Resources.ExceptionFormSecurityErrorMessage
                    : Resources.ExceptionFormSecurityContinueErrorMessage;
                msg = String.Format(messageFormat, exception.GetType().Name, msg);
            } else {
                string messageFormat = Application.AllowQuit
                    ? Resources.ExceptionFormErrorMessage
                    : Resources.ExceptionFormContinueErrorMessage;
                msg = String.Format(messageFormat, msg);
            }
            this.messageTextBox.Text = msg;

            StringBuilder builder = new StringBuilder();
            string eol = "\r\n";
            string sectionSeparatorFormat = Resources.ExceptionFormMessageSectionSeparator;
            builder.AppendFormat(CultureInfo.CurrentCulture, sectionSeparatorFormat, Resources.ExceptionFormExceptionSection);
            builder.Append(exception.ToString());
            builder.Append(eol);
            builder.Append(eol);
            builder.AppendFormat(CultureInfo.CurrentCulture, Resources.ExceptionFormMessageLoadedAssembliesSection);
            new FileIOPermission(PermissionState.Unrestricted).Assert();
            try {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                    AssemblyName name = assembly.GetName();
                    string fileVersion = Resources.NotAvailable;
                    try {
                        if ((name.EscapedCodeBase != null) && (name.EscapedCodeBase.Length > 0)) {
                            Uri uri = new Uri(name.EscapedCodeBase);
                            if (uri.IsFile) {
                                fileVersion = FileVersionInfo.GetVersionInfo(uri.LocalPath).FileVersion;
                            }
                        }
                    }
                    catch (FileNotFoundException) {
                    }
                    builder.AppendFormat(Resources.ExceptionFormMessageAssembliesEntry, name.Name, name.Version, fileVersion, name.EscapedCodeBase);
                    builder.Append(Resources.ExceptionFormMessageSeparator);
                }
            }
            finally {
                CodeAccessPermission.RevertAssert();
            }
            builder.Append(eol);
            builder.Append(eol);

            detailsTextBox.Text = builder.ToString();
            Height -= details.Height; // collapse

            ResumeLayout();
        }

        public ExceptionForm() {
            InitializeComponent();
        }

        private void buttonsPanel_Paint(object sender, PaintEventArgs e) {
            var control = (Control)sender;
            ControlPaint.DrawBorder3D(e.Graphics, control.ClientRectangle, Border3DStyle.Etched, Border3DSide.Top);
        }

        private void detailsButton_Click(object sender, EventArgs e) {
            int height = this.details.Height;
            if (this.details.Visible) {
                height = -height;
            }
            details.Visible = !this.details.Visible;
            detailsButton.ImageIndex = 1 - this.detailsButton.ImageIndex;
            Height += height;
        }

        private void InitializeMessageBoxIcon() {
            icon.Image = MessageForm.GetMessageBoxIcon(messageBoxIcon);
        }

        private void InitializeMessage() {
            Graphics graphics = this.messageTextBox.CreateGraphics();
            Size preferredSize = Size.Ceiling(graphics.MeasureString(messageTextBox.Text, messageTextBox.Font, 560));
            graphics.Dispose();
            preferredSize.Height += 6;
            if (preferredSize.Width < 180) preferredSize.Width = 180;
            if (preferredSize.Height > 360) {
                preferredSize.Height = 360;
                messageTextBox.ScrollBars = ScrollBars.Vertical;
            } else {
                messageTextBox.ScrollBars = ScrollBars.None;
            }

            int dx = preferredSize.Width - messageTextBox.Width;
            int dy = preferredSize.Height - messageTextBox.Height;

            Size size = this.Size;
            if (dx > 0) size.Width += dx;
            if (dy > 0) size.Height += dy;
            Size = size;
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            SuspendLayout();
            InitializeMessageBoxIcon();
            InitializeMessage();
            ResumeLayout(true);

            MessageForm.Beep(messageBoxIcon);
        }

        System.Windows.Forms.MessageBoxIcon messageBoxIcon;
    }
}