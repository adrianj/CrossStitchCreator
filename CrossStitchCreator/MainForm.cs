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
using System.Reflection;
using System.IO;


namespace CrossStitchCreator
{

    public partial class MainForm : Form
    {

        private AboutBox aboutBox = new AboutBox();
        private CrossStitchSettings mSettings = new CrossStitchSettings();
        private bool eventsEnabled = true;
        private PatternEditor patternEditor = null;
        private int maxUndos = 5;
        private int nUndos = 0;

        private Bitmap mInputImage;
        private Bitmap mCroppedImage;
        private Bitmap mResizedImage;
        private Bitmap[] undoRecolours;
        private IColourMap[] undoMaps;
        private Bitmap recolouredImage;
        private Bitmap mRecolouredImage
        {
            get
            {
                return recolouredImage;
            }
            set
            {
                nUndos = 0;
                Console.WriteLine("mRecolouredImageChange");
                recolouredImage = value;

                if (ColourMap == null)
                    recolouredImage.Tag = -1;
                else
                {
                    UpdateColourMap();
                    recolouredImage.Tag = (int)ColourMap.Count;
                }
                for (int i = maxUndos - 1; i > 0; i--)
                {
                    undoMaps[i] = undoMaps[i - 1];
                    undoRecolours[i] = undoRecolours[i - 1];
                    Console.Write("undo shifting: ");
                    if (undoRecolours[i] != null) Console.WriteLine("" + undoRecolours[i].Tag);
                }
                undoRecolours[0] = new Bitmap(recolouredImage);
                if (ColourMap != null) undoMaps[0] = ColourMap.Clone();
                else undoMaps[0] = null;
            }
        }
        private Bitmap mPatternImage;

        public IColourMap ColourMap {get;set;}
        private ColourMapViewer mColourViewer;

        public const int MAX_COLOURS = 500;

        public MainForm()
        {
            undoRecolours = new Bitmap[maxUndos];
            undoMaps = new IColourMap[maxUndos];

            InitializeComponent();
            patternEditor = new PatternEditor(this);
            interpCombo.DataSource = Enum.GetValues(typeof(InterpolationMode));
            patternEditor.FormClosing += new FormClosingEventHandler(patternEditor_FormClosing);
            colourMapCombo.Items.Add(typeof(DMCColourMap));
            colourMapCombo.Items.Add(typeof(SimpleColourMap));
            colourMapCombo.SelectedIndex = 0;
        }

        public void CreateNewColourMap()
        {
            Type t = (Type)colourMapCombo.SelectedItem;
            //ConstructorInfo c = t.GetConstructor(new Type[] { });
            //object[] ps = new object[] { };

            object cmap = Activator.CreateInstance(t);
            ColourMap = (IColourMap)cmap;

            // Add black and white, because they are awesome.
            Color White = Color.FromArgb(255, 255, 255, 255);
            Color Black = Color.FromArgb(255, 0, 0, 0);
            if (!ColourMap.Colours.ContainsKey(White))
            {
                SimpleColour s = new SimpleColour("White", ColourMap.Colours.Count, White);
                ColourMap.Colours[White] = s;
            }
            if (!ColourMap.Colours.ContainsKey(Black))
            {
                SimpleColour s = new SimpleColour("Black", ColourMap.Colours.Count, Black);
                ColourMap.Colours[Black] = s;
            }
            ColourMap.Colours[White].IsChecked = true;
            ColourMap.Colours[White].IsChecked = true;
            ColourMap.Colours[Black].IsChecked = true;
        }


