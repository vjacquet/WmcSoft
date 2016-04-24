using WmcSoft.Windows.Forms;

namespace WindowsFormsGallery
{
    partial class FormLayoutForm
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
            this.tracingComponent1 = new WmcSoft.ComponentModel.TracingComponent(this.components);
            this.horizontalFormLayoutPanel1 = new WmcSoft.Windows.Forms.HorizontalFormLayoutPanel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.tracingComponent1)).BeginInit();
            this.horizontalFormLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tracingComponent1
            // 
            this.tracingComponent1.TraceSource = "Controls";
            // 
            // horizontalFormLayoutPanel1
            // 
            this.horizontalFormLayoutPanel1.AutoScroll = true;
            this.horizontalFormLayoutPanel1.Controls.Add(this.textBox1);
            this.horizontalFormLayoutPanel1.Controls.Add(this.textBox2);
            this.horizontalFormLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.horizontalFormLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.horizontalFormLayoutPanel1.Name = "horizontalFormLayoutPanel1";
            this.horizontalFormLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.horizontalFormLayoutPanel1.Size = new System.Drawing.Size(484, 649);
            this.horizontalFormLayoutPanel1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalFormLayoutPanel1.SetCaption(this.textBox1, "textBox1");
            this.textBox1.Location = new System.Drawing.Point(81, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(398, 26);
            this.textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            this.horizontalFormLayoutPanel1.SetCaption(this.textBox2, "textBox2");
            this.textBox2.Location = new System.Drawing.Point(81, 36);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(398, 26);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "text 2";
            // 
            // FormLayoutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 649);
            this.Controls.Add(this.horizontalFormLayoutPanel1);
            this.Name = "FormLayoutForm";
            this.Text = "Form";
            ((System.ComponentModel.ISupportInitialize)(this.tracingComponent1)).EndInit();
            this.horizontalFormLayoutPanel1.ResumeLayout(false);
            this.horizontalFormLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private HorizontalFormLayoutPanel horizontalFormLayoutPanel1;
        private WmcSoft.ComponentModel.TracingComponent tracingComponent1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
    }
}