namespace WmcSoft.Windows.Forms
{
    partial class HeaderedPanel
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

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent() {
            this.headerStrip = new WmcSoft.Windows.Forms.HeaderStrip();
            this.SuspendLayout();
            // 
            // headerStrip
            // 
            this.headerStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.headerStrip.Location = new System.Drawing.Point(0, 0);
            this.headerStrip.Name = "headerStrip";
            this.headerStrip.Size = new System.Drawing.Size(146, 25);
            this.headerStrip.Stretch = true;
            this.headerStrip.TabIndex = 0;
            this.headerStrip.Text = "headerStrip";
            // 
            // HeaderedPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.headerStrip);
            this.Name = "HeaderedPanel";
            this.Size = new System.Drawing.Size(146, 146);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WmcSoft.Windows.Forms.HeaderStrip headerStrip;
    }
}
