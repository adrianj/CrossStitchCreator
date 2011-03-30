using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Media.Imaging; // PresentationCore.dll
using AdriansLib;
using System.ComponentModel;

namespace CrossStitchCreator
{
    [Serializable()]
    public class ImagingTool
    {
        public Bitmap InputImage { get; set; }
        private Bitmap mOutput;
        public Bitmap OutputImage { get {  return mOutput; } }
        public IColourMap ColourMap { get; set; }
        public int TargetColours { get; set; }

        public ImagingTool(Image inputImage) : this(inputImage, null) { }

        public ImagingTool(Image inputImage, IColourMap cmap)
        {
            InputImage = (Bitmap)inputImage;
            mOutput = InputImage;
            ColourMap = cmap;
        }

        public void ReplaceColour(Color colorToReplace, Color newColor)
        {
            Bitmap original = OutputImage;
            for (int x = 0; x < original.Width; x++)
                for (int y = 0; y < original.Height; y++)
                {
                    if (original.GetPixel(x, y) == colorToReplace)
                        original.SetPixel(x, y, newColor);
                }
        }

        public void RemoveFromPalette(IColourInfo colorToRemove) { if(colorToRemove != null) RemoveFromPalette(colorToRemove.Colour); }
        public void RemoveFromPalette(Color colorToRemove)
        {
            ColourMap.RemoveColour(colorToRemove);
            ReplaceColour(colorToRemove, ColourMap.GetNearestColour(colorToRemove));
        }

        public Bitmap FitToControl(System.Windows.Forms.Control ctrl) { return FitToControl(ctrl, InterpolationMode.Invalid); }
        /// <summary>
        /// The idea here is to stop image from stretching.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        public Bitmap FitToControl(System.Windows.Forms.Control ctrl, InterpolationMode iMode)
        {
            float aspectCtrl = (float)ctrl.Width / (float)ctrl.Height;
            float aspectImg = (float)OutputImage.Width / (float)OutputImage.Height;
            Size newSize = new Size(ctrl.Width, (int)((float)ctrl.Width / aspectImg));
            if (aspectCtrl / aspectImg > 1)
            {
                newSize = new Size((int)((float)ctrl.Height * aspectImg),ctrl.Height);
            }
            ResizeImage(newSize,iMode);
            return OutputImage;
        }


