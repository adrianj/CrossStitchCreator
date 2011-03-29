using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AdriansLib;
using System.ComponentModel;
using System.Threading;

namespace CrossStitchCreator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ProgressBarForm f = new ProgressBarForm("Yay", 20);
            f.StartWorker(operate,20);
            Console.WriteLine("" + f.Cancelled + "," + f.Result);
            Application.Run(new MainForm());
        }

        public static object operate(object p, BackgroundWorker w, DoWorkEventArgs e)
        {
            int d = (int)p;
            int i = 0;
            while (!w.CancellationPending && i < d)
            {
                w.ReportProgress(i);
                e.Cancel = true;
                Thread.Sleep(100);
                i++;
            }
            return i;
        }

    }
}
