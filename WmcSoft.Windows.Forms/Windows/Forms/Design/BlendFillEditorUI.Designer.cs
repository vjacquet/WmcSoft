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

namespace WmcSoft.Windows.Forms.Design
{
    partial class BlendFillEditorUI
    {
        /// <summary>
        /// Required by the Windows Form Designer
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        internal System.Windows.Forms.TabControl mainTab;
        internal System.Windows.Forms.TabPage directionPage;
        internal System.Windows.Forms.TabPage startColorPage;
        internal System.Windows.Forms.TabPage finishColorPage;
        internal System.Windows.Forms.ComboBox directionComboBox;
        internal System.Windows.Forms.ListBox startColorList;
        internal System.Windows.Forms.ListBox finishColorList;
        internal System.Windows.Forms.Panel blendSamplePanel;

        #region Windows Form Designer generated code

        [System.Diagnostics.DebuggerStepThrough]
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BlendFillEditorUI));
            this.mainTab = new System.Windows.Forms.TabControl();
            this.directionPage = new System.Windows.Forms.TabPage();
            this.blendSamplePanel = new System.Windows.Forms.Panel();
            this.directionComboBox = new System.Windows.Forms.ComboBox();
            this.startColorPage = new System.Windows.Forms.TabPage();
            this.startColorList = new System.Windows.Forms.ListBox();
            this.finishColorPage = new System.Windows.Forms.TabPage();
            this.finishColorList = new System.Windows.Forms.ListBox();
            this.mainTab.SuspendLayout();
            this.directionPage.SuspendLayout();
            this.startColorPage.SuspendLayout();
            this.finishColorPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.directionPage);
            this.mainTab.Controls.Add(this.startColorPage);
            this.mainTab.Controls.Add(this.finishColorPage);
            resources.ApplyResources(this.mainTab, "mainTab");
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            // 
            // directionPage
            // 
            this.directionPage.Controls.Add(this.blendSamplePanel);
            this.directionPage.Controls.Add(this.directionComboBox);
            resources.ApplyResources(this.directionPage, "directionPage");
            this.directionPage.Name = "directionPage";
            // 
            // blendSamplePanel
            // 
            resources.ApplyResources(this.blendSamplePanel, "blendSamplePanel");
            this.blendSamplePanel.Name = "blendSamplePanel";
            this.blendSamplePanel.DoubleClick += new System.EventHandler(this.RequestCloseDropDown);
            // 
            // directionComboBox
            // 
            this.directionComboBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.directionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.directionComboBox, "directionComboBox");
            this.directionComboBox.Name = "directionComboBox";
            // 
            // startColorPage
            // 
            this.startColorPage.Controls.Add(this.startColorList);
            resources.ApplyResources(this.startColorPage, "startColorPage");
            this.startColorPage.Name = "startColorPage";
            // 
            // startColorList
            // 
            resources.ApplyResources(this.startColorList, "startColorList");
            this.startColorList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.startColorList.Name = "startColorList";
            this.startColorList.DoubleClick += new System.EventHandler(this.RequestCloseDropDown);
            // 
            // finishColorPage
            // 
            this.finishColorPage.Controls.Add(this.finishColorList);
            resources.ApplyResources(this.finishColorPage, "finishColorPage");
            this.finishColorPage.Name = "finishColorPage";
            // 
            // finishColorList
            // 
            resources.ApplyResources(this.finishColorList, "finishColorList");
            this.finishColorList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.finishColorList.Name = "finishColorList";
            this.finishColorList.DoubleClick += new System.EventHandler(this.RequestCloseDropDown);
            // 
            // BlendFillEditorUI
            // 
            this.Controls.Add(this.mainTab);
            resources.ApplyResources(this, "$this");
            this.Name = "BlendFillEditorUI";
            this.mainTab.ResumeLayout(false);
            this.directionPage.ResumeLayout(false);
            this.startColorPage.ResumeLayout(false);
            this.finishColorPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}