        public void ResizeImage(Size size) { ResizeImage(size, InterpolationMode.Invalid); }
        public void ResizeImage(Size size, InterpolationMode iMode)
        {
            Bitmap original = OutputImage;
            Bitmap b = new Bitmap(size.Width, size.Height, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage((Image)b);
            if (iMode != InterpolationMode.Invalid)
            {
                g.InterpolationMode = iMode;
                g.DrawImage(original, 0, 0, size.Width, size.Height);
            }
            else
            {
                // If going from low res to high res, I want to keep it pixelated
                if (size.Width > original.Width && size.Height > original.Height)
                {
                    float scaleW = (float)size.Width / (float)original.Width;
                    float scaleH = (float)size.Height / (float)original.Height;
                    for (int x = 0; x < original.Width; x++)
                        for (int y = 0; y < original.Height; y++)
                        {
                            Color c = original.GetPixel(x, y);
                            g.FillRectangle(new SolidBrush(c), x * scaleW, y * scaleH, scaleW, scaleH);
                        }
                }
                else
                {
                    g.DrawImage(original, 0, 0, size.Width, size.Height);
                }
            }
            g.Dispose();

            mOutput = b;
        }

        /// <summary>
        /// Resets ColourInfo.Frequency fields to the number of times that colour appears in the image.
        /// If Image has N pixels, and ColourMap has M colours, total time = M + Nlog(M) ~= N
        /// </summary>
        public void UpdateColourMapFrequency()
        {
            if (mOutput == null || ColourMap == null) return;
            ColourMap.ClearFrequencies();  // M
            Bitmap b = mOutput;
            for (int x = 0; x < b.Width; x++) // this loop iterates N times
                for (int y = 0; y < b.Height; y++)
                {
                    Color c = b.GetPixel(x, y);
                    ColourMap.Colours[c].Frequency++; // Dictionary access is log(M)?
                }
        }

        /// <summary>
        /// Resets ColourInfo.Frequency fields, and removes colour not present in Colourmap.
        /// If Image has N pixels, and ColourMap has M colours, total time = 3M + Nlog(M) ~= N
        /// </summary>
        public void UpdateColourMapFromImage()
        {
            if (mOutput == null || ColourMap == null) return;
            UpdateColourMapFrequency();
            IColourInfo[] temp = ColourMap.ToArray();
            foreach (IColourInfo col in temp)
            {
                if (col.Frequency < 1 && !col.IsChecked)
                {
                    ColourMap.RemoveColour(col.Colour);
                }
            }
        }


        /// <summary>
        /// This is the simplest method - it just truncates to RGB555.
        /// if Image has N pixels, total time ~= N.
        /// </summary>
        public void ReduceColourDepth()
        {
            Bitmap orig = mOutput;
            Bitmap b = new Bitmap(orig.Width, orig.Height, PixelFormat.Format24bppRgb);

            for (int x = 0; x < b.Width; x++)
                for (int y = 0; y < b.Height; y++)
                {
                    Color c = orig.GetPixel(x, y);
                    Color newC = Color.FromArgb(c.A & 0xF8, c.R & 0xF8, c.G & 0xF8, c.B & 0xF8);
                    b.SetPixel(x, y, newC);
                }
            mOutput = b;

            UpdateColourMapFromImage();
        }


        public object ReduceColourDepth(object o, BackgroundWorker w, DoWorkEventArgs e) { 
            Console.WriteLine("Worker thread: " + this + ","+TargetColours+","+ColourMap +","+ColourMap.Count);
            // This case we're removing colours one at a time, replacing them with closest colours
            if (TargetColours > 0)
            {
                Console.WriteLine("ReduceColourDepth: initial nColours = " + ColourMap.Count + ", Target: " + TargetColours);
                int toRemove = ColourMap.Count - TargetColours;

                for (int i = 0; i < toRemove; i++)
                {
                    if (w.CancellationPending)
                    {
                        e.Cancel = true;
                        return this;
                    }
                    IColourInfo least = ColourMap.GetLeastCommonColourInfo(true);
                    RemoveFromPalette(least);
                    UpdateColourMapFromImage();
                    w.ReportProgress(i * 100 / toRemove);
                }
                Console.WriteLine("ReduceColourDepth: final number of colours = " + ColourMap.Count);
            }
            else // This case we have no idea how many colours, we just want to fit the colour map.
            {
                Bitmap orig = mOutput;
                Bitmap b = new Bitmap(orig.Width, orig.Height, PixelFormat.Format24bppRgb);

                Console.WriteLine("ReduceColourDepth: initial nColours = " + ColourMap.Count);
                int i = 0;
                for (int x = 0; x < b.Width; x++)
                    for (int y = 0; y < b.Height; y++)
                    {
                        if (w.CancellationPending)
                        {
                            e.Cancel = true;
                            return this;
                        }
                        Color c = orig.GetPixel(x, y);
                        Color newC = ColourMap.GetNearestColour(c);
                        b.SetPixel(x, y, newC);
                        w.ReportProgress((i++) * 100 / (b.Width*b.Height));
                    }
                mOutput = b;
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmap"></param>
        /// <param name="maxColours"></param>
        public void ReduceColourDepth(int maxColours)
        {
            if (ColourMap == null) return;
            this.TargetColours = maxColours;
            UpdateColourMapFromImage();
            int toRemove = ColourMap.Count - TargetColours;
            // Don't bother if we're already less than the target or have no ColourMap.
            if (toRemove <= 0) return; 

            Bitmap saved = new Bitmap(mOutput);
            ProgressBarForm prog = new ProgressBarForm("Reducing Colour Depth...");
            prog.StartWorker(ReduceColourDepth, this);
            if (prog.DialogResult != System.Windows.Forms.DialogResult.OK)
            {
                mOutput = saved;
                UpdateColourMapFromImage();
            }

        }

        /// <summary>
        /// Convert colours to fit a given colourmap
        /// </summary>
        /// <param name="cmap">The colourmap to fit.</param>
        public void ReduceColourDepth(IColourMap cmap)
        {
            ColourMap = cmap;
            this.TargetColours = 0;
            Bitmap saved = new Bitmap(mOutput);
            ProgressBarForm prog = new ProgressBarForm("Fitting Image to Colour Map...");
            prog.CanCancel = false;
            prog.StartWorker(ReduceColourDepth, this);
            Console.WriteLine("ReduceColourDepth: final number of colours = " + ColourMap.Count);
        }

        private Dictionary<Color,Bitmap> mPatterns = null;
        public void ReplaceColoursWithPatterns(PatternEditor patterns)
        {
            mPatterns = patterns.GetPatterns();
            Bitmap saved = new Bitmap(mOutput);
            ProgressBarForm prog = new ProgressBarForm("Replacing Colours With Patterns...");
            prog.StartWorker(ReplaceColoursWithPatterns, this);
            if (prog.DialogResult != System.Windows.Forms.DialogResult.OK)
            {
                mOutput = saved;
            }
        }
        public object ReplaceColoursWithPatterns(object p, BackgroundWorker w, DoWorkEventArgs e)
        {
            if (mPatterns == null) { e.Cancel = true; return this; }
            int pScale = PatternEditor.PATTERN_WIDTH;
            Bitmap orig = mOutput;
            Bitmap b = new Bitmap(orig.Width * pScale, orig.Height * pScale, PixelFormat.Format24bppRgb);
            

            Graphics g = Graphics.FromImage(b);
            //AdriansLib.ProgressBarForm prog = new AdriansLib.ProgressBarForm("Replacing Colours With Patterns...", orig.Width * orig.Height);
            //prog.Show();
            int co = 0;
            for (int x = 0; x < orig.Width; x++)
                for (int y = 0; y < orig.Height; y++)
                {
                    if (w.CancellationPending) { e.Cancel = true; return this; }
                    Color c = orig.GetPixel(x, y);
                    Bitmap i = mPatterns[c];
                    g.DrawImage(i, x * pScale, y * pScale);
                    w.ReportProgress((co++) * 100 / (b.Width * b.Height));
                }
            g.Dispose();
            mOutput = b;
            return this;
        }

    }
}
