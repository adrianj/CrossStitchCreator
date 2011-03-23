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
        private IDictionaryEnumerator mIterator;

        public PatternCreator()
        {
            string[] res = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            foreach (string s in res)
                Console.WriteLine("Resource: " + s);


            ResourceManager rm = Properties.Resources.ResourceManager;
            ResourceSet rs = rm.GetResourceSet(new CultureInfo("en-US"), true, true);
            
            mIterator = rs.GetEnumerator();
            int count = 0;
            while(mIterator.MoveNext())
            {
                object val = mIterator.Value;
                object key = mIterator.Key;
                Console.WriteLine("Inside rs: ("+key+") "+val);
                if (val is Image)
                {
                    Image img = (Image)val;
                    Console.WriteLine("Found an image! " + img.Size);
                }
                count++;
            }
            Console.WriteLine("count: " + count);
            mIterator = rs.GetEnumerator();


        }

        public Bitmap GetNextPattern()
        {
            if (mIterator.MoveNext())
            {
                Bitmap b = (Bitmap)mIterator.Value;
                return b;
            }
            else if (count < 52)
            {

                Bitmap b = new Bitmap(PatternEditor.PATTERN_WIDTH, PatternEditor.PATTERN_WIDTH);
                Graphics g = Graphics.FromImage(b);
                g.FillRectangle(Brushes.White, 0, 0, b.Width, b.Height);
                char c = 'a';
                if(count < 26)
                    c = (char)('A' +count);
                else
                    c = (char)('a' + (count-26));
                g.DrawLine(Pens.Black, 0, 0, b.Width, 0);
                g.DrawLine(Pens.Black, 0, 0, 0,b.Height);
                g.DrawString(""+c, new Font("Courier", 8), new SolidBrush(Color.Black), new Point(0, 0));
                g.Dispose();
                count++;
                return b;
            }
            else
            {
                Bitmap b = new Bitmap(PatternEditor.PATTERN_WIDTH, PatternEditor.PATTERN_WIDTH, PixelFormat.Format16bppRgb555);
                Graphics g = Graphics.FromImage(b);
                g.FillRectangle(Brushes.White, 0, 0, b.Width, b.Height);
                g.DrawRectangle(Pens.Black, 0, 0, b.Width - 1, b.Height - 1);
                return b;
            }
        }
    }
}
