using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CrossStitchCreator
{
    [Serializable()]
    class CrossStitchSettings
    {
        private int mMaxColours = MainForm.MAX_COLOURS;
        public int MaxColours { get { return mMaxColours; } set { if (value >= 2 && value <= MainForm.MAX_COLOURS) mMaxColours = value; } }
        public static string FileExtension = "cse";
        public string ProjectPath { get; set; }
        public string InputImagePath { get; set; }
        public string OutputImagePath { get; set; }
        private bool mFixRatio = false;
        public bool FixSizeRatio { get { return mFixRatio; } set { mFixRatio = value; DoRatioFixing(true); } }
        public Size InputImageSize { get; set; }
        private Size mOutputSize = new Size(64, 48);
        public Size OutputImageSize
        {
            get { return mOutputSize; }
            set
            {
                    OutputHeight = value.Height;
                    OutputWidth = value.Width;
            }
        }
        public int OutputHeight
        {
            get { return mOutputSize.Height; }
            set
            {
                if (value > 0)
                {
                    mOutputSize.Height = value;
                    DoRatioFixing(false);
                }
            }
        }

        public int OutputWidth
        {
            get { return mOutputSize.Width; }
            set
            {
                if (value > 0)
                {
                    mOutputSize.Width = value;
                    DoRatioFixing(true);
                }
            }
        }

        public InterpolationMode IntMode { get; set; }

        public CrossStitchSettings() { }

        private void DoRatioFixing(bool keepWidth)
        {
            if (FixSizeRatio && mOutputSize.Width > 0 && mOutputSize.Height > 0
                && InputImageSize.Width > 0 && InputImageSize.Height > 0)
            {
                float ratio = (float)InputImageSize.Width / (float)InputImageSize.Height;
                if (keepWidth)
                {
                    mOutputSize.Height = (int)((float)mOutputSize.Width / ratio);
                }
                else
                {
                    mOutputSize.Width = (int)((float)mOutputSize.Height * ratio);
                }
            }
        }

        public bool WriteToFile()
        {

            return true;
        }
    }
}
