namespace WmcSoft.CommandLine
{
    partial class BenchComponent
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            WmcSoft.CommandLine.BooleanOption booleanOption1;
            WmcSoft.CommandLine.ChoiceOption choiceOption1;
            WmcSoft.CommandLine.ChoiceOption.Choice choice1 = new WmcSoft.CommandLine.ChoiceOption.Choice();
            WmcSoft.CommandLine.ChoiceOption.Choice choice2 = new WmcSoft.CommandLine.ChoiceOption.Choice();
            WmcSoft.CommandLine.ListOption listOption1;
            WmcSoft.CommandLine.StringOption stringOption1;
            WmcSoft.CommandLine.SwitchOption switchOption1;
            this.commandLine = new WmcSoft.CommandLine.CommandLine();
            booleanOption1 = new WmcSoft.CommandLine.BooleanOption("boolean");
            choiceOption1 = new WmcSoft.CommandLine.ChoiceOption("choice");
            listOption1 = new WmcSoft.CommandLine.ListOption("list");
            stringOption1 = new WmcSoft.CommandLine.StringOption("string");
            switchOption1 = new WmcSoft.CommandLine.SwitchOption("switch");
            // 
            // choiceOption1
            // 
            choice1.Name = "A";
            choice2.Name = "B";
            choiceOption1.Choices.Add(choice1);
            choiceOption1.Choices.Add(choice2);
            choiceOption1.Template = "xxxx";
            // 
            // listOption1
            // 
            listOption1.Template = "xxxx";
            // 
            // stringOption1
            // 
            stringOption1.Template = "xxxx";
            // 
            // commandLine
            // 
            this.commandLine.Options.AddRange(new WmcSoft.CommandLine.Option[] {
            booleanOption1,
            choiceOption1,
            listOption1,
            stringOption1,
            switchOption1});
            this.commandLine.Owner = null;
            this.commandLine.Usage = null;

        }

        #endregion

        public CommandLine commandLine;
    }
}
