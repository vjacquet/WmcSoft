namespace WmcSoft.Windows.Forms
{
    partial class TipOfTheDay
    {

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            this.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.startupTimer = new System.Windows.Forms.Timer(this.components);
            // 
            // startupTimer
            // 
            this.startupTimer.Tick += new System.EventHandler(this.OnStartup);

        }

        #endregion

        private System.Windows.Forms.Timer startupTimer;

    }
}