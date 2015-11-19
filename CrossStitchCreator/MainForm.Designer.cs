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
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBoxOriginal = new System.Windows.Forms.PictureBox();
            this.pictureBoxResized = new System.Windows.Forms.PictureBox();
            this.interpCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.widthUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.heightUpDown = new System.Windows.Forms.NumericUpDown();
            this.fixRatioCheck = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageResize = new System.Windows.Forms.TabPage();
            this.cropButton = new System.Windows.Forms.Button();
            this.resizeButton = new System.Windows.Forms.Button();
            this.tabPageRecolour = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.colourMapCombo = new System.Windows.Forms.ComboBox();
            this.recolourButton = new System.Windows.Forms.Button();
            this.pictureBoxRecoloured = new System.Windows.Forms.PictureBox();
            this.pictureBoxResized2 = new System.Windows.Forms.PictureBox();
            this.showPaletteButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.maxColoursUpDown = new System.Windows.Forms.NumericUpDown();
            this.tabPagePattern = new System.Windows.Forms.TabPage();
            this.updateButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.pictureBoxPattern = new DTALib.ZoomablePictureBox();
            this.pictureBoxRecoloured2 = new System.Windows.Forms.PictureBox();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResized)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightUpDown)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPageResize.SuspendLayout();
            this.tabPageRecolour.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRecoloured)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResized2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxColoursUpDown)).BeginInit();
            this.tabPagePattern.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRecoloured2)).BeginInit();
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
            this.undoToolStripMenuItem,
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
            this.savePatternImageToolStripMenuItem.Click += new System.EventHandler(this.savePatternImageToolStripMenuItem_Click);
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
            this.savePatternImageAsToolStripMenuItem.Click += new System.EventHandler(this.savePatternImageAsToolStripMenuItem_Click);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
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
            this.pictureBoxOriginal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxOriginal.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxOriginal.Name = "pictureBoxOriginal";
            this.pictureBoxOriginal.Size = new System.Drawing.Size(640, 480);
            this.pictureBoxOriginal.TabIndex = 3;
            this.pictureBoxOriginal.TabStop = false;
            // 
            // pictureBoxResized
            // 
            this.pictureBoxResized.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxResized.Location = new System.Drawing.Point(648, 0);
            this.pictureBoxResized.Name = "pictureBoxResized";
            this.pictureBoxResized.Size = new System.Drawing.Size(640, 480);
            this.pictureBoxResized.TabIndex = 4;
            this.pictureBoxResized.TabStop = false;
            // 
            // interpCombo
            // 
            this.interpCombo.FormattingEnabled = true;
            this.interpCombo.Location = new System.Drawing.Point(410, 488);
            this.interpCombo.Name = "interpCombo";
            this.interpCombo.Size = new System.Drawing.Size(121, 21);
            this.interpCombo.TabIndex = 5;
            this.interpCombo.DropDownClosed += new System.EventHandler(this.modifyOutputSize);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(305, 491);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Interpolation Mode";
            // 
            // widthUpDown
            // 
            this.widthUpDown.Location = new System.Drawing.Point(623, 491);
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
            this.label2.Location = new System.Drawing.Point(547, 494);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Output Width";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(691, 494);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Output Height";
            // 
            // heightUpDown
            // 
            this.heightUpDown.Location = new System.Drawing.Point(767, 492);
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
            this.fixRatioCheck.Checked = true;
            this.fixRatioCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fixRatioCheck.Location = new System.Drawing.Point(843, 493);
            this.fixRatioCheck.Name = "fixRatioCheck";
            this.fixRatioCheck.Size = new System.Drawing.Size(67, 17);
            this.fixRatioCheck.TabIndex = 11;
            this.fixRatioCheck.Text = "Fix Ratio";
            this.fixRatioCheck.UseVisualStyleBackColor = true;
            this.fixRatioCheck.CheckedChanged += new System.EventHandler(this.modifyColours);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageResize);
            this.tabControl.Controls.Add(this.tabPageRecolour);
            this.tabControl.Controls.Add(this.tabPagePattern);
            this.tabControl.Location = new System.Drawing.Point(0, 27);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1301, 541);
            this.tabControl.TabIndex = 14;
            // 
            // tabPageResize
            // 
            this.tabPageResize.Controls.Add(this.cropButton);
            this.tabPageResize.Controls.Add(this.resizeButton);
            this.tabPageResize.Controls.Add(this.pictureBoxOriginal);
            this.tabPageResize.Controls.Add(this.label3);
            this.tabPageResize.Controls.Add(this.widthUpDown);
            this.tabPageResize.Controls.Add(this.pictureBoxResized);
            this.tabPageResize.Controls.Add(this.label1);
            this.tabPageResize.Controls.Add(this.heightUpDown);
            this.tabPageResize.Controls.Add(this.fixRatioCheck);
            this.tabPageResize.Controls.Add(this.label2);
            this.tabPageResize.Controls.Add(this.interpCombo);
            this.tabPageResize.Location = new System.Drawing.Point(4, 22);
            this.tabPageResize.Name = "tabPageResize";
            this.tabPageResize.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageResize.Size = new System.Drawing.Size(1293, 515);
            this.tabPageResize.TabIndex = 0;
            this.tabPageResize.Text = "Crop and Resize";
            this.tabPageResize.UseVisualStyleBackColor = true;
            // 
            // cropButton
            // 
            this.cropButton.Location = new System.Drawing.Point(4, 486);
            this.cropButton.Name = "cropButton";
            this.cropButton.Size = new System.Drawing.Size(130, 23);
            this.cropButton.TabIndex = 13;
            this.cropButton.Text = "Apply Crop";
            this.cropButton.UseVisualStyleBackColor = true;
            this.cropButton.Click += new System.EventHandler(this.CropImage);
            // 
            // resizeButton
            // 
            this.resizeButton.Location = new System.Drawing.Point(140, 486);
            this.resizeButton.Name = "resizeButton";
            this.resizeButton.Size = new System.Drawing.Size(130, 23);
            this.resizeButton.TabIndex = 12;
            this.resizeButton.Text = "Apply Resize";
            this.resizeButton.UseVisualStyleBackColor = true;
            this.resizeButton.Click += new System.EventHandler(this.ResizeImage);
            // 
            // tabPageRecolour
            // 
            this.tabPageRecolour.Controls.Add(this.label5);
            this.tabPageRecolour.Controls.Add(this.colourMapCombo);
            this.tabPageRecolour.Controls.Add(this.recolourButton);
            this.tabPageRecolour.Controls.Add(this.pictureBoxRecoloured);
            this.tabPageRecolour.Controls.Add(this.pictureBoxResized2);
            this.tabPageRecolour.Controls.Add(this.showPaletteButton);
            this.tabPageRecolour.Controls.Add(this.label4);
            this.tabPageRecolour.Controls.Add(this.maxColoursUpDown);
            this.tabPageRecolour.Location = new System.Drawing.Point(4, 22);
            this.tabPageRecolour.Name = "tabPageRecolour";
            this.tabPageRecolour.Size = new System.Drawing.Size(1293, 515);
            this.tabPageRecolour.TabIndex = 2;
            this.tabPageRecolour.Text = "Modify Colours";
            this.tabPageRecolour.UseVisualStyleBackColor = true;
            this.tabPageRecolour.Enter += new System.EventHandler(this.tabPageRecolour_Enter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(296, 490);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Colour Map";
            // 
            // colourMapCombo
            // 
            this.colourMapCombo.FormattingEnabled = true;
            this.colourMapCombo.Location = new System.Drawing.Point(363, 487);
            this.colourMapCombo.Name = "colourMapCombo";
            this.colourMapCombo.Size = new System.Drawing.Size(277, 21);
            this.colourMapCombo.TabIndex = 21;
            // 
            // recolourButton
            // 
            this.recolourButton.Location = new System.Drawing.Point(4, 486);
            this.recolourButton.Name = "recolourButton";
            this.recolourButton.Size = new System.Drawing.Size(130, 23);
            this.recolourButton.TabIndex = 20;
            this.recolourButton.Text = "Restart Recolour";
            this.recolourButton.UseVisualStyleBackColor = true;
            this.recolourButton.Click += new System.EventHandler(this.modifyColours);
            // 
            // pictureBoxRecoloured
            // 
            this.pictureBoxRecoloured.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxRecoloured.Location = new System.Drawing.Point(648, 0);
            this.pictureBoxRecoloured.Name = "pictureBoxRecoloured";
            this.pictureBoxRecoloured.Size = new System.Drawing.Size(640, 480);
            this.pictureBoxRecoloured.TabIndex = 19;
            this.pictureBoxRecoloured.TabStop = false;
            this.pictureBoxRecoloured.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxNew_MouseClick);
            this.pictureBoxRecoloured.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxNew_MouseClick);
            this.pictureBoxRecoloured.MouseLeave += new System.EventHandler(this.StopPainting);
            this.pictureBoxRecoloured.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxNew_MouseClick);
            this.pictureBoxRecoloured.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxNew_MouseClick);
            // 
            // pictureBoxResized2
            // 
            this.pictureBoxResized2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxResized2.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxResized2.Name = "pictureBoxResized2";
            this.pictureBoxResized2.Size = new System.Drawing.Size(640, 480);
            this.pictureBoxResized2.TabIndex = 18;
            this.pictureBoxResized2.TabStop = false;
            // 
            // showPaletteButton
            // 
            this.showPaletteButton.Location = new System.Drawing.Point(140, 486);
            this.showPaletteButton.Name = "showPaletteButton";
            this.showPaletteButton.Size = new System.Drawing.Size(130, 23);
            this.showPaletteButton.TabIndex = 17;
            this.showPaletteButton.Text = "Show Palette";
            this.showPaletteButton.UseVisualStyleBackColor = true;
            this.showPaletteButton.Click += new System.EventHandler(this.showPalette);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(683, 490);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Number of Colours";
            // 
            // maxColoursUpDown
            // 
            this.maxColoursUpDown.Location = new System.Drawing.Point(783, 488);
            this.maxColoursUpDown.Maximum = new decimal(new int[] {
            0,
            1,
            0,
            0});
            this.maxColoursUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxColoursUpDown.Name = "maxColoursUpDown";
            this.maxColoursUpDown.Size = new System.Drawing.Size(62, 20);
            this.maxColoursUpDown.TabIndex = 15;
            this.maxColoursUpDown.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.maxColoursUpDown.ValueChanged += new System.EventHandler(this.modifyColours);
            // 
            // tabPagePattern
            // 
            this.tabPagePattern.Controls.Add(this.updateButton);
            this.tabPagePattern.Controls.Add(this.editButton);
            this.tabPagePattern.Controls.Add(this.pictureBoxPattern);
            this.tabPagePattern.Controls.Add(this.pictureBoxRecoloured2);
            this.tabPagePattern.Location = new System.Drawing.Point(4, 22);
            this.tabPagePattern.Name = "tabPagePattern";
            this.tabPagePattern.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePattern.Size = new System.Drawing.Size(1293, 515);
            this.tabPagePattern.TabIndex = 1;
            this.tabPagePattern.Text = "Produce Pattern";
            this.tabPagePattern.UseVisualStyleBackColor = true;
            this.tabPagePattern.Enter += new System.EventHandler(this.tabPage2_Enter);
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(140, 486);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(130, 23);
            this.updateButton.TabIndex = 7;
            this.updateButton.Text = "Update Pattern";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // editButton
            // 
            this.editButton.Location = new System.Drawing.Point(4, 486);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(130, 23);
            this.editButton.TabIndex = 6;
            this.editButton.Text = "Show Pattern Editor";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // pictureBoxPattern
            // 
            this.pictureBoxPattern.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxPattern.Image = null;
            this.pictureBoxPattern.Location = new System.Drawing.Point(648, 0);
            this.pictureBoxPattern.Name = "pictureBoxPattern";
            this.pictureBoxPattern.Picture = "";
            this.pictureBoxPattern.Size = new System.Drawing.Size(640, 480);
            this.pictureBoxPattern.TabIndex = 5;
            this.pictureBoxPattern.TabStop = false;
            // 
            // pictureBoxRecoloured2
            // 
            this.pictureBoxRecoloured2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxRecoloured2.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxRecoloured2.Name = "pictureBoxRecoloured2";
            this.pictureBoxRecoloured2.Size = new System.Drawing.Size(640, 480);
            this.pictureBoxRecoloured2.TabIndex = 4;
            this.pictureBoxRecoloured2.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1301, 569);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip);
            this.Name = "MainForm";
            this.Text = "CrossStitchCreator";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResized)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightUpDown)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPageResize.ResumeLayout(false);
            this.tabPageResize.PerformLayout();
            this.tabPageRecolour.ResumeLayout(false);
            this.tabPageRecolour.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRecoloured)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResized2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxColoursUpDown)).EndInit();
            this.tabPagePattern.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRecoloured2)).EndInit();
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
        private System.Windows.Forms.PictureBox pictureBoxResized;
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
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageResize;
        private System.Windows.Forms.TabPage tabPagePattern;
        private System.Windows.Forms.PictureBox pictureBoxRecoloured2;
        private DTALib.ZoomablePictureBox pictureBoxPattern;
        
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveOutputImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savePatternImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveOutputImageAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savePatternImageAsToolStripMenuItem;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.TabPage tabPageRecolour;
        private System.Windows.Forms.Button showPaletteButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown maxColoursUpDown;
        private System.Windows.Forms.PictureBox pictureBoxRecoloured;
        private System.Windows.Forms.PictureBox pictureBoxResized2;
        private System.Windows.Forms.Button cropButton;
        private System.Windows.Forms.Button resizeButton;
        private System.Windows.Forms.Button recolourButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox colourMapCombo;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
    }
}

