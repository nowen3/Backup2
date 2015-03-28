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
    public partial class FRMinfo : Form
    {
        public FRMinfo()
        {
            InitializeComponent();
        }

        public void setlab(string action)
        {
            label.Text = action;
        }

        public void setlab1(string action)
        {
            label1.Text = action;
        }

        public void setprog(int count)
        {
            progressBar1.Value = count;
        }

        public void setmax(int count)
        {
            progressBar1.Maximum = count;
        }
    }
}
