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

        public ColourMap ColourMap;

        public ColourMapViewer(ColourMap map)
        {
            ColourMap = map;
            InitializeComponent();
            UpdateColourMap();
        }

        public void UpdateColourMap()
        {
            if (ColourMap.Count > 0)
            {
                listView.Items.Clear();
                foreach (KeyValuePair<Color, ColourInfo> pair in ColourMap.Colours)
                {
                    ListViewItem lvi = new ListViewItem(pair.Value.Name);
                    lvi.BackColor = pair.Value.Colour;
                    lvi.Checked = pair.Value.IsChecked;
                    lvi.Tag = pair.Value;
                    listView.Items.Add(lvi);
                }
                listView.Items[0].Selected = true;
            }
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
            ColourInfo c = (ColourInfo)lvi.Tag;
            c.IsChecked = lvi.Checked;
            infoBox.Text = c.PrintInfo();
            Bitmap b = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics g = Graphics.FromImage(b);
            g.FillRectangle(new SolidBrush(lvi.BackColor), 0, 0, b.Width, b.Height);
            g.Dispose();
            pictureBox.Image = b;
        }

        private void listView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            updateInfo(e.Item);
           
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedIndices.Count < 1) return;
            ListViewItem lvi = listView.SelectedItems[0];
            updateInfo(lvi);
            
        }

        private void listView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (listView.SelectedIndices.Count < 1) return;
                ListViewItem lvi = listView.SelectedItems[0];
                OnColourDeleteEvent(new ColourDeleteEventArgs(lvi.BackColor));
            }
        }

        public event ColourDeleteEventHandler ColourDeleteEvent;
        public virtual void OnColourDeleteEvent(ColourDeleteEventArgs e)
        {
            ColourDeleteEvent(this, e);
        }
    }

    public class ColourDeleteEventArgs : EventArgs
    {
        private Color mColour;
        public Color Colour { get { return mColour; } }
        public ColourDeleteEventArgs(Color colour) { mColour = colour; }
    }
    public delegate void ColourDeleteEventHandler(object sender, ColourDeleteEventArgs e);
}
