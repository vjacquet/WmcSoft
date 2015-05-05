namespace WmcSoft.Windows.Forms
{
    partial class MessageForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageForm));
            this.icon = new System.Windows.Forms.PictureBox();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.detailsButton = new System.Windows.Forms.Button();
            this.uiButton3 = new System.Windows.Forms.Button();
            this.uiButton2 = new System.Windows.Forms.Button();
            this.uiButton1 = new System.Windows.Forms.Button();
            this.buttonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.details = new System.Windows.Forms.Panel();
            this.detailsTextBox = new System.Windows.Forms.TextBox();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.bodyPanel = new System.Windows.Forms.Panel();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
            this.buttonsPanel.SuspendLayout();
            this.details.SuspendLayout();
            this.controlPanel.SuspendLayout();
            this.bodyPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // icon
            // 
            this.icon.Location = new System.Drawing.Point(3, 3);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(54, 54);
            this.icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.icon.TabIndex = 0;
            this.icon.TabStop = false;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "down");
            this.imageList.Images.SetKeyName(1, "up");
            // 
            // detailsButton
            // 
            this.detailsButton.ImageIndex = 0;
            this.detailsButton.ImageList = this.imageList;
            this.detailsButton.Location = new System.Drawing.Point(5, 9);
            this.detailsButton.Name = "detailsButton";
            this.detailsButton.Size = new System.Drawing.Size(22, 22);
            this.detailsButton.TabIndex = 1;
            this.detailsButton.Visible = false;
            this.detailsButton.Click += new System.EventHandler(this.detailsButton_Click);
            // 
            // uiButton3
            // 
            this.uiButton3.Location = new System.Drawing.Point(182, 3);
            this.uiButton3.Name = "uiButton3";
            this.uiButton3.Size = new System.Drawing.Size(75, 23);
            this.uiButton3.TabIndex = 2;
            this.uiButton3.Text = "uiButton3";
            // 
            // uiButton2
            // 
            this.uiButton2.Location = new System.Drawing.Point(101, 3);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(75, 23);
            this.uiButton2.TabIndex = 1;
            this.uiButton2.Text = "uiButton2";
            // 
            // uiButton1
            // 
            this.uiButton1.Location = new System.Drawing.Point(20, 3);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(75, 23);
            this.uiButton1.TabIndex = 0;
            this.uiButton1.Text = "uiButton1";
            // 
            // buttonsPanel
            // 
            this.buttonsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonsPanel.BackColor = System.Drawing.Color.Transparent;
            this.buttonsPanel.Controls.Add(this.uiButton3);
            this.buttonsPanel.Controls.Add(this.uiButton2);
            this.buttonsPanel.Controls.Add(this.uiButton1);
            this.buttonsPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.buttonsPanel.Location = new System.Drawing.Point(51, 5);
            this.buttonsPanel.Name = "buttonsPanel";
            this.buttonsPanel.Size = new System.Drawing.Size(260, 30);
            this.buttonsPanel.TabIndex = 0;
            // 
            // details
            // 
            this.details.Controls.Add(this.detailsTextBox);
            this.details.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.details.Location = new System.Drawing.Point(0, 108);
            this.details.Name = "details";
            this.details.Padding = new System.Windows.Forms.Padding(6);
            this.details.Size = new System.Drawing.Size(314, 180);
            this.details.TabIndex = 0;
            this.details.Visible = false;
            this.details.Paint += new System.Windows.Forms.PaintEventHandler(this.buttonsPanel_Paint);
            // 
            // detailsTextBox
            // 
            this.detailsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailsTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.detailsTextBox.Location = new System.Drawing.Point(6, 6);
            this.detailsTextBox.Multiline = true;
            this.detailsTextBox.Name = "detailsTextBox";
            this.detailsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.detailsTextBox.Size = new System.Drawing.Size(302, 168);
            this.detailsTextBox.ReadOnly = true;
            this.detailsTextBox.TabIndex = 0;
            // 
            // controlPanel
            // 
            this.controlPanel.Controls.Add(this.buttonsPanel);
            this.controlPanel.Controls.Add(this.detailsButton);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.controlPanel.Location = new System.Drawing.Point(0, 68);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(314, 40);
            this.controlPanel.TabIndex = 0;
            this.controlPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.buttonsPanel_Paint);
            // 
            // bodyPanel
            // 
            this.bodyPanel.BackColor = System.Drawing.SystemColors.Window;
            this.bodyPanel.Controls.Add(this.messageTextBox);
            this.bodyPanel.Controls.Add(this.icon);
            this.bodyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bodyPanel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.bodyPanel.Location = new System.Drawing.Point(0, 0);
            this.bodyPanel.Name = "bodyPanel";
            this.bodyPanel.Size = new System.Drawing.Size(314, 68);
            this.bodyPanel.TabIndex = 2;
            // 
            // messageTextBox
            // 
            this.messageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.messageTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.messageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageTextBox.Location = new System.Drawing.Point(68, 8);
            this.messageTextBox.Margin = new System.Windows.Forms.Padding(8);
            this.messageTextBox.Multiline = true;
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.Size = new System.Drawing.Size(235, 52);
            this.messageTextBox.TabIndex = 0;
            // 
            // MessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 288);
            this.Controls.Add(this.bodyPanel);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.details);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageForm";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            this.buttonsPanel.ResumeLayout(false);
            this.details.ResumeLayout(false);
            this.details.PerformLayout();
            this.controlPanel.ResumeLayout(false);
            this.bodyPanel.ResumeLayout(false);
            this.bodyPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox icon;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Button detailsButton;
        private System.Windows.Forms.Button uiButton3;
        private System.Windows.Forms.Button uiButton2;
        private System.Windows.Forms.Button uiButton1;
        private System.Windows.Forms.FlowLayoutPanel buttonsPanel;
        private System.Windows.Forms.Panel details;
        private System.Windows.Forms.TextBox detailsTextBox;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Panel bodyPanel;
        private System.Windows.Forms.TextBox messageTextBox;

    }
}