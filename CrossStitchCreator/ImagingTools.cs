using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace CrossStitchCreator
{
    [Serializable()]
    public class ImagingTools
    {
        public Bitmap InputImage { get; set; }
        private Bitmap mOutput;
        public Bitmap OutputImage { get {  return mOutput; } }
        private Dictionary<Color, int> mPalette = new Dictionary<Color, int>();
        public Dictionary<Color, int> OutputImagePalette { get { updatePalette(); return mPalette; } }

        public ImagingTools(Image inputImage)
        {
            InputImage = (Bitmap)inputImage;
            mOutput = InputImage;
            updatePalette();
        }

        private void updatePalette()
        {
            mPalette = new Dictionary<Color, int>();
            for (int x = 0; x < mOutput.Width; x++)
                for (int y = 0; y < mOutput.Height; y++)
                {
                    Color c = mOutput.GetPixel(x, y);
                    if (!mPalette.ContainsKey(c)) mPalette[c] = 1;
                    else mPalette[c]++;
                }

            int nColours = mPalette.Count;
            Console.WriteLine("ReduceColourDepth: final number of colours = " + nColours);
        }

        public void ReplaceColour(Color colorToReplace, Color newColor, bool chainProcess)
        {
            Bitmap original = InputImage;
            if (chainProcess) original = OutputImage;
            for (int x = 0; x < original.Width; x++)
                for (int y = 0; y < original.Height; y++)
                {
                    if (original.GetPixel(x, y) == colorToReplace)
                        original.SetPixel(x, y, newColor);
                }
        }

        private void removeFromPalette(Color colorToRemove)
        {
            if (!mPalette.ContainsKey(colorToRemove)) return;
            mPalette.Remove(colorToRemove);
            ReplaceColour(colorToRemove, findSimilarColour(colorToRemove), true);
            updatePalette();
        }

        private Color findSimilarColour(Color colorToMatch)
        {
            byte minDiff = byte.MaxValue;
            Color c = Color.Black;
            foreach (KeyValuePair<Color, int> pair in mPalette)
            {
                byte diff = getMaxDiff(colorToMatch, pair.Key);
                if (diff < minDiff)
                {
                    minDiff = diff;
                    c = pair.Key;
                }
            }
            return c;
        }

        private Color findLeastCommonColour()
        {
            Color c = Color.Black;
            int min = int.MaxValue;
            foreach (KeyValuePair<Color, int> pair in mPalette)
            {
                // can quit early if the frequency is 1.
                if (pair.Value == 1)
                    return pair.Key;
                if (pair.Value < min)
                {
                    min = pair.Value;
                    c = pair.Key;
                }
            }
            return c;
        }

        private byte getMaxDiff(Color c1, Color c2)
        {
            // Get difference between components ( red green blue )
            // of given color and appropriate components of pallete color
            byte bDiff = c1.B > c2.B ? (byte)(c1.B - c2.B) : (byte)(c2.B - c1.B);
            byte gDiff = c1.G > c2.G ? (byte)(c1.G - c2.G) : (byte)(c2.G - c1.G);
            byte rDiff = c1.R > c2.R ? (byte)(c1.R - c2.R) : (byte)(c2.R - c1.R);
            byte aDiff = c1.A > c2.A ? (byte)(c1.A - c2.A) : (byte)(c2.A - c1.A);

            // Get max difference
            byte max = bDiff > gDiff ? bDiff : gDiff;
            max = max > rDiff ? max : rDiff;
            max = max > aDiff ? max : aDiff;

            return max;

        }
        

        public void ResizeImage(Size size) { ResizeImage(size, true, InterpolationMode.Invalid); }
        public void ResizeImage(Size size, InterpolationMode iMode) { ResizeImage(size, true, iMode); }
        public void ResizeImage(Size size, bool chainProcess) { ResizeImage(size, chainProcess, InterpolationMode.Invalid); }
        public void ResizeImage(Size size, bool chainProcess,InterpolationMode iMode)
        {
            Bitmap original = InputImage;
            if (chainProcess) original = OutputImage;
            Bitmap b = new Bitmap(size.Width, size.Height,PixelFormat.Format16bppRgb555);
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
                    float scaleW = size.Width / original.Width;
                    float scaleH = size.Height / original.Height;
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

        public void ReduceColourDepth() { ReduceColourDepth(int.MaxValue, true); }
        public void ReduceColourDepth(int maxColours, bool chainProcess)
        {
            Bitmap orig = InputImage;
            if (chainProcess) orig = mOutput;
            Bitmap b = new Bitmap(orig.Width, orig.Height, PixelFormat.Format16bppRgb555);

            updatePalette();
            Console.WriteLine("ReduceColourDepth: initial nColours = " + mPalette.Count);

            for (int x = 0; x < b.Width; x++)
                for (int y = 0; y < b.Height; y++)
                {
                    Color c = orig.GetPixel(x, y);
                    Color newC = Color.FromArgb(c.A & 0xF0, c.R & 0xF0, c.G & 0xF0, c.B & 0xF0);
                    b.SetPixel(x, y, newC);
                }
            mOutput = b;
            updatePalette();
            int nColours = mPalette.Count;
            Console.WriteLine("ReduceColourDepth: colours after simple reduction = " + nColours);

            int toRemove = nColours - maxColours;
            if (toRemove > 0)
            {
                AdriansLib.ProgressBarForm prog = new AdriansLib.ProgressBarForm("Reducing Colour Depth...",toRemove);
                prog.Show();

                for (int i = 0; i < toRemove; i++)
                {
                    removeFromPalette(findLeastCommonColour());
                    prog.Increment(1);
                }
                prog.Close();
            }


            nColours = mPalette.Count;
            Console.WriteLine("ReduceColourDepth: final number of colours = " + nColours);
        }

        public void ReplaceColoursWithPatterns(PatternEditor patterns) { ReplaceColoursWithPatterns(patterns,true); }
        public void ReplaceColoursWithPatterns(PatternEditor patterns,bool chainProcess)
        {
            int pScale = PatternEditor.PATTERN_WIDTH;
            Bitmap orig = InputImage;
            if (chainProcess) orig = mOutput;
            Bitmap b = new Bitmap(orig.Width*pScale, orig.Height*pScale,PixelFormat.Format16bppRgb555);

            Graphics g = Graphics.FromImage(b);
            for (int x = 0; x < orig.Width; x++)
                for (int y = 0; y < orig.Height; y++)
                {
                    Color c = orig.GetPixel(x, y);
                    Image i = patterns.GetPattern(c);
                    g.DrawImage(i, x * pScale, y * pScale);
                    //g.FillRectangle(new SolidBrush(c), x * pScale + 1, y * pScale + 1, pScale - 2, pScale - 2);
                    //g.DrawRectangle(Pens.Black, x * pScale, y * pScale, pScale, pScale);
                }
            g.Dispose();
            mOutput = b;
        }

        // This is a pretty hideous O(n^2) implementation...
        public void SortPaletteByFrequency()
        {
            if (mPalette.Count > 1)
            {
                Dictionary<Color, int> sorted = new Dictionary<Color, int>();
                while (mPalette.Count > 0)
                {
                    int max = int.MinValue;
                    Color maxC = Color.White;
                    foreach (KeyValuePair<Color, int> pair in mPalette)
                    {
                        if (pair.Value > max)
                        {
                            maxC = pair.Key;
                            max = pair.Value;
                        }
                    }
                    
                    //Console.WriteLine(""+mPalette.Count+","+max+","+maxC);
                    sorted.Add(maxC, max);
                    mPalette.Remove(maxC);
                }
                mPalette = sorted;
            }
        }
    }
}
