using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Media.Imaging; // PresentationCore.dll

namespace CrossStitchCreator
{
    [Serializable()]
    public class ImagingTools
    {
        public Bitmap InputImage { get; set; }
        private Bitmap mOutput;
        public Bitmap OutputImage { get {  return mOutput; } }
        public IColourMap ColourMap { get; set; }

        public ImagingTools(Image inputImage) : this(inputImage, null) { }

        public ImagingTools(Image inputImage, IColourMap cmap)
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


        public void UpdateColourMapFrequency()
        {
            if (mOutput == null || ColourMap == null) return;
            ColourMap.ClearFrequencies();
            Bitmap b = mOutput;
            for (int x = 0; x < b.Width; x++)
                for (int y = 0; y < b.Height; y++)
                {
                    Color c = b.GetPixel(x, y);
                    //if (!ColourMap.Colours.ContainsKey(c))
                    //    ColourMap.Colours[c] = new DMCColour("???", ColourMap.Count, c, "???");
                    ColourMap.Colours[c].Frequency++;
                }
        }

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


        // this is the simplest method - it just truncates to RGB555.
        public void ReduceColourDepth()
        {
            Bitmap orig = mOutput;
            Bitmap b = new Bitmap(orig.Width, orig.Height, PixelFormat.Format24bppRgb);

            Console.WriteLine("ReduceColourDepth: initial nColours = " + ColourMap.Count);

            for (int x = 0; x < b.Width; x++)
                for (int y = 0; y < b.Height; y++)
                {
                    Color c = orig.GetPixel(x, y);
                    Color newC = Color.FromArgb(c.A & 0xF8, c.R & 0xF8, c.G & 0xF8, c.B & 0xF8);
                    b.SetPixel(x, y, newC);
                }
            mOutput = b;

            UpdateColourMapFromImage();
            int nColours = ColourMap.Count;
            Console.WriteLine("ReduceColourDepth: colours after simple reduction = " + nColours);
        }
        public void ReduceColourDepth(int maxColours)
        {

            UpdateColourMapFromImage();
            Bitmap orig = mOutput;
            Bitmap b = new Bitmap(orig.Width, orig.Height, PixelFormat.Format24bppRgb);

            Console.WriteLine("ReduceColourDepth: initial nColours = " + ColourMap.Count);
            int nColours = ColourMap.Count;

            int toRemove = nColours - maxColours;
            if (toRemove > 0)
            {
                AdriansLib.ProgressBarForm prog = new AdriansLib.ProgressBarForm("Reducing Colour Depth...",toRemove);
                prog.Show();

                for (int i = 0; i < toRemove; i++)
                {
                    IColourInfo least = ColourMap.GetLeastCommonColourInfo(true);
                    RemoveFromPalette(least);
                    UpdateColourMapFromImage();
                    prog.Increment(1);
                }
                prog.Close();
            }


            nColours = ColourMap.Count;
            Console.WriteLine("ReduceColourDepth: final number of colours = " + nColours);
        }
        // Convert colours to fit a given colourmap
        public void ReduceColourDepth(IColourMap cmap)
        {
            ColourMap = cmap;
            Bitmap orig = mOutput;
            Bitmap b = new Bitmap(orig.Width, orig.Height, PixelFormat.Format24bppRgb);

            Console.WriteLine("ReduceColourDepth: initial nColours = " + ColourMap.Count);

            AdriansLib.ProgressBarForm prog = new AdriansLib.ProgressBarForm("Fitting Colours to ColourMap...", orig.Width * orig.Height);
            prog.Show();
            for (int x = 0; x < b.Width; x++)
                for (int y = 0; y < b.Height; y++)
                {
                    Color c = orig.GetPixel(x, y);
                    Color newC = cmap.GetNearestColour(c);
                    b.SetPixel(x, y, newC);
                    prog.Increment(1);
                }
            mOutput = b;
            prog.Close();
            UpdateColourMapFromImage();
            Console.WriteLine("ReduceColourDepth: final number of colours = " + ColourMap.Count);
        }

        public void ReplaceColoursWithPatterns(PatternEditor patterns)
        {
            int pScale = PatternEditor.PATTERN_WIDTH;
            Bitmap orig = mOutput;
            Bitmap b = new Bitmap(orig.Width * pScale, orig.Height * pScale, PixelFormat.Format24bppRgb);
            

            Graphics g = Graphics.FromImage(b);
            AdriansLib.ProgressBarForm prog = new AdriansLib.ProgressBarForm("Replacing Colours With Patterns...", orig.Width * orig.Height);
            prog.Show();

            for (int x = 0; x < orig.Width; x++)
                for (int y = 0; y < orig.Height; y++)
                {
                    Color c = orig.GetPixel(x, y);
                    Image i = patterns.GetPattern(c);
                    g.DrawImage(i, x * pScale, y * pScale);
                    prog.Increment(1);
                }
            prog.Close();
            g.Dispose();
            mOutput = b;
        }

    }
}
