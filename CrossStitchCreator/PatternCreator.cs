using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Resources;
using System.Globalization;

namespace CrossStitchCreator
{
    public class PatternCreator
    {

        private int count = 0;
        List<Image> patterns;

        public PatternCreator()
        {
            ResourceManager rm = Properties.Patterns.ResourceManager;
            ResourceSet rs = rm.GetResourceSet(new CultureInfo("en-US"), true, true);
            patterns = new List<Image>();

            IDictionaryEnumerator mIterator = rs.GetEnumerator();
            int count = 0;
            while(mIterator.MoveNext())
            {
                object val = mIterator.Value;
                object key = mIterator.Key;
                if (val is Image)
                {
                    Image img = (Image)val;
                    patterns.Add(img);
                }
                count++;
            }
            Reset();
        }

        public void Reset() { count = 0; }

        public Bitmap GetNextPattern()
        {
            Bitmap b = new Bitmap(PatternEditor.PATTERN_WIDTH, PatternEditor.PATTERN_WIDTH,PixelFormat.Format16bppRgb555);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.FillRectangle(Brushes.White, 0, 0, b.Width, b.Height);
                g.DrawRectangle(Pens.Gray, 0, 0, b.Width, b.Height);

                if(count == 0)
                {
                    // Do nothing, already have a filled white rectangle.
                }
                else if (count == 1)
                {
                    g.FillRectangle(Brushes.Black, 1, 1, b.Width-2, b.Height-2);
                }
                else if (count < patterns.Count + 2)
                {
                    Bitmap bRes = (Bitmap)patterns[count-2];
                    g.DrawImage(bRes, 1, 1);
                }
                else if (count < patterns.Count + 54)
                {
                    int cnt = count - patterns.Count - 2;
                    char c = (char)('A' + cnt);
                    if (cnt >= 26)
                        c = (char)('A' + cnt-26);
                    g.DrawString("" + c, new Font("Courier", 9), new SolidBrush(Color.Black), new Point(0, 0));
                }
            }
            count++;
            return b;
        }
    }
}
