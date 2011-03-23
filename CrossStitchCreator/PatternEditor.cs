using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CrossStitchCreator
{
    public partial class PatternEditor : Form
    {
        public const int PATTERN_WIDTH = 12;

        private int drawing = 0;
        private Bitmap mCurrentImage = new Bitmap(PATTERN_WIDTH, PATTERN_WIDTH, PixelFormat.Format16bppRgb555);

        private MainForm mParent;

        private PatternCreator patternCreator = null;


        /// <summary>
        /// Associates each of the colours in the pattern list with the number of times the colour appears.
        /// Will not set if number of entries exceeds MainForm.MAX_COLOURS
        /// </summary>
        public Dictionary<Color, int> Palette
        {
            set
            {
                patternList.Items.Clear();
                if (value.Count > MainForm.MAX_COLOURS)
                {
                    MessageBox.Show("Too many colours!");
                    return;
                }
                patternCreator = new PatternCreator();
                Dictionary<Color, int> temp = new Dictionary<Color, int>(value);
                // loop through all key pairs, adding max frequency first.
                while (temp.Count > 0)
                {
                    int max = int.MinValue;
                    Color maxC = Color.White;
                    foreach (KeyValuePair<Color, int> pair in temp)
                    {
                        if (pair.Value > max)
                        {
                            max = pair.Value;
                            maxC = pair.Key;
                        }
                    }
                    AddPattern(maxC, max);
                    temp.Remove(maxC);
                }
                patternList.Sort();
            }
        }

        public PatternEditor(MainForm parent)
        {
            mParent = parent;
            InitializeComponent();
            sortingCombo.DataSource = Enum.GetValues(typeof(PatternListComparer.SortOptions));
            patternList.SmallImageList = new ImageList();
            patternList.SmallImageList.ImageSize = new Size(PATTERN_WIDTH, PATTERN_WIDTH);
            patternList.ListViewItemSorter = new PatternListComparer();
            patternCreator = new PatternCreator();
            if (parent.OutputImagePalette != null && parent.OutputImagePalette.Count > 0)
            {
                Palette = parent.OutputImagePalette;
                patternList.Items[0].Selected = true;
            }
        }

        public void AddPattern(Color c, int frequency)
        {
            // find out if Color already exists in list.
            foreach (ListViewItem lvi in patternList.Items)
            {
                if (lvi.BackColor == c)
                {
                    lvi.Tag = frequency;
                    return;
                }
            }
            // Colour wasn't in list. Add a new one.
            AddPattern(c, CreateNewPattern(),frequency);
        }
        public void AddPattern(Color c) { AddPattern(c, CreateNewPattern()); }
        public void AddPattern(Color c, Bitmap b)
        {
            AddPattern(c, b, 0);
        }
        public void AddPattern(Color c, Bitmap b, int frequency)
        {
            // find an index i that is unique in patternList.
            int i = 0;
            string key = ("" + i).PadLeft(4, '0');
            bool keyFree = false;
            while (!keyFree)
            {
                keyFree = true;
                foreach (ListViewItem lvi in patternList.Items)
                {
                    key = ("" + i).PadLeft(4, '0');
                    string imageKey = lvi.ImageKey;
                    if (key.Equals(imageKey))
                    {
                        keyFree = false;
                        break;
                    }
                }
                i++;
            }
            AddPattern(c, b, key, frequency);
        }

        public Bitmap GetPattern(Color c)
        {
            foreach (ListViewItem lvi in patternList.Items)
            {
                if (lvi.BackColor.Equals(c))
                {
                    int imageIndex = patternList.SmallImageList.Images.IndexOfKey(lvi.ImageKey);
                    return (Bitmap)patternList.SmallImageList.Images[imageIndex];
                }
            }
            return null;
        }

        public void AddPattern(Color c, Bitmap bm, string imageKey, int frequency)
        {
            ListViewItem lvi = new ListViewItem("    ",imageKey);
            patternList.SmallImageList.Images.Add(imageKey,bm);
            string r = String.Format("{0:X}", c.R);
            string g = String.Format("{0:X}", c.G);
            string b = String.Format("{0:X}", c.B);
            lvi.ToolTipText = String.Format("Colour: 0x" + r.PadLeft(2, '0') + g.PadLeft(2, '0') + b.PadLeft(2, '0')+
                ", frequency: "+frequency+", imageKey: "+lvi.ImageKey);
            lvi.BackColor = c;
            lvi.Tag = frequency;
            patternList.Items.Add(lvi);
        }

        private Bitmap CreateNewPattern()
        {
            return patternCreator.GetNextPattern();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            byte b = (byte)(patternList.Items.Count * 30);
            AddPattern(Color.FromArgb(255, b, b, b));
            patternList.Items[patternList.Items.Count - 1].Selected = true;
        }

        private void setImage(Bitmap b)
        {
            ImagingTools tool = new ImagingTools(b);
            tool.ResizeImage(pictureBox.Size);
            pictureBox.Image = tool.OutputImage;
        }

        private void setListItem(int listIndex)
        {
            if (listIndex >= patternList.Items.Count) return;
            ListViewItem lvi = patternList.Items[listIndex];
            int imageIndex = patternList.SmallImageList.Images.IndexOfKey(lvi.ImageKey);
            Image img = patternList.SmallImageList.Images[imageIndex];
            mCurrentImage = (Bitmap)img;
            setImage((Bitmap)img);
            if (colourBox.Image == null) colourBox.Image = new Bitmap(colourBox.Width, colourBox.Height,PixelFormat.Format16bppRgb555);
            Bitmap b = (Bitmap)colourBox.Image;
            Graphics g = Graphics.FromImage(b);
            g.FillRectangle(new SolidBrush(lvi.BackColor), 0, 0, b.Width,b.Height);
            g.Dispose();
            Refresh();
        }

        #region PictureBoxMouseEvents


        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                drawing = 1;
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                drawing = 2;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing > 0)
            {
                int x = (int)((float)e.X * (float)PATTERN_WIDTH / (float)pictureBox.Width);
                int y = (int)((float)e.Y * (float)PATTERN_WIDTH / (float)pictureBox.Height);
                if (x >= 0 && x < PATTERN_WIDTH && y >= 0 && y < PATTERN_WIDTH)
                {
                    if (drawing == 1) mCurrentImage.SetPixel(x, y, Color.Black);
                    else if (drawing == 2) mCurrentImage.SetPixel(x, y, Color.White);
                    setImage(mCurrentImage);
                }
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawing > 0)
            {
                int x = (int)((float)e.X * (float)PATTERN_WIDTH / (float)pictureBox.Width);
                int y = (int)((float)e.Y * (float)PATTERN_WIDTH / (float)pictureBox.Height);
                if (x >= 0 && x < PATTERN_WIDTH && y >= 0 && y < PATTERN_WIDTH)
                {
                    if (drawing == 1) mCurrentImage.SetPixel(x, y, Color.Black);
                    else if (drawing == 2) mCurrentImage.SetPixel(x, y, Color.White);
                    setImage(mCurrentImage);
                }
            }
            drawing = 0;
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            drawing = 0;
        }
        #endregion

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (patternList.SelectedIndices.Count > 0)
            {
                ListViewItem lvi = patternList.Items[patternList.SelectedIndices[0]];
                int imageIndex = patternList.SmallImageList.Images.IndexOfKey(lvi.ImageKey);
                patternList.SmallImageList.Images[imageIndex] = new Bitmap(mCurrentImage);
                Bitmap b = (Bitmap)colourBox.Image;
                lvi.BackColor = b.GetPixel(0, 0);
                Refresh();
            }
        }

        private void patternList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (patternList.SelectedItems.Count > 0)
            {
                setListItem(patternList.SelectedIndices[0]);
            }
        }

        private void sortingCombo_DropDownClosed(object sender, EventArgs e)
        {
            patternList.ListViewItemSorter = new PatternListComparer((PatternListComparer.SortOptions)sortingCombo.SelectedItem);
            patternList.Sort();
        }

    }

    class PatternListComparer : IComparer
    {
        public enum SortOptions { ByTagAscending, ByTagDescending, ByIndexAscending, ByIndexDescending, ByImageKeyAscending, ByImageKeyDescending };

        public SortOptions SortOption { get; set; }

        public PatternListComparer() : this(SortOptions.ByTagDescending) { }
        public PatternListComparer(SortOptions s) { SortOption = s; }

        public int Compare(object x, object y)
        {
            if (x == null) return 1;
            if (y == null) return -1;
            if (x.GetType() != typeof(ListViewItem)) return 1;
            if(y.GetType() != typeof(ListViewItem)) return -1;
            ListViewItem lx = (ListViewItem)x;
            ListViewItem ly = (ListViewItem)y;

            if (SortOption == SortOptions.ByTagAscending)
            {
                int fx = (int)lx.Tag;
                int fy = (int)ly.Tag;
                return fx.CompareTo(fy);
            } 
            else if (SortOption == SortOptions.ByTagDescending)
            {
                int fx = (int)lx.Tag;
                int fy = (int)ly.Tag;
                return fy.CompareTo(fx);
            }
            else if (SortOption == SortOptions.ByIndexAscending)
            {
                return lx.Index.CompareTo(ly.Index);
            }
            else if (SortOption == SortOptions.ByIndexDescending)
            {
                return ly.Index.CompareTo(lx.Index);
            }
            else if (SortOption == SortOptions.ByImageKeyAscending)
            {
                return lx.ImageKey.CompareTo(ly.ImageKey);
            }
            else if (SortOption == SortOptions.ByImageKeyDescending)
            {
                return ly.ImageKey.CompareTo(lx.ImageKey);
            }
            return 0;
        }
    }
}
