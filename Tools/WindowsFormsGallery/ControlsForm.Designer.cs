﻿namespace WindowsFormsGallery
{
    partial class ControlsForm
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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.deckLayoutPanel1 = new WmcSoft.Windows.Forms.DeckLayoutPanel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tracingComponent1 = new WmcSoft.ComponentModel.TracingComponent(this.components);
            this.uiService1 = new WmcSoft.Windows.Forms.UIService(this.components);
            this.uiTextEditor1 = new WmcSoft.Windows.Forms.UITextEditor(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.deckLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tracingComponent1)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(69, 75);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(80, 20);
            this.numericUpDown1.TabIndex = 0;
            // 
            // deckLayoutPanel1
            // 
            this.deckLayoutPanel1.ActiveControl = this.richTextBox1;
            this.deckLayoutPanel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.deckLayoutPanel1.Controls.Add(this.richTextBox1);
            this.deckLayoutPanel1.Controls.Add(this.listBox1);
            this.deckLayoutPanel1.Location = new System.Drawing.Point(199, 31);
            this.deckLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.deckLayoutPanel1.Name = "deckLayoutPanel1";
            this.deckLayoutPanel1.Size = new System.Drawing.Size(152, 216);
            this.deckLayoutPanel1.TabIndex = 1;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(152, 216);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "Root\n\tItem 1\n\t\tItem 1.1\n\tItem 2\n\t\tItem 2.1\n\t\tItem 2.2";
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(153, 217);
            this.listBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Location = new System.Drawing.Point(410, 97);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(53, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // uiService1
            // 
            this.uiService1.ContainerControl = this;
            // 
            // uiTextEditor1
            // 
            this.uiTextEditor1.AutoSize = true;
            this.uiTextEditor1.BackColor = System.Drawing.SystemColors.Window;
            this.uiTextEditor1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uiTextEditor1.Location = new System.Drawing.Point(199, 251);
            this.uiTextEditor1.Margin = new System.Windows.Forms.Padding(2);
            this.uiTextEditor1.Name = "uiTextEditor1";
            this.uiTextEditor1.Size = new System.Drawing.Size(100, 20);
            this.uiTextEditor1.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(357, 251);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 4;
            // 
            // ControlsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 354);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.uiTextEditor1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.deckLayoutPanel1);
            this.Controls.Add(this.numericUpDown1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ControlsForm";
            this.Text = "ControlsForm";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.deckLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tracingComponent1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private WmcSoft.Windows.Forms.DeckLayoutPanel deckLayoutPanel1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private WmcSoft.ComponentModel.TracingComponent tracingComponent1;
        private WmcSoft.Windows.Forms.UIService uiService1;
        private WmcSoft.Windows.Forms.UITextEditor uiTextEditor1;
        private System.Windows.Forms.TextBox textBox1;
    }
}