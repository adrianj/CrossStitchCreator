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
        private bool suspendUndo = false;

        private Bitmap mInputImage;
        private Bitmap mCroppedImage;
        private Bitmap mResizedImage;
        private Bitmap[] undoRecolours;
        //private IColourMap[] undoMaps;
        private Bitmap recolouredImage;
        private Bitmap mRecolouredImage
        {
            get
            {
                return recolouredImage;
            }
            set
            {
                if (!suspendUndo)
                {
                    for (int i = maxUndos - 1; i > 0; i--)
                    {
                        //undoMaps[i] = undoMaps[i - 1];
                        undoRecolours[i] = undoRecolours[i - 1];
                    }
                    if (recolouredImage != null)
                    {
                        undoRecolours[0] = new Bitmap(recolouredImage);
                        undoRecolours[0].Tag = recolouredImage.Tag;
                    }
                    else undoRecolours[0] = null;
                }
                recolouredImage = value;
                if (recolouredImage != null)
                {
                    UpdateColourMap();
                    if (ColourMap != null) recolouredImage.Tag = ColourMap.Clone();
                    else recolouredImage.Tag = null;
                }
            }
        }
        private Bitmap mPatternImage;

        public IColourMap ColourMap {get;set;}
        private ColourMapViewer mColourViewer;

        public const int MAX_COLOURS = 100;

        public MainForm()
        {
            undoRecolours = new Bitmap[maxUndos];
            //undoMaps = new IColourMap[maxUndos];

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

        string imageFileFilter = "PNG (*.png)|*.png|24bit Bitmap (*.bmp)|*.bmp|JPEG (*.jpeg)|*.jpeg|TIFF (*.tiff)|*.tiff,*.tif|Gif (*.gif)|*.gif|All Files (*.*)|*.*";

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
            openFileDialog.Filter = imageFileFilter;
            openFileDialog.Title = "Open Image File...";
            openFileDialog.Multiselect = false;
            openFileDialog.ShowDialog();
        }

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            mSettings.InputImagePath = openFileDialog.FileName;
            LoadImage();
        }


        private void savePatternImageAs(object sender, EventArgs e)
        {
            if (mPatternImage == null) return;

            saveFileDialog.Filter = imageFileFilter;
            saveFileDialog.FileName = mSettings.PatternImagePath;
            if(saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                mSettings.PatternImagePath = saveFileDialog.FileName;
                SaveImage(mPatternImage, mSettings.PatternImagePath);
            }
            
        }


        private void SaveImage(Bitmap bmp, string path)
        {
            try
            {
                ImageFormat format = GetImageFormatFromExtension(path);
                if (format != ImageFormat.MemoryBmp)
                    bmp.Save(path, format);
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

        private static ImageFormat GetImageFormatFromExtension(string path)
        {
            string extension = Path.GetExtension(path).Substring(1);
            ImageFormat format = ImageFormat.MemoryBmp;

            if (extension.Equals("bmp"))
                format = ImageFormat.Bmp;
            else if (extension.Equals("gif"))
                format = ImageFormat.Gif;
            else if (extension.Equals("tiff") || extension.Equals("tif"))
                format = ImageFormat.Tiff;
            else if (extension.Equals("jpeg") || extension.Equals("jpg"))
                format = ImageFormat.Jpeg;
            else if (extension.Equals("png"))
                format = ImageFormat.Png;
            return format;
        }


        private void savePatternLegendAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = imageFileFilter;
            saveFileDialog.FileName = mSettings.PatternLegendPath;
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                mSettings.PatternLegendPath = saveFileDialog.FileName;
                SavePatternLegend();
            }
        }


        private void SavePatternLegend()
        {
            Control con = this.patternEditor.ColourList;
            Size oldSize = con.Size;
            if(oldSize.Width < 400 || oldSize.Height < 600)
                con.Size = new System.Drawing.Size(400, 600);
            Bitmap bmp = new Bitmap(con.Width, con.Height);
            con.DrawToBitmap(bmp, new Rectangle(0, 0, con.Width, con.Height));
            SaveImage(bmp, mSettings.PatternLegendPath);
            con.Size = oldSize;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mRecolouredImage == null) return;

            saveFileDialog.Filter = imageFileFilter;
            saveFileDialog.FileName = mSettings.OutputImagePath;
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                mSettings.OutputImagePath = saveFileDialog.FileName;
                SaveImage(mRecolouredImage, mSettings.OutputImagePath);
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
                    ImagingTool tool = new ImagingTool(mInputImage);
                    tool.ResizeImage(newSize,InterpolationMode.Default);
                    mInputImage = tool.OutputImage;
                    
                }
                mSettings.InputImageSize = mInputImage.Size;
                mSettings.FixSizeRatio = fixRatioCheck.Checked;
                mResizedImage = null;
                //mCroppedImage = null;
                mRecolouredImage = null;
                mPatternImage = null;
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("Ooops! Bad Argument" + Environment.NewLine + "Filename = " + mSettings.InputImagePath
                    + Environment.NewLine + e + Environment.NewLine + e.StackTrace);
            }
            finally { this.Cursor = Cursors.Default; }
            RedrawTab1Images();
            RedrawTab2Images();
            RedrawTab3Images();
        }


        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < maxUndos; i++)
            {
                if (undoRecolours[i] != null)
                {
                    Console.Write("Undo: " + i + ", " + undoRecolours[i].Tag);
                    if (undoRecolours[i].Tag != null)
                    {
                        IColourMap cmap = (IColourMap)undoRecolours[i].Tag;
                        Console.WriteLine(", " + cmap.Count);
                    }
                    else Console.WriteLine(", null");
                }
                else Console.WriteLine("Undo: " + i + ", null, null");
            }
            if (undoRecolours[0] != null)
            {
                Console.WriteLine("Undo tag: " + undoRecolours[0].Tag);
                // shift undo arrays to the left.
                ColourMap = (IColourMap)undoRecolours[0].Tag;
                recolouredImage = undoRecolours[0];
                for (int i = 0; i < maxUndos - 1; i++)
                {
                    undoRecolours[i] = undoRecolours[i + 1];
                    //undoMaps[i] = undoMaps[i + 1];
                }
                undoRecolours[maxUndos - 1] = null;
                //undoMaps[maxUndos - 1] = null;
                UpdateColourMap();
                RedrawTab2Images();
            }
        }

        #endregion

        #region Tab1 Stuff

        public void ResizeImage(object sender, EventArgs e)
        {
            if (mInputImage == null) return;
            this.Cursor = Cursors.WaitCursor;
            ImagingTool tool;
            if (mCroppedImage == null)
                tool = new ImagingTool(mInputImage, ColourMap);
            else
                tool = new ImagingTool(mCroppedImage, ColourMap);
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
                ImagingTool tool1 = new ImagingTool(mInputImage);
                pictureBoxOriginal.Image = tool1.FitToControl(pictureBoxOriginal);
            }
            else
            {
                pictureBoxOriginal.Image = null;
            }
            if (mResizedImage != null)
            {
                ImagingTool tool2 = new ImagingTool(mResizedImage);
                pictureBoxResized.Image = tool2.FitToControl(pictureBoxOriginal);
            }
            else
            {
                pictureBoxResized.Image = null;
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
            ImagingTool tool = new ImagingTool(mRecolouredImage, ColourMap);
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
                ImagingTool tool = new ImagingTool(mRecolouredImage, ColourMap);
                tool.UpdateColourMapFromImage();
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
                ImagingTool tool1 = new ImagingTool(mResizedImage);
                pictureBoxResized2.Image = tool1.FitToControl(pictureBoxOriginal);
            }
            else
            {
                pictureBoxResized2.Image = null;
            }
            if (mRecolouredImage != null)
            {
                ImagingTool tool2 = new ImagingTool(mRecolouredImage);
                pictureBoxRecoloured.Image = tool2.FitToControl(pictureBoxOriginal);
            }
            else
            {
                pictureBoxRecoloured.Image = null;
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
                if (sender == recolourButton) RecolourImage(true);
                else RecolourImage(false);
            }
        }

        public void RecolourImage(bool startAgain)
        {
            this.Cursor = Cursors.WaitCursor;
            ImagingTool tool;
            if (mRecolouredImage == null || startAgain)
            {
                CreateNewColourMap();
                tool = new ImagingTool(mResizedImage, ColourMap);
                //tool.ReduceColourDepth();
                tool.ReduceColourDepth(ColourMap);
                mRecolouredImage = tool.OutputImage;
            }
            else
            {
                tool = new ImagingTool(mRecolouredImage, ColourMap);
                tool.ReduceColourDepth((int)maxColoursUpDown.Value);
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

        private Color mPaintingColor;
        private bool mPainting = false;
        private bool lastClickWasRight = false;

        private void pictureBoxNew_MouseClick(object sender, MouseEventArgs e)
        {
            if (mRecolouredImage != null)
            {
                float xScale = (float)pictureBoxResized.Image.Width / (float)mRecolouredImage.Width;
                float yScale = (float)pictureBoxResized.Image.Height / (float)mRecolouredImage.Height;
                int x = (int)((float)e.X / xScale);
                int y = (int)((float)e.Y / yScale);
                if (x >= 0 && x < mRecolouredImage.Width && y >= 0 && y < mRecolouredImage.Height)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (mColourViewer == null || mColourViewer.Visible == false)
                        {
                            showPalette(null, null);
                        }
                        Color c = mRecolouredImage.GetPixel(x, y);
                        mColourViewer.SelectColour(c);
                        mPainting = true;
                        suspendUndo = true;
                        mPaintingColor = c;
                        lastClickWasRight = true;
                    }
                    else if (e.Button == MouseButtons.Left)
                    {
                        lastClickWasRight = false;
                        if (mPainting && mRecolouredImage.GetPixel(x, y) != mPaintingColor)
                        {
                            mRecolouredImage.SetPixel(x, y, mPaintingColor);
                            RedrawTab2Images();
                        }
                    }
                    else
                    {
                        if (mPainting && !lastClickWasRight)
                        {
                            if (suspendUndo)
                            {
                                suspendUndo = false;
                                mRecolouredImage = mRecolouredImage;
                            }
                        }
                    }
                }
            }
        }

        private void StopPainting(object sender, EventArgs e)
        {
            mPainting = false;
            if (suspendUndo)
            {
                suspendUndo = false;
                mRecolouredImage = mRecolouredImage;
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
                ImagingTool tool = new ImagingTool(mRecolouredImage);
                patternEditor.UpdateColourMap();
                this.Cursor = Cursors.Default;
                patternEditor.Show();
                patternEditor.Focus();
            }
        }

        private void RedrawTab3Images()
        {
            this.Cursor = Cursors.WaitCursor;
            if (mRecolouredImage != null)
            {
                ImagingTool tool1 = new ImagingTool(mRecolouredImage);
                pictureBoxRecoloured2.Image = tool1.FitToControl(pictureBoxRecoloured2);
            }
            else
            {
                pictureBoxRecoloured2.Image = null;
            }
            if (mPatternImage != null)
            {
                ImagingTool tool2 = new ImagingTool(mPatternImage);
                pictureBoxPattern.Image = tool2.FitToControl(pictureBoxPattern);
            }
            else
            {
                pictureBoxPattern.Image = null;
            }
            updateFormFromSettings();
            this.Cursor = Cursors.Default;
        }
        private void RedrawPattern()
        {
            if (mRecolouredImage != null)
            {
                ImagingTool tool3 = new ImagingTool(mRecolouredImage);
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
