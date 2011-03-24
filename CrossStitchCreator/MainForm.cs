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

        private Bitmap mOutputImage;
        private Bitmap mInputImage;
        private Bitmap mPatternImage;
        //public Dictionary<Color, int> OutputImagePalette { get; set; }

        public ColourMap ColourMap = null;
        private ColourMapViewer mColourViewer;

        public const int MAX_COLOURS = 200;

        public MainForm()
        {
            InitializeComponent();
            patternEditor = new PatternEditor(this);
            interpCombo.DataSource = Enum.GetValues(typeof(InterpolationMode));
            patternEditor.FormClosing += new FormClosingEventHandler(patternEditor_FormClosing);
        }

        void mColourViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            mColourViewer.Hide();
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
                disposeColourViewer();
                mColourViewer = null;
                mInputImage = (Bitmap)Image.FromFile(mSettings.InputImagePath);
                Console.WriteLine("Input Size: " + mInputImage.Size);
                if (mInputImage.Height > 1000 || mInputImage.Width > 1000)
                {
                    // too big. resize to be smaller.
                    float maxDim = Math.Max(mInputImage.Width, mInputImage.Height);
                    float scale = maxDim / 1000;
                    Size newSize = new Size((int)((float)mInputImage.Width / scale),(int)((float) mInputImage.Height / scale));
                    ImagingTools tool = new ImagingTools(mInputImage);
                    tool.ResizeImage(newSize,InterpolationMode.Default);
                    mInputImage = tool.OutputImage;
                    
                }
                mSettings.InputImageSize = mInputImage.Size;
                mSettings.FixSizeRatio = fixRatioCheck.Checked;
                Console.WriteLine("Input Size: " + mInputImage.Size);
                mOutputImage = mInputImage;
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("Ooops! Bad Argument" + Environment.NewLine + "Filename = " + mSettings.InputImagePath
                    + Environment.NewLine + e + Environment.NewLine + e.StackTrace);
            }
            finally { this.Cursor = Cursors.Default; }
            ResizeImage();
            updateFormFromSettings();
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
            ColourMap = new DMCColourMap();
            ImagingTools tool = new ImagingTools(mInputImage, ColourMap);
            mSettings.InputImageSize = mInputImage.Size;
            tool.ResizeImage(mSettings.OutputImageSize, (InterpolationMode)interpCombo.SelectedItem);
            tool.ReduceColourDepth();
            tool.ReduceColourDepth(ColourMap);
            mOutputImage = tool.OutputImage;
            this.Cursor = Cursors.Default;
            RedrawImages();
        }

        public void RecolourImage(bool startAgain)
        {
            this.Cursor = Cursors.WaitCursor;
            if (startAgain) 
                ResizeImage();
            ImagingTools tool = new ImagingTools(mOutputImage, ColourMap);
            tool.ReduceColourDepth(mSettings.MaxColours);
            mOutputImage = tool.OutputImage;
            this.Cursor = Cursors.Default;
            RedrawImages();
        }

        private void RedrawImages()
        {
            this.Cursor = Cursors.WaitCursor;
            ImagingTools tool1 = new ImagingTools(mInputImage);
            pictureBoxOriginal.Image = tool1.FitToControl(pictureBoxOriginal);
            ImagingTools tool2 = new ImagingTools(mOutputImage);

            UpdateColourMap();
            eventsEnabled = false;
            maxColoursUpDown.Value = ColourMap.Count;
            mSettings.MaxColours = ColourMap.Count;
            eventsEnabled = true;

            //tool2.ResizeImage(pictureBoxNew.Size);
            pictureBoxResized.Image = tool2.FitToControl(pictureBoxResized);
            
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
            ShowPatternEditor();
            RedrawPattern();
        }    
        

        void patternEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            patternEditor.Hide();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            ShowPatternEditor();
        }

        public void ShowPatternEditor()
        {
            if (mOutputImage != null)
            {
                this.Cursor = Cursors.WaitCursor;
                ImagingTools tool = new ImagingTools(mOutputImage);
                patternEditor.UpdateColourMap();
                this.Cursor = Cursors.Default;
                patternEditor.Show();
            }
        }

        private void UpdateColourMap()
        {

            ImagingTools tool = new ImagingTools(mOutputImage, ColourMap);
            tool.UpdateColourMapFromImage();
            if (mColourViewer != null)
            {
                Console.WriteLine("Update viewer");
                mColourViewer.UpdateColourMap();
            }
        }

        private void RedrawPattern()
        {
            // Pattern image size is PatternEditor.PATTERN_WIDTH x OutputImage.
            ImagingTools tool3 = new ImagingTools(mOutputImage);
            tool3.ReplaceColoursWithPatterns(patternEditor);
            mPatternImage = tool3.OutputImage;
            zoomablePictureBox1.Image = mPatternImage;
            pictureBoxRecoloured2.Image = pictureBoxResized.Image;
        }
        #endregion

        private void saveOutputImage(object sender, EventArgs e)
        {
            if (mSettings.OutputImagePath == null)
            {
                saveOutputImageAs(sender, e);
            }
        }

        private void saveOutputImageAs(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            ShowPatternEditor();
            RedrawPattern();
        }

        private void showPaletteButton_Click(object sender, EventArgs e)
        {
            if (mColourViewer == null)
            {
                if (ColourMap != null)
                {
                    mColourViewer = new ColourMapViewer(ColourMap);
                    mColourViewer.FormClosing += new FormClosingEventHandler(mColourViewer_FormClosing);
                    mColourViewer.ColourDeleteEvent += new ColourDeleteEventHandler(mColourViewer_ColourDeleteEvent);
                }
                else return;
            }
            UpdateColourMap();
            mColourViewer.Show();
            mColourViewer.Focus();
        }

        void mColourViewer_ColourDeleteEvent(object sender, ColourDeleteEventArgs e)
        {
            ImagingTools tool = new ImagingTools(mOutputImage, ColourMap);
            tool.RemoveFromPalette(e.Colour);
            RedrawImages();
        }

        private void disposeColourViewer()
        {
            if (mColourViewer != null)
            {
                mColourViewer.FormClosing -= mColourViewer_FormClosing;
                mColourViewer.ColourDeleteEvent -= mColourViewer_ColourDeleteEvent;
                mColourViewer.Close();
                mColourViewer.Dispose();
            }
        }

        private void pictureBoxNew_MouseClick(object sender, MouseEventArgs e)
        {
            if (mOutputImage != null)
            {
                float xScale = (float)pictureBoxResized.Image.Width / (float)mOutputImage.Width;
                float yScale = (float)pictureBoxResized.Image.Height / (float)mOutputImage.Height;
                int x = (int)((float)e.X / xScale);
                int y = (int)((float)e.Y / yScale);
                if (x >= 0 && x < mOutputImage.Width && y >= 0 && y < mOutputImage.Height && mColourViewer != null)
                {
                    Color c = mOutputImage.GetPixel(x, y);
                    mColourViewer.SelectColour(c);
                }
            }
        }
    }
}
