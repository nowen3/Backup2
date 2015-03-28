using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Backup_2
{
    public partial class FRMrestore : Form
    {
        public FRMrestore()
        {
            InitializeComponent();
        }

        private void BTNlocation_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.Description = "Destination Folder";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                TXTlocation.Text = folderBrowserDialog.SelectedPath;
            }

        }

        public string NewLocation
        {
            get { return TXTlocation.Text; }
            set { this.TXTlocation.Text = value; }
        }

        public bool RestoreOriginal
        {
            get { return this.radioButton1.Checked; }
        }

        public bool RestoreOriginalAsk
        {
            get { return this.radioButton2.Checked; }
        }

        public bool Restorenew
        {
            get { return this.radioButton3.Checked; }
        }
    }
}
