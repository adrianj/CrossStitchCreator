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


            ResourceManager rm = Properties.Patterns.ResourceManager;
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
            Bitmap b = new Bitmap(PatternEditor.PATTERN_WIDTH, PatternEditor.PATTERN_WIDTH);
            Graphics g = Graphics.FromImage(b);
            g.FillRectangle(Brushes.White, 0, 0, b.Width, b.Height);
            g.DrawLine(Pens.Black, 0, 0, b.Width, 0);
            g.DrawLine(Pens.Black, 0, 0, 0, b.Height);
            if (mIterator.MoveNext())
            {
                Bitmap bRes = (Bitmap)mIterator.Value;
                g.DrawImage(bRes, 1, 1);
            }
            else if (count < 52)
            {
                char c = 'a';
                if(count < 26)
                    c = (char)('A' +count);
                else
                    c = (char)('a' + (count-26));
                g.DrawString(""+c, new Font("Courier", 8), new SolidBrush(Color.Black), new Point(0, 0));
                count++;
            }
            g.Dispose();
            return b;
        }
    }
}
