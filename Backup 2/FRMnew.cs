using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Drawing;

namespace Backup_2

{    public partial class FRMnew : Form
    {
        private string conbackup = Path.GetDirectoryName(Application.ExecutablePath) + "\\CONBackup.exe";
        private bool disable = false;
        string originaltask = string.Empty;

    public FRMnew()
        {
            InitializeComponent();
            timePicker.Format = DateTimePickerFormat.Custom;
            timePicker.CustomFormat = "HH:mm"; // Only use hours and minutes
            timePicker.ShowUpDown = true;
            if (RBdiff.Checked == true)
            { NUMdiffnumber.Enabled = true; }
            else NUMdiffnumber.Enabled = false;
            
        }

    public string Addexclude
    {
        set
        {
            LBexclude.Items.Add(value);
        }
    }

    public string AddexcludeExtn
    {
        set
        {
            LBexclude.Items.Add(value);
        }
    }

    public string Addinclude
    {
        set
        {
            LBinclude.Items.Add(value);
        }
    }

    public string BackupDestination
    {
        get { return TXBdest.Text; }
        set { this.TXBdest.Text = value; }
    }

    public string BackupName
    {
        get { return TXBbackname.Text; }
        set { this.TXBbackname.Text = value; }
    }

    public string BackupPath
    {
        get { return TXBdest.Text + "\\" + Path.GetFileName(TXBbackname.Text); }
    }

    public string BackupSource
    {
        get { return TXBsource.Text; }
        set { this.TXBsource.Text = value; }
    }

    public bool completebackup
    {
        get { return RBcomplete.Checked; }
        set { this.RBcomplete.Checked = value; }
    }

    public string compression
    {
        get { return COMBcompression.Text; }
        set { this.COMBcompression.Text = value; }
    }

    public bool diffbackup
    {
        get { return RBdiff.Checked; }
        set { this.RBdiff.Checked = value; }
    }

    public bool DisableControls
    {
        set { disable = value; }
    }

    public bool EnableButton
    {
        set
        {
            BTNupdate.Enabled = value;
            BTNdelete.Enabled = value;
            if (value == false)
            {
            button1.Text = "Backup";
            }
            else button1.Text = "OK";
        }
    }

    public List<string> ExcludeDirs
    {

        get
        {
            var exdir = new List<string>();
            foreach (string s in LBexclude.Items)
            {
                if (Directory.Exists(s))
                {
                    exdir.Add(s);
                }

            }
            return exdir;
        }
    }

    public List<string> ExcludeExtn
    {

        get
        {
            var exextn = new List<string>();
            foreach (string s in LBexclude.Items)
            {
                if (!File.Exists(s) & !Directory.Exists(s))
                {
                    exextn.Add(s);
                }

            }
            return exextn;
        }
    }

    public List<string> ExcludeFiles
    {

        get
        {
            var exfiles = new List<string>();
            foreach (string s in LBexclude.Items)
            {
                if (File.Exists(s))
                {
                    exfiles.Add(s);
                }

            }
            return exfiles;
        }
    }

    public List<string> IncludeDirs
    {

        get
        {
            var indir = new List<string>();
            foreach (string s in LBinclude.Items)
            {
                if (Directory.Exists(s))
                {
                    indir.Add(s);
                }

            }
            return indir;
        }
    }

    public List<string> IncludeFiles
    {

        get
        {
            var infiles = new List<string>();
            foreach (string s in LBinclude.Items)
            {
                if (File.Exists(s))
                {
                    infiles.Add(s);
                }

            }
            return infiles;
        }
    }

    public Dictionary<string, string> BeforeBackup
    {
        get
        {
            Dictionary<string, string> beforeDictionary = new Dictionary<string, string>();
            foreach (ListViewItem item in LSVbefore.Items)
            {
             beforeDictionary.Add(item.SubItems[0].Text, item.SubItems[1].Text);
            }
            return beforeDictionary;

        }
        set
            {
            ListViewItem lvi;
            var aHeaders = new string[2];
            
            foreach (KeyValuePair<string, string> entry in value)
                {
                    aHeaders[0] = entry.Key;
                    aHeaders[1] = entry.Value;
                    lvi = new ListViewItem(aHeaders);
                    LSVbefore.Items.Add(lvi);
                }

            }

    }

    public Dictionary<string, string> AfterBackup
    {
        get
        {
            Dictionary<string, string> afterDictionary = new Dictionary<string, string>();
            foreach (ListViewItem item in LSVafter.Items)
            {
                afterDictionary.Add(item.SubItems[0].Text, item.SubItems[1].Text);
            }
            return afterDictionary;

        }
        set
        {
            ListViewItem lvi;
            var aHeaders = new string[2];

            foreach (KeyValuePair<string, string> entry in value)
            {
                aHeaders[0] = entry.Key;
                aHeaders[1] = entry.Value;
                lvi = new ListViewItem(aHeaders);
                LSVafter.Items.Add(lvi);
            }

        }

    }

    public decimal Logsize
    {
        get { return NUMlog.Value; }
        set { this.NUMlog.Value = value; }
    }

    public int NumberDiffFiles
    {
        get { return (int)NUMdiffnumber.Value; }
        set { this.NUMdiffnumber.Value = (decimal)value; }

    }

    public bool Recursive
    {
        get { return CHBrecu.Checked; }
    }

    public string Schedule
    {
        get { return COMBschedule.Text; }
        set { COMBschedule.Text = value; }
    }

    public string ScheduleOn
    {
        get { return COMBon.Text; }
        set { COMBon.Text = value; }
    }

    public string ScheduleTaskName
    {
        get { return TXTTaskDefinitionName.Text; }
        set
        {
            TXTTaskDefinitionName.Text = value;
            originaltask = value;
        }
    }

    public string CopyBackupFile
    {
        get { return TXTbackupcopy.Text; }
        set { TXTbackupcopy.Text = value; }
    }

    public string ScheduleTime
    {
        get { return timePicker.Value.ToShortTimeString(); }
        set { timePicker.Text = value; }
    }

    public void Togglecontrols()
    {

        TXBbackname.ReadOnly = !TXBbackname.ReadOnly;
        TXBsource.ReadOnly = !TXBsource.ReadOnly;
        TXBdest.ReadOnly = !TXBdest.ReadOnly;
        BTNsource.Enabled = !BTNsource.Enabled;
        BTNdest.Enabled = !BTNdest.Enabled;
    }

    private void BTNbackupcopy_Click(object sender, EventArgs e)
    {
        folderBrowserDialog.Description = "Backup Copy Folder";

        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        {
            TXTbackupcopy.Text = folderBrowserDialog.SelectedPath + "\\";
        }
    }

    private void BTNdelete_Click(object sender, EventArgs e)
    {
        var schedule = new MySchedule(TXTTaskDefinitionName.Text);
        if (schedule.DeleteTask())
        {
            MessageBox.Show("Task Deleted");
            //delete records in inixml file
            string fname = Path.GetDirectoryName(Application.ExecutablePath) + "\\Jobs\\" + Path.GetFileName(TXBbackname.Text);
            var inibackup = new XMLconfig(fname);
            inibackup.DeleteElement("Schedule");
            inibackup.DeleteElement("ScheduleOn");
            inibackup.DeleteElement("TaskDefination");
            inibackup.DeleteElement("ScheduleTime");


        }
    }

    private void BTNdest_Click(object sender, EventArgs e)
    {
        folderBrowserDialog.Description = "Destination Folder";

        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        {
            TXBdest.Text = folderBrowserDialog.SelectedPath;
        }
    }

    private void BTNincludedir_Click(object sender, EventArgs e)
    {
        folderBrowserDialog.Description = "Include Folder";

        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        {
            LBinclude.Items.Add(folderBrowserDialog.SelectedPath + "\\");
        }

    }

    private void BTNincludefile_Click(object sender, EventArgs e)
    {
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {

            if (File.Exists(openFileDialog.FileName))
                LBinclude.Items.Add(openFileDialog.FileName);
        }

    }

    private void BTNremove1_Click(object sender, EventArgs e)
    {
        if (LBinclude.SelectedItems.Count == 0) { return; }
        try
        {
            LBinclude.Items.Remove(LBexclude.SelectedItem.ToString());
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }

    private void BTNremovedir_Click(object sender, EventArgs e)
    {
        try
        {
            folderBrowserDialog.Description = "Exclude Folder";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                LBexclude.Items.Add(folderBrowserDialog.SelectedPath + "\\");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }

    private void BTNremoveextn_Click(object sender, EventArgs e)
    {
        string value = "";
        if (MyInputBox.InputBox("Remove File Extn", "Extn Name ie tmp:", ref value) == DialogResult.OK & value != "")
        {
            LBexclude.Items.Add(value);
        }
    }

    private void BTNremovefile_Click(object sender, EventArgs e)
    {
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            if (File.Exists(openFileDialog.FileName))
                LBexclude.Items.Add(openFileDialog.FileName);
        }
    }

    private void BTNsource_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.Description = "Source Folder";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                TXBsource.Text = folderBrowserDialog.SelectedPath;
            }
        }
    

