namespace WmcSoft.Windows.Forms
{
    partial class UITextEditor
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

        #region Component Designer generated code

        /// <summary>
        /// Required initializeComponent for Designer support - do not modify 
        /// the contents of this initializeComponent with the code editor.
        /// </summary>
        private void InitializeComponent() {
			this.textBox = new System.Windows.Forms.TextBox();
			this.canvas = new System.Windows.Forms.Panel();
			this.button = new System.Windows.Forms.Button();
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// textBox
			// 
			this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", this, "BackColor", true));
			this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox.Location = new System.Drawing.Point(17, 1);
			this.textBox.Margin = new System.Windows.Forms.Padding(1);
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(114, 13);
			this.textBox.TabIndex = 0;
			this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
			// 
			// canvas
			// 
			this.canvas.Location = new System.Drawing.Point(0, 0);
			this.canvas.Margin = new System.Windows.Forms.Padding(0);
			this.canvas.Name = "canvas";
			this.canvas.Size = new System.Drawing.Size(16, 16);
			this.canvas.TabIndex = 1;
			this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
			// 
			// button
			// 
			this.button.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button.Location = new System.Drawing.Point(132, 0);
			this.button.Margin = new System.Windows.Forms.Padding(0);
			this.button.Name = "button";
			this.button.Size = new System.Drawing.Size(18, 17);
			this.button.TabIndex = 2;
			this.button.Text = "…";
			this.button.UseVisualStyleBackColor = true;
			this.button.Click += new System.EventHandler(this.button_Click);
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.AutoSize = true;
			this.tableLayoutPanel.ColumnCount = 3;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.Controls.Add(this.canvas, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.textBox, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.button, 2, 0);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 1;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(150, 17);
			this.tableLayoutPanel.TabIndex = 3;
			// 
			// UITextEditor
			// 
			this.AutoSize = true;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.tableLayoutPanel);
			this.Name = "UITextEditor";
			this.Size = new System.Drawing.Size(150, 20);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Panel canvas;
        private System.Windows.Forms.Button button;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}
