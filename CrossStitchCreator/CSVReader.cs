using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CrossStitchCreator
{
    /// <summary>
    /// A class for reading .CSV files
    /// </summary>
    public class CSVReader
    {

        public string[] FirstRow { get; set; }
        /// <summary>
        /// The number of rows, NOT including the first row that has header information.
        /// </summary>
        public int NumberOfRows { get; set; }
        public int NumberOfColumns { get; set; }

        private StreamReader mStream;

        /// <summary>
        /// Initialises a new CSVReader and sets up a FileStream on the given filename.
        /// Will throw some kind of Exception if the filename is no good.
        /// </summary>
        /// <param name="filename"></param>
        public CSVReader(string filename)
        {
            mStream = new StreamReader(filename);
            countRows();
        }

        /// <summary>
        /// Initialises a new CVSReader around the given Stream.
        /// </summary>
        /// <param name="stream"></param>
        public CSVReader(Stream stream)
        {
            mStream = new StreamReader(stream);
            countRows();
        }

        

        /// <summary>
        /// Reinitialises the stream back to the beginning
        /// </summary>
        private void initialise()
        {
            if (mStream.BaseStream.CanSeek) mStream.BaseStream.Seek(0, SeekOrigin.Begin);
            mStream.DiscardBufferedData();
        }

        /// <summary>
        /// Reads through the stream once and populates properties like NumberOfRows, NumberOfColumns and FirstRow.
        /// </summary>
        private void countRows()
        {
            initialise();
            NumberOfRows = -1;
            while (!mStream.EndOfStream)
            {
                string[] s = readLine();
                NumberOfRows++;
                if (NumberOfRows == 0)
                {
                    FirstRow = s;
                    NumberOfColumns = FirstRow.Length;
                }
            }
        }

        /// <summary>
        /// Reads the file, and returns an array of strings organised [Column][Row]
        /// </summary>
        /// <returns>a 2Dimensional array structured [Column][Row] of decimal values. Values that could not be parsed are returned as -1.</returns>
        public string[][] ReadFile()
        {
            initialise();
            string [][] ret = new string[NumberOfColumns][];
            for (int c = 0; c < NumberOfColumns; c++) ret[c] = new string[NumberOfRows];

            int row = 0;
            // skip first line
            mStream.ReadLine();
            while (!mStream.EndOfStream)
            {
                string[] s = readLine();
                for (int i = 0; i < s.Length; i++) ret[i][row] = s[i];
                row++;
            }
            initialise();
            return ret;
        }




        private string[] readLine()
        {
            string s = mStream.ReadLine();
            return s.Split(new char[] { ',' });
        }
    }
}