    private void BTremove_Click(object sender, EventArgs e)
        {
            if (LBexclude.SelectedItems.Count == 0) { return; }
            try
            {
                LBexclude.Items.Remove(LBexclude.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void COMBschedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] week = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            if (COMBschedule.Text == "Daily")
            {
                COMBon.Enabled = false;
                COMBon.Text = "";
                //COMBon.Items.Clear();
                // COMBon.DataSource = Enumerable.Range(1, 30).ToArray();
            }

            if (COMBschedule.Text == "Weekly")
            {
                COMBon.Enabled = true;
                COMBon.DataSource = null;
                COMBon.Items.Clear();
                COMBon.Items.AddRange(week);
                COMBon.Text = "Sunday";
            }
            if (COMBschedule.Text == "Monthly")
            {
                COMBon.Enabled = true;
                COMBon.Items.Clear();
                COMBon.DataSource = Enumerable.Range(1, 30).ToArray();
            }
        }

        private void BTNupdate_Click(object sender, EventArgs e)
        {
            var myschedule = new MySchedule(originaltask);
            if (myschedule.TaskExists())
            {
                myschedule.DeleteTask();
            }
            DateTime? datetime = timePicker.Value;
            string InibackupXML = Path.GetDirectoryName(Application.ExecutablePath) + "\\Jobs\\" + Path.GetFileName(TXBbackname.Text);
            if (COMBschedule.Text == "Daily")
            {
                BackupUtils.ScheduleDaily(conbackup, datetime.Value.Hour, datetime.Value.Minute, InibackupXML, TXTTaskDefinitionName.Text, InibackupXML);
            }

            if (COMBschedule.Text == "Weekly")
            {
                BackupUtils.ScheduleWeekly(conbackup, datetime.Value.Hour, datetime.Value.Minute, InibackupXML, TXTTaskDefinitionName.Text, COMBon.Text, InibackupXML);
            }

            if (COMBschedule.Text == "Monthly")
            {
                BackupUtils.ScheduleMonthly(conbackup, datetime.Value.Hour, datetime.Value.Minute, InibackupXML, TXTTaskDefinitionName.Text, COMBon.Text, InibackupXML);
            }
        }

        private void RBdiff_CheckedChanged(object sender, EventArgs e)
        {
            if (RBdiff.Checked == true)
            { NUMdiffnumber.Enabled = true; }
            else NUMdiffnumber.Enabled = false;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
             Guid id = Guid.NewGuid();
            if (tabControl1.SelectedIndex == 3 && disable == false)
            {
                TXTTaskDefinitionName.Text = "Schedule-" + TXBbackname.Text +"{" + id + "}";
            }
        }

        private void BTNtaskbefor_Click(object sender, EventArgs e)
        {
            Point screenPoint = BTNtaskbefor.PointToScreen(new Point(BTNtaskbefor.Left, BTNtaskbefor.Bottom));
            if (screenPoint.Y + contextMenuStrip.Size.Height > Screen.PrimaryScreen.WorkingArea.Height)
            {
                contextMenuStrip.Show(BTNtaskbefor, new Point(0, -contextMenuStrip.Size.Height));
            }
            else
            {
                contextMenuStrip.Show(BTNtaskbefor, new Point(0, BTNtaskbefor.Height));
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void TSMIstartprog_Click(object sender, EventArgs e)
        {
            ListViewItem lvi;
            var aHeaders = new string[2];
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                if (File.Exists(openFileDialog.FileName))
                {
                    aHeaders[0] = "StartProgram";
                    aHeaders[1] = openFileDialog.FileName;
                    lvi = new ListViewItem(aHeaders);
                    LSVbefore.Items.Add(lvi);
                }
            }

        }

        private void TSMIpause_Click(object sender, EventArgs e)
        {
            ListViewItem lvi;
            var aHeaders = new string[2];
            string value = "";
            if (MyInputBox.InputBox("Set Time", "Seconds:", ref value) == DialogResult.OK & value != "")
            {
                int num;
                bool success = Int32.TryParse(value, out num);
                if (success == true)
                {
                aHeaders[0] = "Pause";
                aHeaders[1] = value;
                lvi = new ListViewItem(aHeaders);
                LSVbefore.Items.Add(lvi);
                }
                else MessageBox.Show("Has to be a whole number");
            }
        }

 
        private void BTNafterbackup_Click(object sender, EventArgs e)
        {
            Point screenPoint = BTNafterbackup.PointToScreen(new Point(BTNafterbackup.Left, BTNafterbackup.Bottom));
            if (screenPoint.Y + CONTafter.Size.Height > Screen.PrimaryScreen.WorkingArea.Height)
            {
                CONTafter.Show(BTNafterbackup, new Point(0, -CONTafter.Size.Height));
            }
            else
            {
                CONTafter.Show(BTNafterbackup, new Point(0, BTNafterbackup.Height));
            }

        }

        private void startProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem lvi;
            var aHeaders = new string[2];
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                if (File.Exists(openFileDialog.FileName))
                {
                    aHeaders[0] = "StartProgram";
                    aHeaders[1] = openFileDialog.FileName;
                    lvi = new ListViewItem(aHeaders);
                    LSVafter.Items.Add(lvi);
                }
            }

        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem lvi;
            var aHeaders = new string[2];
            string value = "";
            if (MyInputBox.InputBox("Set Time", "Seconds:", ref value) == DialogResult.OK & value != "")
            {
                int num;
                bool success = Int32.TryParse(value, out num);
                if (success == true)
                {
                    aHeaders[0] = "Pause";
                    aHeaders[1] = value;
                    lvi = new ListViewItem(aHeaders);
                    LSVafter.Items.Add(lvi);
                }
                else MessageBox.Show("Has to be a whole number");
            }

        }
    }
}