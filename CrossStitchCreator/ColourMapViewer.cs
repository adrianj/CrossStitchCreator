using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CrossStitchCreator
{
    public partial class ColourMapViewer : Form
    {

        private IColourMap mColourMap;
        private bool mReplacing = false;
        private Color mColorToReplace;
        private bool enableEvents = false;

        public ColourMapViewer(IColourMap cmap)
        {
            InitializeComponent();
            UpdateColourMap(cmap);
        }

        public void UpdateColourMap(IColourMap cmap)
        {
            enableEvents = false;
            mColourMap = cmap;
            if (mColourMap.Count > 0)
            {
                listView.Items.Clear();
                foreach (KeyValuePair<Color, IColourInfo> pair in mColourMap.Colours)
                {
                    ListViewItem lvi = new ListViewItem(pair.Value.Name);
                    lvi.BackColor = pair.Value.Colour;
                    lvi.Checked = pair.Value.IsChecked;
                    lvi.Tag = pair.Value;
                    listView.Items.Add(lvi);
                    
                }
                enableEvents = true;
                listView.Items[0].Selected = true;
            }
            enableEvents = true;
        }

        public void SelectColour(Color colour)
        {
            foreach(ListViewItem lvi in listView.Items)
            {
                if (lvi.BackColor == colour) lvi.Selected = true;
            }

        }
        private void updateInfo(ListViewItem lvi)
        {
            if (lvi == null)
            {
                pictureBox.Image = null;
                infoBox.Text = "";
                return;
            }
            IColourInfo c = (IColourInfo)lvi.Tag;
            //c.IsChecked = lvi.Checked;
            infoBox.Text = c.PrintInfo();
            statusLabel.Text = c.Name;
            Bitmap b = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics g = Graphics.FromImage(b);
            g.FillRectangle(new SolidBrush(lvi.BackColor), 0, 0, b.Width, b.Height);
            g.Dispose();
            pictureBox.Image = b;
        }

        private void listView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (enableEvents)
            {
                IColourInfo c = (IColourInfo)e.Item.Tag;
                c.IsChecked = e.Item.Checked;
                updateInfo(e.Item);
            }
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedIndices.Count < 1) return;
            ListViewItem lvi = listView.SelectedItems[0];
            if (mReplacing)
            {
                mReplacing = false;
                OnColourChangeEvent(new ColourChangeEventArgs(mColorToReplace, lvi.BackColor));
            }
            updateInfo(lvi);
            
        }

        private void listView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (listView.SelectedIndices.Count < 1) return;
                ListViewItem lvi = listView.SelectedItems[0];
                if(!lvi.Checked) OnColourChangeEvent(new ColourChangeEventArgs(lvi.BackColor));
            }
        }

        public event ColourChangeEventHandler ColourChangeEvent;
        public virtual void OnColourChangeEvent(ColourChangeEventArgs e)
        {
            ColourChangeEvent(this, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices.Count < 1) return;
            ListViewItem lvi = listView.SelectedItems[0];
            if(!lvi.Checked) OnColourChangeEvent(new ColourChangeEventArgs(lvi.BackColor));
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices.Count < 1) return;
            ListViewItem lvi = listView.SelectedItems[0];
            if (!lvi.Checked)
            {
                mReplacing = true;
                mColorToReplace = lvi.BackColor;
                statusLabel.Text = "Select Colour to replace this one";
            }
        }
    }

    public class ColourChangeEventArgs : EventArgs
    {
        private Color mColour;
        public Color Colour { get { return mColour; } }
        private Color mReplacement;
        public Color ReplacementColour {get { return mReplacement;}}
        public bool DoReplace { get { return mDoReplace; } }
        private bool mDoReplace = false;

        public ColourChangeEventArgs(Color colour) { mColour = colour; }
        public ColourChangeEventArgs(Color colour, Color replacement) { 
            mColour = colour;
            mDoReplace = true;
            mReplacement = replacement;
        }
    }
    public delegate void ColourChangeEventHandler(object sender, ColourChangeEventArgs e);
}
