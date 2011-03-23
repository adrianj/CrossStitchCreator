using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace CrossStitchCreator
{

    public partial class MainForm : Form
    {

        private AboutBox aboutBox = new AboutBox();
        private CrossStitchSettings mSettings = new CrossStitchSettings();
        private bool eventsEnabled = true;
        private PatternEditor patternEditor = null;

        private Image mOutputImage;
        private Image mInputImage;
        private Image mPatternImage;
        public Dictionary<Color, int> OutputImagePalette { get; set; }

        public const int MAX_COLOURS = 200;

        public MainForm()
        {
            InitializeComponent();
            patternEditor = new PatternEditor(this);
            interpCombo.DataSource = Enum.GetValues(typeof(InterpolationMode));
            patternEditor.FormClosing += new FormClosingEventHandler(patternEditor_FormClosing);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aboutBox.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filter = "Image Files|*.jpg;*.jpeg;*.bmp;*.tif;*.tiff;*.png|All Files|*.*";
            openFileDialog.Filter = filter;
            openFileDialog.Title = "Open Image File...";
            openFileDialog.Multiselect = false;
            openFileDialog.ShowDialog();
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            mSettings.InputImagePath = openFileDialog.FileName;
            LoadImage();
        }

        public void LoadImage()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                mInputImage = Image.FromFile(mSettings.InputImagePath);
                mSettings.InputImageSize = mInputImage.Size;
                mOutputImage = mInputImage;
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("Ooops! Bad Argument" + Environment.NewLine + "Filename = " + mSettings.InputImagePath
                    + Environment.NewLine + e + Environment.NewLine + e.StackTrace);
            }
            finally { this.Cursor = Cursors.Default; }
            ResizeImage();
        }

        private void updateSettingsFromForm()
        {
            mSettings.OutputHeight = (int)heightUpDown.Value;
            mSettings.OutputWidth = (int)widthUpDown.Value;
            mSettings.FixSizeRatio = fixRatioCheck.Checked;
        }

        private void updateFormFromSettings()
        {
            eventsEnabled = false;
            heightUpDown.Value = mSettings.OutputHeight;
            widthUpDown.Value = mSettings.OutputWidth;
            fixRatioCheck.Checked = mSettings.FixSizeRatio;
            maxColoursUpDown.Value = mSettings.MaxColours;
            eventsEnabled = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            updateSettingsFromForm();
        }

        #region Tab1 Stuff

        public void ResizeImage()
        {
            this.Cursor = Cursors.WaitCursor;
            ImagingTools tool = new ImagingTools(mInputImage);
            mSettings.InputImageSize = mInputImage.Size;
            tool.ResizeImage(mSettings.OutputImageSize, (InterpolationMode)interpCombo.SelectedItem);
            mOutputImage = tool.OutputImage;
            this.Cursor = Cursors.Default;
            RedrawImages();
        }

        public void RecolourImage(bool startAgain)
        {
            this.Cursor = Cursors.WaitCursor;
            if (startAgain) ResizeImage();
            ImagingTools tool = new ImagingTools(mOutputImage);
            tool.ReduceColourDepth(mSettings.MaxColours, true);
            mOutputImage = tool.OutputImage;
            this.Cursor = Cursors.Default;
            RedrawImages();
        }

        private void RedrawImages()
        {
            this.Cursor = Cursors.WaitCursor;
            ImagingTools tool1 = new ImagingTools(mInputImage);
            tool1.ResizeImage(pictureBoxOriginal.Size);
            pictureBoxOriginal.Image = tool1.OutputImage;

            ImagingTools tool2 = new ImagingTools(mOutputImage);
            eventsEnabled = false;
            maxColoursUpDown.Value = (int)tool2.OutputImagePalette.Count;
            eventsEnabled = true;

            tool2.ResizeImage(pictureBoxNew.Size);
            pictureBoxNew.Image = tool2.OutputImage;

            
            this.Cursor = Cursors.Default;
        }

        private void interpCombo_DropDownClosed(object sender, EventArgs e)
        {
            ResizeImage();
        }

        private void modifyOutputSize(object sender, EventArgs e)
        {
            if (eventsEnabled)
            {
                if (sender == widthUpDown)
                {
                    mSettings.OutputWidth = (int)widthUpDown.Value;
                }
                else if (sender == heightUpDown)
                {
                    mSettings.OutputHeight = (int)heightUpDown.Value;
                }
                else if (sender == fixRatioCheck)
                {
                    mSettings.FixSizeRatio = fixRatioCheck.Checked;
                }
                updateFormFromSettings();
                ResizeImage();
            }
        }
        private void modifyColours(object sender, EventArgs e)
        {
            if (eventsEnabled)
            {
                int old = mSettings.MaxColours;
                mSettings.MaxColours = (int)maxColoursUpDown.Value;
                if (mSettings.MaxColours > old) RecolourImage(true);
                else RecolourImage(false);
            }
        }
        #endregion


        #region Tab2 Stuff
        private void tabPage2_Enter(object sender, EventArgs e)
        {
            if (mOutputImage != null)
            {
                ImagingTools tool = new ImagingTools(mOutputImage);
                tool.SortPaletteByFrequency();
                patternEditor.Palette = tool.OutputImagePalette;
                patternEditor.Show();
                RedrawPattern();
            }
        }    
        

        void patternEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            patternEditor.Hide();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (mOutputImage != null)
            {
                ImagingTools tool = new ImagingTools(mOutputImage);
                tool.SortPaletteByFrequency();
                patternEditor.Palette = tool.OutputImagePalette;
                patternEditor.Show();
                RedrawPattern();
            }
        }

        private void RedrawPattern()
        {
            // Pattern image is PatternEditor.PATTERN_WIDTH x OutputImage.
            ImagingTools tool3 = new ImagingTools(mOutputImage);
            tool3.ReplaceColoursWithPatterns(patternEditor);
            mPatternImage = tool3.OutputImage;
            zoomablePictureBox1.Image = mPatternImage;
            pictureBoxOutput.Image = pictureBoxNew.Image;
        }
        #endregion
    }
}