        #region Form Stuff
        private void updateSettingsFromForm()
        {
            mSettings.OutputHeight = (int)heightUpDown.Value;
            mSettings.OutputWidth = (int)widthUpDown.Value;
            mSettings.FixSizeRatio = fixRatioCheck.Checked;
            if (ColourMap == null)
                mSettings.MaxColours = MAX_COLOURS;
            else
                mSettings.MaxColours = ColourMap.Count;
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
        #endregion

        #region Main Menu Stuff
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

        private void saveOutputImage(object sender, EventArgs e)
        {
            if (mSettings.OutputImagePath == null)
            {
                saveOutputImageAs(sender, e);
            }
        }

        private void saveOutputImageAs(object sender, EventArgs e)
        {
            if (mPatternImage == null) return;
            string filter = "24bit Bitmap (*.bmp)|*.bmp|JPEG (*.jpeg)|*.jpeg|TIFF (*.tiff)|*.tiff,*.tif|Gif (*.gif)|*.gif";
            saveFileDialog.Filter = filter;
            saveFileDialog.ShowDialog();
            string extension = Path.GetExtension(saveFileDialog.FileName).Substring(1);
            try
            {
                if(extension.Equals("bmp"))
                    mPatternImage.Save(saveFileDialog.FileName, ImageFormat.Bmp);
                else if(extension.Equals("gif"))
                    mPatternImage.Save(saveFileDialog.FileName, ImageFormat.Gif);
                else if (extension.Equals("tiff") || extension.Equals("tif"))
                    mPatternImage.Save(saveFileDialog.FileName, ImageFormat.Tiff);
                else if (extension.Equals("jpeg") || extension.Equals("jpg"))
                    mPatternImage.Save(saveFileDialog.FileName, ImageFormat.Jpeg);
                else
                {
                    MessageBox.Show("No Codec to save to this format");
                    return;
                }
            }
            catch (IOException ioe)
            {
                MessageBox.Show("Saving Image Failed: " + ioe);
            }
            
        }

        public void LoadImage()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                disposeColourViewer();
                mColourViewer = null;
                mInputImage = (Bitmap)Image.FromFile(mSettings.InputImagePath);
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
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("Ooops! Bad Argument" + Environment.NewLine + "Filename = " + mSettings.InputImagePath
                    + Environment.NewLine + e + Environment.NewLine + e.StackTrace);
            }
            finally { this.Cursor = Cursors.Default; }
            RedrawTab1Images();
        }


        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nUndos+1 < maxUndos)
            {
                Console.WriteLine("UNDO! (" + undoRecolours[nUndos] + ")" + nUndos);
                if (undoRecolours[nUndos+1] != null)
                {
                    ColourMap = undoMaps[nUndos+1];
                    recolouredImage = undoRecolours[nUndos+1];
                    UpdateColourMap();
                    RedrawTab2Images();
                    Console.WriteLine("Undo tag: " + undoRecolours[nUndos+1].Tag);
                }
                nUndos++;
            }
        }

        #endregion

        #region Tab1 Stuff

        public void ResizeImage(object sender, EventArgs e)
        {
            if (mInputImage == null) return;
            this.Cursor = Cursors.WaitCursor;
            ImagingTools tool;
            if (mCroppedImage == null)
                tool = new ImagingTools(mInputImage, ColourMap);
            else
                tool = new ImagingTools(mCroppedImage, ColourMap);
            //mSettings.InputImageSize = mInputImage.Size;
            tool.ResizeImage(mSettings.OutputImageSize, (InterpolationMode)interpCombo.SelectedItem);
            mResizedImage = tool.OutputImage;
            this.Cursor = Cursors.Default;
            RedrawTab1Images();
        }

        public void CropImage(object sender, EventArgs e)
        {
            mResizedImage = null;
            RedrawTab1Images();
        }

        private void RedrawTab1Images()
        {
            this.Cursor = Cursors.WaitCursor;
            if (mInputImage != null)
            {
                ImagingTools tool1 = new ImagingTools(mInputImage);
                pictureBoxOriginal.Image = tool1.FitToControl(pictureBoxOriginal);
            }
            if (mResizedImage != null)
            {
                ImagingTools tool2 = new ImagingTools(mResizedImage);
                pictureBoxResized.Image = tool2.FitToControl(pictureBoxOriginal);
            }

            updateFormFromSettings();
            this.Cursor = Cursors.Default;
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
                else if (sender == interpCombo)
                {
                    mSettings.IntMode = (InterpolationMode)interpCombo.SelectedItem;
                }
                updateFormFromSettings();
            }
        }

        #endregion

        #region Tab2 Stuff

        private void showPalette(object sender, EventArgs e)
        {
            if (mColourViewer == null)
            {
                if (ColourMap != null)
                {
                    mColourViewer = new ColourMapViewer(ColourMap);
                    mColourViewer.FormClosing += new FormClosingEventHandler(mColourViewer_FormClosing);
                    mColourViewer.ColourChangeEvent += new ColourChangeEventHandler(mColourViewer_ColourChangeEvent);
                }
                else return;
            }
            UpdateColourMap();
            mColourViewer.Show();
            mColourViewer.Focus();
        }

        void mColourViewer_ColourChangeEvent(object sender, ColourChangeEventArgs e)
        {
            if (mRecolouredImage == null) return;
            ImagingTools tool = new ImagingTools(mRecolouredImage, ColourMap);
            if (!e.DoReplace)
            {
                tool.RemoveFromPalette(e.Colour);
            }
            else
            {
                tool.ReplaceColour(e.Colour, e.ReplacementColour);
            }
            mRecolouredImage = tool.OutputImage;
            //UpdateColourMap();
            //showPalette(null, null);
            RedrawTab2Images();
        }

        private void UpdateColourMap()
        {
            if (mRecolouredImage != null && ColourMap != null)
            {
                ImagingTools tool = new ImagingTools(mRecolouredImage, ColourMap);
                tool.UpdateColourMapFromImage();
                Console.WriteLine("Updating Colour Map: " + ColourMap.Count);
                mSettings.MaxColours = ColourMap.Count;
                updateFormFromSettings();
                if (mColourViewer != null)
                {
                    mColourViewer.UpdateColourMap(ColourMap);
                }
            }
        }

        private void RedrawTab2Images()
        {
            this.Cursor = Cursors.WaitCursor;
            if (mResizedImage != null)
            {
                ImagingTools tool1 = new ImagingTools(mResizedImage);
                pictureBoxResized2.Image = tool1.FitToControl(pictureBoxOriginal);
            }
            if (mRecolouredImage != null)
            {
                ImagingTools tool2 = new ImagingTools(mRecolouredImage);
                pictureBoxRecoloured.Image = tool2.FitToControl(pictureBoxOriginal);
            }
            updateFormFromSettings();
            this.Cursor = Cursors.Default;
        }

        private void modifyColours(object sender, EventArgs e)
        {
            if (eventsEnabled && mResizedImage != null)
            {
                if (sender == maxColoursUpDown)
                {
                    updateSettingsFromForm();
                }
                int old = mSettings.MaxColours;
                mSettings.MaxColours = (int)maxColoursUpDown.Value;
                if (mSettings.MaxColours > old) RecolourImage(true);
                else RecolourImage(false);
            }
        }

        public void RecolourImage(bool startAgain)
        {
            this.Cursor = Cursors.WaitCursor;
            ImagingTools tool;
            if (mRecolouredImage == null || startAgain)
            {
                CreateNewColourMap();
                tool = new ImagingTools(mResizedImage, ColourMap);
                //tool.ReduceColourDepth();
                tool.ReduceColourDepth(ColourMap);
                mRecolouredImage = tool.OutputImage;
            }
            else
            {
                tool = new ImagingTools(mRecolouredImage, ColourMap);
                tool.ReduceColourDepth(mSettings.MaxColours);
                mRecolouredImage = tool.OutputImage;
            }
            //UpdateColourMap();
            this.Cursor = Cursors.Default;
            RedrawTab2Images();
        }


        private void disposeColourViewer()
        {
            if (mColourViewer != null)
            {
                mColourViewer.FormClosing -= mColourViewer_FormClosing;
                mColourViewer.ColourChangeEvent -= mColourViewer_ColourChangeEvent;
                mColourViewer.Close();
                mColourViewer.Dispose();
            }
        }

        void mColourViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            mColourViewer.Hide();
        }

        private void pictureBoxNew_MouseClick(object sender, MouseEventArgs e)
        {
            if (mRecolouredImage != null)
            {
                float xScale = (float)pictureBoxResized.Image.Width / (float)mRecolouredImage.Width;
                float yScale = (float)pictureBoxResized.Image.Height / (float)mRecolouredImage.Height;
                int x = (int)((float)e.X / xScale);
                int y = (int)((float)e.Y / yScale);
                if (mColourViewer == null || mColourViewer.Visible == false)
                {
                    showPalette(null, null);
                }
                if (x >= 0 && x < mRecolouredImage.Width && y >= 0 && y < mRecolouredImage.Height)
                {
                    Color c = mRecolouredImage.GetPixel(x, y);
                    mColourViewer.SelectColour(c);
                }
            }
        }


        private void tabPageRecolour_Enter(object sender, EventArgs e)
        {
            RedrawTab2Images();
        }
        #endregion

        #region Tab3 Stuff
        private void tabPage2_Enter(object sender, EventArgs e)
        {
            RedrawTab3Images();
            ShowPatternEditor();
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
            if (mRecolouredImage != null)
            {
                this.Cursor = Cursors.WaitCursor;
                ImagingTools tool = new ImagingTools(mRecolouredImage);
                patternEditor.UpdateColourMap();
                this.Cursor = Cursors.Default;
                patternEditor.Show();
            }
        }

        private void RedrawTab3Images()
        {
            this.Cursor = Cursors.WaitCursor;
            if (mRecolouredImage != null)
            {
                ImagingTools tool1 = new ImagingTools(mRecolouredImage);
                pictureBoxRecoloured2.Image = tool1.FitToControl(pictureBoxRecoloured2);
            }
            if (mPatternImage != null)
            {
                ImagingTools tool2 = new ImagingTools(mPatternImage);
                pictureBoxPattern.Image = tool2.FitToControl(pictureBoxPattern);
            }
            updateFormFromSettings();
            this.Cursor = Cursors.Default;
        }
        private void RedrawPattern()
        {
            if (mRecolouredImage != null)
            {
                ImagingTools tool3 = new ImagingTools(mRecolouredImage);
                tool3.ReplaceColoursWithPatterns(patternEditor);
                mPatternImage = tool3.OutputImage;
                pictureBoxPattern.Image = mPatternImage;
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            ShowPatternEditor();
            RedrawPattern();
        }
        #endregion






    }
}
