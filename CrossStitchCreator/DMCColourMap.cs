using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace CrossStitchCreator
{

    public interface ColourMap
    {
        Color GetNearestColour(Color c);
        /// <summary>
        /// Finds the least common colour. If parameter matchWhenChecked is true, then the search will ignore ColourInfos that are checked
        /// (typically to indicate that this colour is not to be removed).
        /// </summary>
        /// <param name="matchWhenChecked"></param>
        /// <returns></returns>
        Color GetLeastCommonColour(bool matchWhenChecked);
        void ClearFrequencies();
        void RemoveColour(Color c);
        Dictionary<Color,ColourInfo> Colours { get; set; }
        int Count { get; }
        /// <summary>
        /// Returns a (preferably deep) clone of the ColourMap array.
        /// </summary>
        /// <returns></returns>
        ColourInfo[] ToArray();
        /// <summary>
        /// Returns a (preferably deep) cloned list of the ColourMap array.
        /// </summary>
        /// <returns></returns>
        List<ColourInfo> ToList();
    }

    public interface ColourInfo
    {
        Color Colour { get; set; }
        string Name { get; set; }
        bool IsChecked { get; set; }
        string PrintInfo();
        object Tag { get; set; }
        int Frequency { get; set; }
        ColourInfo Clone();
    }

    public class DMCColourMap : ColourMap
    {
        public Dictionary<Color,ColourInfo> Colours { get; set; }
        public int Count { get { return Colours.Count; } }

        /// <summary>
        /// Initialises the colour map from a filename pointing to the .csv file with RGB values listed.
        /// </summary>
        /// <param name="filename"></param>
        public DMCColourMap()
        {
            byte[] bytes = Encoding.ASCII.GetBytes(DMCColours.DmcToRgb);
            MemoryStream stream = new MemoryStream(bytes);
            CSVReader reader = new CSVReader(stream);
            string[][] text = reader.ReadFile();

            Colours = new Dictionary<Color,ColourInfo>();
            for (int i = 0; i < reader.NumberOfRows; i++)
            {
                int r = parse(text[2][i]);
                int g = parse(text[3][i]);
                int b = parse(text[4][i]);
                string name = text[1][i];
                string dmcNum = text[0][i];
                Color c = Color.FromArgb(255, r, g, b);
                Colours[c] = (new DMCColour(name, i, c, dmcNum));
            }
        }


        public Color GetNearestColour(Color colourToMatch)
        {
            byte minDiff = byte.MaxValue;
            Color c = Color.Black;
            foreach (KeyValuePair<Color,ColourInfo> col in Colours)
            {
                byte diff = getMaxDiff(colourToMatch, col.Key);
                if (diff < minDiff)
                {
                    minDiff = diff;
                    c = col.Key;
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

        /// <summary>
        /// Finds the least common colour. If parameter matchWhenChecked is true, then the search will ignore ColourInfos that are checked
        /// (typically to indicate that this colour is not to be removed).
        /// </summary>
        /// <param name="matchWhenChecked"></param>
        /// <returns></returns>
        public Color GetLeastCommonColour(bool matchWhenChecked)
        {
            if (Colours.Count == 0) return Color.Black;
            Color c = Color.Black;
            int min = int.MaxValue;
            foreach (KeyValuePair<Color,ColourInfo> col in Colours)
            {
                if (matchWhenChecked && col.Value.IsChecked) continue;
                // can quit early if the frequency is 1.
                if (col.Value.Frequency == 1)
                    return col.Key;
                if (col.Value.Frequency < min)
                {
                    min = col.Value.Frequency;
                    c = col.Key;
                }
            }
            return c;
        }

        private int parseFailures = 0; 

        private int parse(string s)
        {
            try
            {
                return int.Parse(s);
            }
            catch {
                return parseFailures++;
            }
        }

        public ColourInfo[] ToArray()
        {
            ColourInfo[] ret = new ColourInfo[Colours.Count];
            int i = 0;
            foreach(KeyValuePair<Color,ColourInfo> pair in Colours)
            {
                ret[i] = pair.Value.Clone();
                i++;
            }
            return ret;
        }
        public List<ColourInfo> ToList()
        {
            List<ColourInfo> ret = new List<ColourInfo>();
            foreach (KeyValuePair<Color, ColourInfo> pair in Colours)
                ret.Add(pair.Value.Clone());
            return ret;
        }

        public void RemoveColour(Color c)
        {
            if (Colours.ContainsKey(c)) Colours.Remove(c);
        }

        public void ClearFrequencies()
        {
            foreach (KeyValuePair<Color, ColourInfo> pair in Colours)
                pair.Value.Frequency = 0;
        }
    }

    public class DMCColour : ColourInfo
    {
        public Color Colour { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string DMCNumber { get; set; }
        public bool IsChecked { get; set; }
        public string AnchorNumber { get; set; }
        public object Tag { get; set; }
        public int Frequency { get; set; }

        public DMCColour(string name, int index, Color colour, string dmcNum)
        {
            Name = name;
            Index = index;
            Colour = colour;
            DMCNumber = dmcNum;
        }
        public DMCColour(DMCColour d)
        {
            Name = d.Name;
            Index = d.Index;
            Colour = d.Colour;
            DMCNumber = d.DMCNumber;
            Tag = d.Tag;
            AnchorNumber = d.AnchorNumber;
            Frequency = d.Frequency;
        }

        public string PrintInfo()
        {
            return Name+Environment.NewLine+
                "Checked: "+IsChecked+Environment.NewLine+
                "Tag: "+Tag+Environment.NewLine+
                "Colour: " + Colour + Environment.NewLine +
                "Frequency: " + Frequency + Environment.NewLine +
                "Index: " + Index + Environment.NewLine +
                "DMC: "+DMCNumber+", Anchor: "+AnchorNumber;
        }

        public ColourInfo Clone()
        {
            return new DMCColour(this);
        }
    }
}
