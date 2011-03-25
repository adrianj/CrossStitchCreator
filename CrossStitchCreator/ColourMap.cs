using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CrossStitchCreator
{
    public interface IColourMap
    {
        Color GetNearestColour(Color c);
        /// <summary>
        /// Finds the least common colour. If parameter matchWhenChecked is true, then the search will ignore ColourInfos that are checked
        /// (typically to indicate that this colour is not to be removed).
        /// </summary>
        /// <param name="matchWhenChecked"></param>
        /// <returns></returns>
        Color GetLeastCommonColour(bool matchWhenChecked);
        IColourInfo GetLeastCommonColourInfo(bool matchWhenChecked);
        void ClearFrequencies();
        void RemoveColour(Color c);
        Dictionary<Color, IColourInfo> Colours { get; set; }
        int Count { get; }
        /// <summary>
        /// Returns a (preferably deep) clone of the ColourMap array.
        /// </summary>
        /// <returns></returns>
        IColourInfo[] ToArray();
        /// <summary>
        /// Returns a (preferably deep) cloned list of the ColourMap array.
        /// </summary>
        /// <returns></returns>
        List<IColourInfo> ToList();
        List<Color> GetPalette();
        IColourMap Clone();
    }

    public interface IColourInfo
    {
        Color Colour { get; set; }
        string Name { get; set; }
        bool IsChecked { get; set; }
        string PrintInfo();
        object Tag { get; set; }
        int Frequency { get; set; }
        IColourInfo Clone();
    }
    /// <summary>
    /// A simple colourmap that implements RGB444
    /// </summary>
    public class SimpleColourMap : IColourMap
    {
        public Dictionary<Color, IColourInfo> Colours { get; set; }
        public int Count { get { return Colours.Count; } }

        public SimpleColourMap() : this(false) { }

        public SimpleColourMap(bool createEmpty)
        {
            Colours = new Dictionary<Color, IColourInfo>();
            if (!createEmpty)
            {
                Console.WriteLine("Constructing?");
                int index = 0;
                for (int r = 0; r < 256; r += 16)
                    for (int g = 0; g < 256; g += 16)
                        for (int b = 0; b < 256; b += 16)
                        {
                            Color c = Color.FromArgb(255, r, g, b);
                            SimpleColour s = new SimpleColour("" + c, index, c);
                            Colours[c] = s;
                        }
            }
        }

        public virtual IColourMap Clone()
        {
            IColourMap cmap = new SimpleColourMap(true);
            Colours = new Dictionary<Color, IColourInfo>();
            foreach (KeyValuePair<Color, IColourInfo> pair in cmap.Colours)
            {
                Colours[pair.Key] = pair.Value.Clone();
            }
            return cmap;
        }

        public Color GetNearestColour(Color colourToMatch)
        {
            byte minDiff = byte.MaxValue;
            Color c = Color.Black;
            foreach (KeyValuePair<Color, IColourInfo> col in Colours)
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
            foreach (KeyValuePair<Color, IColourInfo> col in Colours)
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

        public IColourInfo GetLeastCommonColourInfo(bool matchWhenChecked)
        {
            Color c = GetLeastCommonColour(matchWhenChecked);
            if (Colours.ContainsKey(c)) return Colours[c];
            return null;

        }

       

        public IColourInfo[] ToArray()
        {
            IColourInfo[] ret = new IColourInfo[Colours.Count];
            int i = 0;
            foreach (KeyValuePair<Color, IColourInfo> pair in Colours)
            {
                ret[i] = pair.Value.Clone();
                i++;
            }
            return ret;
        }
        public List<IColourInfo> ToList()
        {
            List<IColourInfo> ret = new List<IColourInfo>();
            foreach (KeyValuePair<Color, IColourInfo> pair in Colours)
                ret.Add(pair.Value.Clone());
            return ret;
        }

        public void RemoveColour(Color c)
        {
            if (c == Color.FromArgb(255, 255, 255, 255))
                Console.WriteLine("Deleting white!");
            if (Colours.ContainsKey(c)) Colours.Remove(c);
        }

        public void ClearFrequencies()
        {
            foreach (KeyValuePair<Color, IColourInfo> pair in Colours)
                pair.Value.Frequency = 0;
        }

        public List<Color> GetPalette()
        {
            return Colours.Keys.ToList();
        }
    }

    public class SimpleColour : IColourInfo
    {
        public Color Colour { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        private bool mChecked = false;
        public bool IsChecked
        {
            get
            {
                return mChecked;
            }
            set
            {
                mChecked = value;
            }
        }
        public object Tag { get; set; }
        public int Frequency { get; set; }

        public SimpleColour(string name, int index, Color colour)
        {
            Name = name;
            Index = index;
            Colour = colour;
        }
        public SimpleColour(SimpleColour d)
        {
            Name = d.Name;
            Index = d.Index;
            Colour = d.Colour;
            Tag = d.Tag;
            Frequency = d.Frequency;
            IsChecked = d.IsChecked;
        }

        public virtual string PrintInfo()
        {
            return Name + Environment.NewLine +
                "Checked: " + IsChecked + Environment.NewLine +
                "Tag: " + Tag + Environment.NewLine +
                "Colour: " + Colour + Environment.NewLine +
                "Frequency: " + Frequency + Environment.NewLine +
                "Index: " + Index + Environment.NewLine;
        }

        public virtual IColourInfo Clone()
        {
            return new SimpleColour(this);
        }
    }
}
