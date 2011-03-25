using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace CrossStitchCreator
{



    public class DMCColourMap : SimpleColourMap
    {

        /// <summary>
        /// Initialises the colour map from a filename pointing to the .csv file with RGB values listed.
        /// </summary>
        /// <param name="filename"></param>
        public DMCColourMap() : this(false) { }

        public DMCColourMap(bool createEmpty)
        {
            Colours = new Dictionary<Color, IColourInfo>();
            if (!createEmpty)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(DMCColours.DmcToRgb);
                MemoryStream stream = new MemoryStream(bytes);
                CSVReader reader = new CSVReader(stream);
                string[][] text = reader.ReadFile();

                for (int i = 0; i < reader.NumberOfRows; i++)
                {
                    int r = parse(text[2][i]);
                    int g = parse(text[3][i]);
                    int b = parse(text[4][i]);
                    string name = text[1][i];
                    string dmcNum = text[0][i];
                    Color c = Color.FromArgb(255, r, g, b);
                    if (!Colours.ContainsKey(c)) Colours[c] = (new DMCColour(name, i, c, dmcNum));
                }
            }
        }

        private int parseFailures = 0;

        private int parse(string s)
        {
            try
            {
                return int.Parse(s);
            }
            catch
            {
                return parseFailures++;
            }
        }

        public override IColourMap Clone()
        {
            IColourMap cmap = new DMCColourMap(true);
            foreach (KeyValuePair<Color, IColourInfo> pair in Colours)
            {
                cmap.Colours[pair.Key] = pair.Value.Clone();
            }
            return cmap;
        }
    }

    public class DMCColour : SimpleColour
    {
        public string DMCNumber { get; set; }
        public string AnchorNumber { get; set; }

        public DMCColour(string name, int index, Color colour, string dmcNum) : base(name,index,colour)
        {
            DMCNumber = dmcNum;
        }
        public DMCColour(DMCColour d) :base(d)
        {
            DMCNumber = d.DMCNumber;
            AnchorNumber = d.AnchorNumber;
        }

        public override string PrintInfo()
        {
            string s = base.PrintInfo();

            return s+Environment.NewLine+"DMC: "+DMCNumber+", Anchor: "+AnchorNumber;
        }

        public override IColourInfo Clone()
        {
            return new DMCColour(this);
        }
    }
}
