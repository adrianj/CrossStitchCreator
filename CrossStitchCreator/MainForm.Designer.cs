namespace CrossStitchCreator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveOutputImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savePatternImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveOutputImageAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savePatternImageAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBoxOriginal = new System.Windows.Forms.PictureBox();
            this.pictureBoxNew = new System.Windows.Forms.PictureBox();
            this.interpCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.widthUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.heightUpDown = new System.Windows.Forms.NumericUpDown();
            this.fixRatioCheck = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.maxColoursUpDown = new System.Windows.Forms.NumericUpDown();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.editButton = new System.Windows.Forms.Button();
            this.zoomablePictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxOutput = new System.Windows.Forms.PictureBox();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxColoursUpDown)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zoomablePictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOutput)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // contextMenu
            // 
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1301, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.openProjectToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.openToolStripMenuItem.Text = "Open Image File...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            this.openProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.openProjectToolStripMenuItem.Text = "Open Project...";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveProjectToolStripMenuItem,
            this.saveOutputImageToolStripMenuItem,
            this.savePatternImageToolStripMenuItem});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.saveToolStripMenuItem.Text = "Save...";
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.saveProjectToolStripMenuItem.Text = "Save Project...";
            // 
            // saveOutputImageToolStripMenuItem
            // 
            this.saveOutputImageToolStripMenuItem.Name = "saveOutputImageToolStripMenuItem";
            this.saveOutputImageToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.saveOutputImageToolStripMenuItem.Text = "Save Output Image...";
            this.saveOutputImageToolStripMenuItem.Click += new System.EventHandler(this.saveOutputImage);
            // 
            // savePatternImageToolStripMenuItem
            // 
            this.savePatternImageToolStripMenuItem.Name = "savePatternImageToolStripMenuItem";
            this.savePatternImageToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.savePatternImageToolStripMenuItem.Text = "Save Pattern Image...";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveProjectAsToolStripMenuItem,
            this.saveOutputImageAsToolStripMenuItem,
            this.savePatternImageAsToolStripMenuItem});
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            // 
            // saveProjectAsToolStripMenuItem
            // 
            this.saveProjectAsToolStripMenuItem.Name = "saveProjectAsToolStripMenuItem";
            this.saveProjectAsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.saveProjectAsToolStripMenuItem.Text = "Save Project As...";
            // 
            // saveOutputImageAsToolStripMenuItem
            // 
            this.saveOutputImageAsToolStripMenuItem.Name = "saveOutputImageAsToolStripMenuItem";
            this.saveOutputImageAsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.saveOutputImageAsToolStripMenuItem.Text = "Save Output Image As...";
            this.saveOutputImageAsToolStripMenuItem.Click += new System.EventHandler(this.saveOutputImageAs);
            // 
            // savePatternImageAsToolStripMenuItem
            // 
            this.savePatternImageAsToolStripMenuItem.Name = "savePatternImageAsToolStripMenuItem";
            this.savePatternImageAsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.savePatternImageAsToolStripMenuItem.Text = "Save Pattern Image As...";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // pictureBoxOriginal
            // 
            this.pictureBoxOriginal.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxOriginal.Name = "pictureBoxOriginal";
            this.pictureBoxOriginal.Size = new System.Drawing.Size(640, 480);
            this.pictureBoxOriginal.TabIndex = 3;
            this.pictureBoxOriginal.TabStop = false;
            // 
            // pictureBoxNew
            // 
            this.pictureBoxNew.Location = new System.Drawing.Point(647, 0);
            this.pictureBoxNew.Name = "pictureBoxNew";
            this.pictureBoxNew.Size = new System.Drawing.Size(640, 480);
            this.pictureBoxNew.TabIndex = 4;
            this.pictureBoxNew.TabStop = false;
            // 
            // interpCombo
            // 
            this.interpCombo.FormattingEnabled = true;
            this.interpCombo.Location = new System.Drawing.Point(107, 486);
            this.interpCombo.Name = "interpCombo";
            this.interpCombo.Size = new System.Drawing.Size(121, 21);
            this.interpCombo.TabIndex = 5;
            this.interpCombo.DropDownClosed += new System.EventHandler(this.interpCombo_DropDownClosed);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 489);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Interpolation Mode";
            // 
            // widthUpDown
            // 
            this.widthUpDown.Location = new System.Drawing.Point(320, 489);
            this.widthUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.widthUpDown.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.widthUpDown.Name = "widthUpDown";
            this.widthUpDown.Size = new System.Drawing.Size(62, 20);
            this.widthUpDown.TabIndex = 7;
            this.widthUpDown.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.widthUpDown.ValueChanged += new System.EventHandler(this.modifyOutputSize);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(244, 492);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Output Width";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(388, 492);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Output Height";
            // 
            // heightUpDown
            // 
            this.heightUpDown.Location = new System.Drawing.Point(464, 490);
            this.heightUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.heightUpDown.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.heightUpDown.Name = "heightUpDown";
            this.heightUpDown.Size = new System.Drawing.Size(62, 20);
            this.heightUpDown.TabIndex = 9;
            this.heightUpDown.Value = new decimal(new int[] {
            48,
            0,
            0,
            0});
            this.heightUpDown.ValueChanged += new System.EventHandler(this.modifyOutputSize);
            // 
            // fixRatioCheck
            // 
            this.fixRatioCheck.AutoSize = true;
            this.fixRatioCheck.Location = new System.Drawing.Point(540, 491);
            this.fixRatioCheck.Name = "fixRatioCheck";
            this.fixRatioCheck.Size = new System.Drawing.Size(67, 17);
            this.fixRatioCheck.TabIndex = 11;
            this.fixRatioCheck.Text = "Fix Ratio";
            this.fixRatioCheck.UseVisualStyleBackColor = true;
            this.fixRatioCheck.CheckedChanged += new System.EventHandler(this.modifyOutputSize);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(613, 493);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Max Colours";
            // 
            // maxColoursUpDown
            // 
            this.maxColoursUpDown.Location = new System.Drawing.Point(689, 490);
            this.maxColoursUpDown.Maximum = new decimal(new int[] {
            0,
            1,
            0,
            0});
            this.maxColoursUpDown.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.maxColoursUpDown.Name = "maxColoursUpDown";
            this.maxColoursUpDown.Size = new System.Drawing.Size(62, 20);
            this.maxColoursUpDown.TabIndex = 12;
            this.maxColoursUpDown.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.maxColoursUpDown.ValueChanged += new System.EventHandler(this.modifyColours);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1301, 541);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pictureBoxOriginal);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.widthUpDown);
            this.tabPage1.Controls.Add(this.pictureBoxNew);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.maxColoursUpDown);
            this.tabPage1.Controls.Add(this.heightUpDown);
            this.tabPage1.Controls.Add(this.fixRatioCheck);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.interpCombo);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1293, 515);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Crop and Resize";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.editButton);
            this.tabPage2.Controls.Add(this.zoomablePictureBox1);
            this.tabPage2.Controls.Add(this.pictureBoxOutput);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1293, 515);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Produce Pattern";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage2.Enter += new System.EventHandler(this.tabPage2_Enter);
            // 
            // editButton
            // 
            this.editButton.Location = new System.Drawing.Point(8, 486);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(130, 23);
            this.editButton.TabIndex = 6;
            this.editButton.Text = "Edit / Update Patterns";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // zoomablePictureBox1
            // 
            this.zoomablePictureBox1.Location = new System.Drawing.Point(646, 0);
            this.zoomablePictureBox1.Name = "zoomablePictureBox1";
            this.zoomablePictureBox1.Size = new System.Drawing.Size(640, 480);
            this.zoomablePictureBox1.TabIndex = 5;
            this.zoomablePictureBox1.TabStop = false;
            // 
            // pictureBoxOutput
            // 
            this.pictureBoxOutput.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxOutput.Name = "pictureBoxOutput";
            this.pictureBoxOutput.Size = new System.Drawing.Size(640, 480);
            this.pictureBoxOutput.TabIndex = 4;
            this.pictureBoxOutput.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1301, 569);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip);
            this.Name = "MainForm";
            this.Text = "CrossStitchCreator";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxColoursUpDown)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.zoomablePictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOutput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBoxOriginal;
        private System.Windows.Forms.PictureBox pictureBoxNew;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ComboBox interpCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown widthUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown heightUpDown;
        private System.Windows.Forms.CheckBox fixRatioCheck;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown maxColoursUpDown;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PictureBox pictureBoxOutput;
        //private AdriansLib.ZoomablePictureBox zoomablePictureBox1;
        private System.Windows.Forms.PictureBox zoomablePictureBox1;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveOutputImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savePatternImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveOutputImageAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savePatternImageAsToolStripMenuItem;
    }
}

