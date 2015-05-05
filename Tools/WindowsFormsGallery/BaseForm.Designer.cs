namespace WindowsFormsGallery
{
    partial class BaseForm
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
            this.splitContainer1 = new WmcSoft.Windows.Forms.SplitContainer();
            this.button1 = new System.Windows.Forms.Button();
            this.headeredPanel1 = new WmcSoft.Windows.Forms.HeaderedPanel();
            this.toolStripSpringLabel1 = new WmcSoft.Windows.Forms.ToolStripSpringLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.splitContainer2 = new WmcSoft.Windows.Forms.SplitContainer();
            this.button2 = new System.Windows.Forms.Button();
            this.headeredPanel2 = new WmcSoft.Windows.Forms.HeaderedPanel();
            this.toolStripSpringLabel2 = new WmcSoft.Windows.Forms.ToolStripSpringLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button5 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.headeredPanel1.PlaceHolder.SuspendLayout();
            this.headeredPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.headeredPanel2.PlaceHolder.SuspendLayout();
            this.headeredPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(198, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Size = new System.Drawing.Size(189, 266);
            this.splitContainer1.SplitterDistance = 62;
            this.splitContainer1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 33);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // headeredPanel1
            // 
            this.headeredPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // headeredPanel1.HeaderStrip
            // 
            this.headeredPanel1.HeaderStrip.ClickThrough = false;
            this.headeredPanel1.HeaderStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.headeredPanel1.HeaderStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.headeredPanel1.HeaderStrip.HeaderStyle = WmcSoft.Windows.Forms.HeaderAreaStyle.Small;
            this.headeredPanel1.HeaderStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSpringLabel1});
            this.headeredPanel1.HeaderStrip.Location = new System.Drawing.Point(0, 0);
            this.headeredPanel1.HeaderStrip.Name = "HeaderStrip";
            this.headeredPanel1.HeaderStrip.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.headeredPanel1.HeaderStrip.Size = new System.Drawing.Size(187, 28);
            this.headeredPanel1.HeaderStrip.Stretch = true;
            this.headeredPanel1.HeaderStrip.SuppressHighlighting = false;
            this.headeredPanel1.HeaderStrip.TabIndex = 0;
            this.headeredPanel1.HeaderStrip.Text = "headerStrip";
            this.headeredPanel1.Location = new System.Drawing.Point(4, 5);
            this.headeredPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.headeredPanel1.Name = "headeredPanel1";
            // 
            // headeredPanel1.PlaceHolder
            // 
            this.headeredPanel1.PlaceHolder.Controls.Add(this.linkLabel1);
            this.headeredPanel1.PlaceHolder.Paint += new System.Windows.Forms.PaintEventHandler(this.headeredPanel1_PlaceHolder_Paint);
            this.headeredPanel1.Size = new System.Drawing.Size(187, 262);
            this.headeredPanel1.TabIndex = 0;
            // 
            // toolStripSpringLabel1
            // 
            this.toolStripSpringLabel1.Name = "toolStripSpringLabel1";
            this.toolStripSpringLabel1.Size = new System.Drawing.Size(184, 25);
            this.toolStripSpringLabel1.Text = "toolStripSpringLabel1";
            this.toolStripSpringLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(31, 45);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(80, 20);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "linkLabel1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.headeredPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.headeredPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(586, 544);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(393, 275);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(190, 266);
            this.panel2.TabIndex = 3;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(17, 23);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 33);
            this.button4.TabIndex = 0;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(198, 275);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.button2);
            this.splitContainer2.Size = new System.Drawing.Size(189, 266);
            this.splitContainer2.SplitterDistance = 62;
            this.splitContainer2.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(20, 23);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 33);
            this.button2.TabIndex = 0;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // headeredPanel2
            // 
            this.headeredPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // headeredPanel2.HeaderStrip
            // 
            this.headeredPanel2.HeaderStrip.ClickThrough = false;
            this.headeredPanel2.HeaderStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.headeredPanel2.HeaderStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.headeredPanel2.HeaderStrip.HeaderStyle = WmcSoft.Windows.Forms.HeaderAreaStyle.Small;
            this.headeredPanel2.HeaderStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSpringLabel2});
            this.headeredPanel2.HeaderStrip.Location = new System.Drawing.Point(0, 0);
            this.headeredPanel2.HeaderStrip.Name = "HeaderStrip";
            this.headeredPanel2.HeaderStrip.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.headeredPanel2.HeaderStrip.Size = new System.Drawing.Size(187, 28);
            this.headeredPanel2.HeaderStrip.Stretch = true;
            this.headeredPanel2.HeaderStrip.SuppressHighlighting = false;
            this.headeredPanel2.HeaderStrip.TabIndex = 0;
            this.headeredPanel2.HeaderStrip.Text = "headerStrip";
            this.headeredPanel2.Location = new System.Drawing.Point(4, 277);
            this.headeredPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.headeredPanel2.Name = "headeredPanel2";
            // 
            // headeredPanel2.PlaceHolder
            // 
            this.headeredPanel2.PlaceHolder.Controls.Add(this.linkLabel2);
            this.headeredPanel2.Size = new System.Drawing.Size(187, 262);
            this.headeredPanel2.TabIndex = 0;
            // 
            // toolStripSpringLabel2
            // 
            this.toolStripSpringLabel2.Name = "toolStripSpringLabel2";
            this.toolStripSpringLabel2.Size = new System.Drawing.Size(184, 25);
            this.toolStripSpringLabel2.Text = "toolStripSpringLabel1";
            this.toolStripSpringLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(31, 45);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(80, 20);
            this.linkLabel2.TabIndex = 0;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "linkLabel2";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(393, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(190, 266);
            this.panel1.TabIndex = 2;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(17, 23);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 33);
            this.button3.TabIndex = 0;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(586, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(192, 544);
            this.panel3.TabIndex = 3;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(17, 23);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 33);
            this.button5.TabIndex = 0;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 544);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BaseForm";
            this.Text = "BaseForm";
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.headeredPanel1.PlaceHolder.ResumeLayout(false);
            this.headeredPanel1.PlaceHolder.PerformLayout();
            this.headeredPanel1.ResumeLayout(false);
            this.headeredPanel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.headeredPanel2.PlaceHolder.ResumeLayout(false);
            this.headeredPanel2.PlaceHolder.PerformLayout();
            this.headeredPanel2.ResumeLayout(false);
            this.headeredPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WmcSoft.Windows.Forms.ToolStripSpringLabel toolStripSpringLabel1;
        protected System.Windows.Forms.LinkLabel linkLabel1;
        private WmcSoft.Windows.Forms.HeaderedPanel headeredPanel1;
        protected System.Windows.Forms.Button button1;
        private WmcSoft.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public WmcSoft.Windows.Forms.SplitContainer splitContainer2;
        protected System.Windows.Forms.Button button2;
        public WmcSoft.Windows.Forms.HeaderedPanel headeredPanel2;
        private WmcSoft.Windows.Forms.ToolStripSpringLabel toolStripSpringLabel2;
        protected System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Panel panel1;
        protected System.Windows.Forms.Button button4;
        protected System.Windows.Forms.Button button3;
        public System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Panel panel3;
        protected System.Windows.Forms.Button button5;
    }
}