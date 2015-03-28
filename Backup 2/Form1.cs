using SevenZip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace Backup_2
{
    public partial class Form1 : Form
    {
        private string appPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\Jobs";

        private string conbackup = Path.GetDirectoryName(Application.ExecutablePath) + "\\CONBackup.exe";

        private string filebackupXML;

        private FRMinfo info = new FRMinfo();

        private string InibackupXML;

        private string logfilepath;

        private Boolean m_IsScanning;

        private BindingList<Fileinfo> newsynclist = new BindingList<Fileinfo>();

        public Form1()
        {
            InitializeComponent();
            if (!Directory.Exists(appPath))
            {
                Directory.CreateDirectory(appPath);
            }

            string[] filePaths = Directory.GetFiles(appPath, "*.*");
            foreach (string files in filePaths)// load backup jobs into combobox
            {
                if (files.EndsWith(".xml"))
                {
                    COMBbackupjob.Items.Add(Path.GetFileName(files));
                }

                if (files.EndsWith(".syn"))
                {
                    COMBsyncjob.Items.Add(Path.GetFileName(files));
                }
            }
        }

        private delegate void AddfiletoLBDelegate(string fname);
        private delegate void AddfiletoLSDelegate(string source,string dest);
        private delegate void EndRecursiveScanningDelegate();
        private delegate void MylogDelegate(string msg);
        private delegate void SetlabelFilenameDelegate(string progressValue);
        private delegate void SetlabelTitleDelegate(string progressValue);
        private delegate void UpdateProgresszipDelegate(int progressValue);

        private void BTNnew_Click(object sender, EventArgs e)
        {
            var filelist = new BindingList<Fileinfo>();
            var bworker = new BackgroundWorker();
            LSBbackup.Items.Clear();
            using (var FMnew = new FRMnew())
            {
                FMnew.EnableButton = false;
                DialogResult result = FMnew.ShowDialog();
                if (result == DialogResult.OK)
                {
                    try
                    {
                        filebackupXML = FMnew.BackupPath + ".xml";
                        InibackupXML = appPath + "\\" + FMnew.BackupName + ".xml";
                        var mysetting = new BackupSettings(InibackupXML);
                        var mybackup = new XMLconfig(filebackupXML);
                        logfilepath = appPath + "\\" + FMnew.BackupName + ".log";
                        logfilepath = Path.ChangeExtension(InibackupXML, ".log");
                        LoadLogFile(logfilepath);
                        // FileSize = Convert.ToInt32(FMnew.Logsize);
                        log("Creating a new backup = " + FMnew.BackupName);
                        log("SourceDir = " + FMnew.BackupSource);
                        log("Saving backup in = " + FMnew.BackupDestination);
                        log("compression = " + FMnew.compression);
                        loglist("ExcludeFiles = ", FMnew.ExcludeFiles);
                        loglist("ExcludeDirs = ", FMnew.ExcludeDirs);
                        loglist("IncludeFiles = ", FMnew.IncludeFiles);
                        loglist("IncludeDirs = ", FMnew.IncludeDirs);

                        //write xml file with backup details

                        mysetting.Filename = filebackupXML;
                        mysetting.ZIPFilename = FMnew.BackupPath + ".7z";
                        mysetting.SourceDir = FMnew.BackupSource;
                        mysetting.DestDir = FMnew.BackupDestination;
                        mysetting.Compression = FMnew.compression;
                        mysetting.ExcludeDirs = FMnew.ExcludeDirs;
                        mysetting.IncludeDir = FMnew.IncludeDirs;
                        mysetting.IncludeFiles = FMnew.IncludeFiles;
                        mysetting.ExcludeFiles = FMnew.ExcludeFiles;
                        mysetting.ExcludeExtn = FMnew.ExcludeExtn;
                        mysetting.Recursive = FMnew.Recursive;
                        mysetting.Logsize = FMnew.Logsize;
                        mysetting.DiffBackup = FMnew.diffbackup;
                        mysetting.CompleteBackup = FMnew.completebackup;
                        mysetting.NumberDiffiles = FMnew.NumberDiffFiles;
                        mysetting.CopyBackupFile = FMnew.CopyBackupFile;
                        mysetting.BeforeBackup = FMnew.BeforeBackup;
                        mysetting.AfterBackup = FMnew.AfterBackup;

                        mysetting.SaveXMLfile();
                        //create backup xml file
                        COMBbackupjob.Text = FMnew.BackupName;
                        // var backup = new BackupFiles(FMnew.BackupSource, FMnew.BackupDestination, FMnew.Recursive, FMnew.BackupPath);
                        var backup = new BackupFiles(InibackupXML);
                        backup.mylogger += myloggerevent;
                        backup.OnListFiles += OnListFilesEvent;
                        backup.ExcludeDirs = FMnew.ExcludeDirs;
                        backup.ExcludeFiles = FMnew.ExcludeFiles;
                        backup.IncludeDir = FMnew.IncludeDirs;
                        backup.ExcludeExtn = FMnew.ExcludeExtn;
                        backup.IncludeFiles = FMnew.ExcludeFiles;

                        //set up schedule
                        int myhours = 0;
                        int myminutes = 0;
                        string[] words = FMnew.ScheduleTime.Split(':');
                        int.TryParse(words[0], out myhours);
                        int.TryParse(words[1], out myminutes);
                        switch (FMnew.Schedule)
                        {
                            case "Daily":
                                log("Creating a Daily Schedule");
                                BackupUtils.ScheduleDaily(conbackup, myhours, myminutes, InibackupXML, FMnew.ScheduleTaskName, InibackupXML);
                                break;

                            case "Weekly":
                                log("Creating a weekly Schedule");
                                BackupUtils.ScheduleWeekly(conbackup, myhours, myminutes, InibackupXML, FMnew.ScheduleTaskName, FMnew.ScheduleOn, InibackupXML);
                                break;

                            case "Monthly":
                                log("Creating a Monthly Schedule");
                                BackupUtils.ScheduleMonthly(conbackup, myhours, myminutes, InibackupXML, FMnew.ScheduleTaskName, FMnew.ScheduleOn, InibackupXML);
                                break;
                        }
                        if (mysetting.BeforeBackup.Count > 0)
                        {
                            var mytasks = new Tasks(mysetting.BeforeBackup);
                            mytasks.mylogger += myloggerevent;
                            mytasks.RunTasks();
                        }
                        // run back class to get list of files yo backup
                        backup.RunBackup();
                        filelist = backup.BackupList;
                        //create thread to run the zip function
                        bworker.DoWork += new DoWorkEventHandler(CreateZipFile);
                        bworker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompressionFinished);
                        bworker.WorkerReportsProgress = true;
                        bworker.WorkerSupportsCancellation = true;
                        bworker.RunWorkerAsync(filelist);
                        Logsize(Convert.ToInt32(mysetting.Logsize));
                        if (info.ShowDialog(this) == DialogResult.OK)
                        {
                            if (m_IsScanning)
                            {
                                //  m_CancelScanning = true;
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
        }

        private void CreateZipFile(object sender, DoWorkEventArgs e)
        {
            var filelist = new BindingList<Fileinfo>();
            filelist = (BindingList<Fileinfo>)e.Argument;
            var mysetting = new BackupSettings(InibackupXML);

            createbackup(filelist, mysetting.ZIPFilename, mysetting.Compression, mysetting.CopyBackupFile);
            if (mysetting.AfterBackup.Count > 0)
            {
                var mytasks = new Tasks(mysetting.AfterBackup);
                mytasks.mylogger += myloggerevent;
                mytasks.RunTasks();
            }
        }

        //load backup when user selects backup from combobox
        private void COMBbackupjobbs_SelectedIndexChanged(object sender, EventArgs e)
        {
            Openbackup(appPath + "\\" + COMBbackupjob.Text);
        }

        private void openBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK & Path.GetExtension(openFileDialog.FileName) == ".xml")
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    COMBbackupjob.Text = Path.GetFileName(openFileDialog.FileName);
                }
            }
        }


        private void Openbackup(string fname)
        {
            InibackupXML = fname;
            try
            {
                logfilepath = Path.ChangeExtension(InibackupXML, ".log");
                LoadLogFile(Path.ChangeExtension(InibackupXML, ".log"));
                log("Opening File " + InibackupXML);
                var mysetting = new BackupSettings(InibackupXML);
                // FileSize = Convert.ToInt32(mysetting.Logsize);
                if (!File.Exists(mysetting.Filename))
                {
                    DialogResult dialogResult = MessageBox.Show("Unable to find backup file, do you want to recreate it", "Unable to find backup file", MessageBoxButtons.YesNo);
                    {
                        if (dialogResult == DialogResult.Yes)
                        {
                            var backup = new BackupFiles(InibackupXML);
                            log("Unable to find backup file " + mysetting.Filename + " Creating a new one");
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            COMBbackupjob.Text = "";
                            return;
                        }
                    }
                }

                filebackupXML = mysetting.Filename;
                LSBbackup.Items.Clear();
                var mybackup = new XMLconfig(mysetting.Filename);
                Cursor.Current = Cursors.WaitCursor;
                foreach (string mynodes in mybackup.Loadlist("Filename").ToArray())
                {
                    LSBbackup.Items.Add(mynodes);
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
        private void BTNupdate_Click(object sender, EventArgs e)
        {
            try
            {
                LSBbackup.Items.Clear();
                var bworker = new BackgroundWorker();
                bworker.DoWork += new DoWorkEventHandler(refreshbackup);
                // bworker.ProgressChanged += new ProgressChangedEventHandler(ProgressChanged);
                bworker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompressionFinished);
                bworker.WorkerReportsProgress = true;
                bworker.WorkerSupportsCancellation = true;
                bworker.RunWorkerAsync();
                if (info.ShowDialog(this) == DialogResult.OK)
                {
                    if (m_IsScanning)
                    {
                        //m_CancelScanning = true;
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //look for new files and add to backup
        private void refreshbackup(object sender, DoWorkEventArgs e)
        {
            try
            {
                string oldzipname = "";
                var mysetting = new BackupSettings(InibackupXML);
                var ufilelist = new BindingList<Fileinfo>();
                var filelist = new BindingList<Fileinfo>(); //files in real directorys
                if (mysetting.BeforeBackup.Count > 0)
                {
                    var mytasks = new Tasks(mysetting.BeforeBackup);
                    mytasks.mylogger += myloggerevent;
                    mytasks.RunTasks();
                }

                SetlabelTitle("Checking for new files"); // check for new files
                var backup = new BackupFiles(InibackupXML);
                backup.mylogger += myloggerevent;
                backup.OnListFiles += SetlabelFilename;
                backup.OnMessage += SetlabelTitle;
                backup.UpdateProgress += UpdateProgresszip;
                backup.Diffbackup = mysetting.DiffBackup;

                if (backup.Diffbackup == true)
                {
                    ufilelist = backup.RunUpdateDiff();
                    oldzipname = Path.GetFileNameWithoutExtension(mysetting.Filename);
                    string newzip = Path.GetDirectoryName(mysetting.ZIPFilename) + "\\" + oldzipname + "-" + Fileutils.Getdiffilename();
                    mysetting.ZIPFilename = newzip;

                }
                else
                {
                    if (File.Exists(mysetting.ZIPFilename))
                    {
                        ufilelist = backup.RunUpdate();

                    }
                    else
                    {
                        File.Delete(mysetting.Filename);
                        backup.RunBackup();
                        ufilelist = backup.BackupList;
                    }
                }

                for (int i = 0; i <= ufilelist.Count - 1; i++)
                {
                    AddfiletoLB(ufilelist[i].Filename);
                }


                SetlabelTitle("Backingup");
                myloggerevent("Backingup");
                createbackup(ufilelist, mysetting.ZIPFilename, mysetting.Compression, mysetting.CopyBackupFile);
                SetlabelTitle("Finished Update");
                // myloggerevent("Finished Update");
                if (backup.Diffbackup == true)
                {
                    Fileutils.DeleteDiffBackup(mysetting.DestDir, oldzipname, mysetting.NumberDiffiles);
                }
                Logsize(Convert.ToInt32(mysetting.Logsize));
                if (mysetting.AfterBackup.Count > 0)
                {
                    var mytasks = new Tasks(mysetting.AfterBackup);
                    mytasks.mylogger += myloggerevent;
                    mytasks.RunTasks();
                }
                Endrefresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backupPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> exclude = new List<string>();
            string fname = null;
            if (tabControl1.SelectedIndex == 0)
            {
                fname = appPath + "\\" + COMBbackupjob.Text;
                if (File.Exists(fname))
                {
                    var mysetting = new BackupSettings(InibackupXML);
                    using (FRMnew FMrule = new FRMnew())
                    {

                        FMrule.EnableButton = true;
                        FMrule.BackupName = mysetting.Filename;
                        FMrule.BackupSource = mysetting.SourceDir;
                        FMrule.BackupDestination = mysetting.DestDir;
                        FMrule.compression = mysetting.Compression;
                        FMrule.Schedule = mysetting.Schedule;
                        FMrule.ScheduleOn = mysetting.ScheduleOn;
                        FMrule.ScheduleTime = mysetting.ScheduleTime;
                        FMrule.ScheduleTaskName = mysetting.TaskDefination;
                        FMrule.DisableControls = true;
                        FMrule.diffbackup = mysetting.DiffBackup;
                        FMrule.completebackup = mysetting.CompleteBackup;
                        FMrule.Logsize = mysetting.Logsize;
                        FMrule.NumberDiffFiles = mysetting.NumberDiffiles;
                        FMrule.CopyBackupFile = mysetting.CopyBackupFile;
                        FMrule.BeforeBackup = mysetting.BeforeBackup;
                        FMrule.AfterBackup = mysetting.AfterBackup;
                        exclude = mysetting.ExcludeFiles;
                        foreach (string s in exclude)
                        {
                            FMrule.Addexclude = s;
                        }
                        exclude.Clear();
                        exclude = mysetting.ExcludeDirs;
                        foreach (string s in exclude)
                        {
                            FMrule.Addexclude = s;
                        }
                        exclude.Clear();
                        exclude = mysetting.ExcludeExtn;
                        foreach (string s in exclude)
                        {
                            FMrule.AddexcludeExtn = s;
                        }


                        exclude.Clear();
                        exclude = mysetting.IncludeFiles;
                        foreach (string s in exclude)
                        {
                            FMrule.Addinclude = s;
                        }
                        exclude.Clear();
                        exclude = mysetting.IncludeDir;
                        foreach (string s in exclude)
                        {
                            FMrule.Addinclude = s;
                        }
                        DialogResult result = FMrule.ShowDialog();
                        if (result == DialogResult.OK) // <-- Checks DialogResult
                        {
                            try
                            {
                                File.Delete(InibackupXML);
                                //write xml file with backup details
                                mysetting.Filename = FMrule.BackupPath;
                                mysetting.ZIPFilename = Path.GetDirectoryName(FMrule.BackupPath) + "\\" + Path.GetFileNameWithoutExtension(FMrule.BackupPath) + ".7z";
                                mysetting.SourceDir = FMrule.BackupSource;
                                mysetting.DestDir = FMrule.BackupDestination;
                                mysetting.Compression = FMrule.compression;
                                mysetting.Schedule = FMrule.Schedule;
                                mysetting.ScheduleOn = FMrule.ScheduleOn;
                                mysetting.ExcludeFiles = FMrule.ExcludeFiles;
                                mysetting.ExcludeDirs = FMrule.ExcludeDirs;
                                mysetting.ExcludeExtn = FMrule.ExcludeExtn;
                                mysetting.IncludeFiles = FMrule.IncludeFiles;
                                mysetting.IncludeDir = FMrule.IncludeDirs;
                                mysetting.Logsize = FMrule.Logsize;
                                mysetting.DiffBackup = FMrule.diffbackup;
                                mysetting.CompleteBackup = FMrule.completebackup;
                                mysetting.NumberDiffiles = FMrule.NumberDiffFiles;
                                mysetting.CopyBackupFile = FMrule.CopyBackupFile;
                                mysetting.BeforeBackup = FMrule.BeforeBackup;
                                mysetting.AfterBackup = FMrule.AfterBackup;
                                mysetting.SaveXMLfile();

                                COMBbackupjob.Text = FMrule.BackupName;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }

                    }
                }
                else
                    MessageBox.Show("Open or Create a New backup First");
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                OpenSynOption();  
            }
        }

        

        private void BTNbackupremove_Click(object sender, EventArgs e)
        {
            try
            {
                string appPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\Jobs";
                if (File.Exists(InibackupXML))
                {
                    File.Delete(InibackupXML);
                    COMBbackupjob.Items.Remove(COMBbackupjob.Text);
                    COMBbackupjob.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void createbackup(BindingList<Fileinfo> filelist, string zipname, string compresion, string copydir)
        {
            if (filelist.Count == 0) { return; }
            // var mysetting = new BackupSettings(InibackupXML);
            Dictionary<string, string> filesDictionary = new Dictionary<string, string>();
            try
            {
                SevenZipExtractor.SetLibraryPath(Path.GetDirectoryName(Application.ExecutablePath) + "\\7z64.dll");
                var Compressor = new SevenZipCompressor();
                if (!File.Exists(zipname))
                {
                    Compressor.CompressionMode = CompressionMode.Create;
                    myloggerevent("Zip file does not exist, Creating one " + zipname);
                }
                else Compressor.CompressionMode = CompressionMode.Append;
                switch (compresion)
                {
                    case "Fast":
                        Compressor.CompressionLevel = CompressionLevel.Fast;
                        break;

                    case "High":
                        Compressor.CompressionLevel = CompressionLevel.High;
                        break;

                    case "Low":
                        Compressor.CompressionLevel = CompressionLevel.Low;
                        break;

                    case "None":
                        Compressor.CompressionLevel = CompressionLevel.None;
                        break;

                    case "Normal":
                        Compressor.CompressionLevel = CompressionLevel.Normal;
                        break;

                    case "Ultra":
                        Compressor.CompressionLevel = CompressionLevel.Ultra;
                        break;

                    default:
                        Compressor.CompressionLevel = CompressionLevel.Normal;
                        break;
                }
                Compressor.Compressing += new EventHandler<ProgressEventArgs>(SaveProgress);
                // Compressor.CompressionFinished += new EventHandler<EventArgs>(CompressionFinished);
                Compressor.FileCompressionStarted += new EventHandler<FileNameEventArgs>(FileCompressionStarted);
                Compressor.ArchiveFormat = OutArchiveFormat.SevenZip;
                Compressor.CompressionMethod = CompressionMethod.Lzma;
                Compressor.DirectoryStructure = true;
                Compressor.PreserveDirectoryRoot = true;
                Compressor.TempFolderPath = System.IO.Path.GetTempPath();

                for (int i = 0; i <= filelist.Count - 1; i++)
                {
                    if (File.Exists(filelist[i].Filename))
                    {
                        string MyPath = filelist[i].Filename.Substring(Path.GetPathRoot(filelist[i].Filename).Length);
                        if (filesDictionary.ContainsKey(MyPath))
                        {
                            myloggerevent("dictioary key exists-" + MyPath);
                        }
                        else
                            filesDictionary.Add(MyPath, filelist[i].Filename);
                    }
                }
                Compressor.CompressFileDictionary(filesDictionary, zipname);
                myloggerevent("Compression Finished");
                if (Directory.Exists(copydir))
                {
                    File.Copy(zipname, copydir + Path.GetFileName(zipname));

                }

                Endrefresh();
            } //end try
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // ends the tread
        private void Endrefresh()
        {
            if (!this.InvokeRequired)
            {
                m_IsScanning = false;
                info.Hide();
            }
            else
            {
                this.Invoke(new EndRecursiveScanningDelegate(this.Endrefresh));
            }
        }

        public void CompressionFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // lblStatus.Text = "Task Cancelled.";
            }
            else if (e.Error != null)
            {
                //lblStatus.Text = "Error while performing background operation.";
            }
            else
            {
                // Everything completed normally.
                MessageBox.Show("Finished Backing up Data");
            }
        }

        private void listModifiedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filelist = new BindingList<Fileinfo>();

            LSBbackup.Items.Clear();
            var mybackup = new XMLconfig(filebackupXML);
            filelist = mybackup.Getmodified();
            for (int i = 0; i <= filelist.Count - 1; i++)
            {
                LSBbackup.Items.Add(filelist[i].Filename);
            }



        }

        private void BTNrestore_Click(object sender, EventArgs e)
        {
            string Edir;
            if (LSBbackup.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select some files");
                return;
            }
            using (FRMrestore FMrestore = new FRMrestore())
            {
                DialogResult result = FMrestore.ShowDialog();

                if (result == DialogResult.OK) // <-- Checks DialogResult
                {
                    try
                    {
                        List<Textract> extractfiles = new List<Textract>();
                        Edir = null;
                        bool overwrite = FMrestore.RestoreOriginal;
                        if (FMrestore.Restorenew == true)
                            Edir = FMrestore.NewLocation;

                        for (int i = 0; i <= LSBbackup.SelectedItems.Count - 1; i++)
                        {
                            Textract exdir = new Textract();
                            if (Edir == null)
                            {
                                exdir.dir = LSBbackup.SelectedItems[i].ToString().Substring(0, 3);
                            }
                            else
                            {
                                exdir.dir = Edir;
                            }

                            exdir.fname = LSBbackup.SelectedItems[i].ToString().Remove(0, 3);
                            extractfiles.Add(exdir);
                        }
                        Extractfiles(extractfiles, Edir, overwrite);
                        MessageBox.Show("Finished Extrating Files");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
        }

        // extract files from zip file
        private void Extractfiles(List<Textract> Efiles, string Edir, bool overwrite)
        {
            //XMLconfig backup = new XMLconfig(InibackupXML);
            //string ZipFileToCreate = backup.Getjobrecord("ZIPFilename");

            //using (ZipFile zip = ZipFile.Read(ZipFileToCreate))
            //{
            //    foreach (Textract item in Efiles)
            //    {
            //        if (overwrite == true)
            //        {
            //            ZipEntry e = zip[item.fname];
            //            e.Extract(item.dir, ExtractExistingFileAction.OverwriteSilently);
            //        }
            //        else
            //        {
            //            ZipEntry e = zip[item.fname];
            //            e.Extract(item.dir, ExtractExistingFileAction.DoNotOverwrite);

            //        }

            //    }
            //}
        }

       
//---------------------------------- Events-------------------------------------------------------------------//

        public void SaveProgress(object sender, ProgressEventArgs e)
        {
            UpdateProgresszip(e.PercentDone);
        }

        private void AddfiletoLB(string fname)
        {
            if (this.LSBbackup.InvokeRequired)
            {
                AddfiletoLBDelegate d = new AddfiletoLBDelegate(AddfiletoLB);
                this.Invoke(d, new object[] { fname });
            }
            else
                this.LSBbackup.Items.Add(fname);
        }

        private void AddFilestoLS(string source,string dest)
        {
            if(this.LSsync.InvokeRequired)
            {
                AddfiletoLSDelegate d = new AddfiletoLSDelegate(AddFilestoLS);
                this.Invoke(d, new object[] { source, dest });
            }
            else
            {
                var aHeaders = new string[2];
                aHeaders[0] = source;
                aHeaders[1] = dest;
                ListViewItem lvi = new ListViewItem(aHeaders);
                this.LSsync.Items.Add(lvi);

            }
                

        }

        private void FileCompressionStarted(object sender, FileNameEventArgs e)
        {
            SetlabelTitle("Saving Filename:");
            SetlabelFilename(e.FileName);
        }

        private List<string> Getlist()
        {
            List<string> listboxlist = new List<string>();
            listboxlist = LSBbackup.Items.Cast<String>().ToList();
            return listboxlist;
        }

        private void myloggerevent(string msg)
        {

            if (this.TXTlogscreen.InvokeRequired)
            {
                MylogDelegate d = new MylogDelegate(myloggerevent);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                this.log(msg);
            }
        }
        
        private void OnListFilesEvent(string fname)
        {
            LSBbackup.Items.Add(fname);
        }

        private void OnSyncListFilesEvent(string fname, string destname)
        {
            var aHeaders = new string[2];
            aHeaders[0] = fname;
            aHeaders[1] = destname;
            ListViewItem lvi = new ListViewItem(aHeaders);
            int comp = Fileutils.Comparemoddate(fname, destname);
            if (comp > 0)
            {
                lvi.ForeColor = Color.Red;
            }
            else if (comp == 0)
            {
                lvi.ForeColor = Color.Black;
            }
            else if (comp < 0)
            {
                lvi.ForeColor = Color.Green;
            }


            LSsync.Items.Add(lvi);
        }

        // adds file to main listbox
        private void SetlabelTitle(string progressValue)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.info.InvokeRequired)
            {
                SetlabelTitleDelegate d = new SetlabelTitleDelegate(SetlabelTitle);
                this.Invoke(d, new object[] { progressValue });
            }
            else
            {
                this.info.setlab(progressValue);
            }
        }

        private void SetlabelFilename(string progressValue)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.info.InvokeRequired)
            {
                SetlabelFilenameDelegate d = new SetlabelFilenameDelegate(SetlabelFilename);
                this.Invoke(d, new object[] { progressValue });
            }
            else
            {
                this.info.setlab1(progressValue);
            }
        }

        // updates the progress bar

        private void UpdateProgresszip(int progressValue)
        {
            if (this.info.InvokeRequired)
            {
                UpdateProgresszipDelegate d = new UpdateProgresszipDelegate(UpdateProgresszip);
                this.Invoke(d, new object[] { progressValue });
            }
            else
            {
                this.info.setprog(progressValue);
            }
        }


//--------------------------------------Sync--------------------------------------------------------------------//
        private void OpenSynOption()
        {
            List<string> exclude = new List<string>();
            string  fname = appPath + "\\" + COMBsyncjob.Text;
            var mysetting = new SyncSettings(fname);
          using (FRMsync FMnew = new FRMsync())
          {
              FMnew.EnableButton = true;
              FMnew.BackupName = mysetting.Filename;
              FMnew.Syncjobs = mysetting.Syncjobs;
              FMnew.Schedule = mysetting.Schedule;
              FMnew.ScheduleOn = mysetting.ScheduleOn;
              FMnew.ScheduleTime = mysetting.ScheduleTime;
              FMnew.ScheduleTaskName = mysetting.TaskDefination;
              FMnew.BeforeBackup = mysetting.BeforeBackup;
              FMnew.AfterBackup = mysetting.AfterBackup;
              FMnew.Logsize = mysetting.Logsize;
              exclude = mysetting.ExcludeFiles;
              foreach (string s in exclude)
              {
                  FMnew.Addexclude = s;
              }
              exclude.Clear();
              exclude = mysetting.ExcludeDirs;
              foreach (string s in exclude)
              {
                  FMnew.Addexclude = s;
              }
              exclude.Clear();
              exclude = mysetting.ExcludeExtn;
              foreach (string s in exclude)
              {
                  FMnew.AddexcludeExtn = s;
              }
              
              DialogResult result = FMnew.ShowDialog();
              if (result == DialogResult.OK) // <-- Checks DialogResult
              {
                  try
                  {
                    File.Delete(InibackupXML);
                     //write xml file with backup details
                    
                      mysetting.Filename = InibackupXML;
                      mysetting.Syncjobs = FMnew.Syncjobs;
                      mysetting.ExcludeFiles = FMnew.ExcludeFiles;
                      mysetting.ExcludeDirs = FMnew.ExcludeDirs;
                      mysetting.ExcludeExtn = FMnew.ExcludeExtn;
                      mysetting.BeforeBackup = FMnew.BeforeBackup;
                      mysetting.AfterBackup = FMnew.AfterBackup;
                      mysetting.Logsize = FMnew.Logsize;
                      mysetting.Schedule = FMnew.Schedule;
                      mysetting.ScheduleOn = FMnew.ScheduleOn;
                      mysetting.SaveXMLfile();
                      
                  }
                  catch (Exception ex)
                  {
                      MessageBox.Show(ex.Message);
                  }
              }

          }

        }
       

        private void BTNsyncremove_Click(object sender, EventArgs e)
        {
            string appPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\Jobs";
            if (File.Exists(InibackupXML))
            {
                File.Delete(InibackupXML);
                COMBsyncjob.Items.Remove(COMBsyncjob.Text);
                COMBsyncjob.Text = "";
                LSsync.Items.Clear();
            }
        }

        private void BTNsynnew_Click(object sender, EventArgs e)
        {
            LSsync.Items.Clear();
            using (FRMsync FMnew = new FRMsync())
            {
                FMnew.EnableButton = false;
                DialogResult result = FMnew.ShowDialog();
                if (result == DialogResult.OK) // <-- Checks DialogResult
                {
                    try
                    {
                        InibackupXML = appPath + "\\" + FMnew.BackupName + ".syn";
                        var mysetting = new SyncSettings(InibackupXML);
                        logfilepath = appPath + "\\" + "syn" + FMnew.BackupName + ".log";
                        LoadLogFile(logfilepath);
                        mysetting.Filename = InibackupXML;
                        mysetting.Syncjobs = FMnew.Syncjobs;
                        mysetting.ExcludeFiles = FMnew.ExcludeFiles;
                        mysetting.ExcludeDirs = FMnew.ExcludeDirs;
                        mysetting.Recursive = FMnew.Recursive;
                        mysetting.BeforeBackup = FMnew.BeforeBackup;
                        mysetting.AfterBackup = FMnew.AfterBackup;
                        mysetting.Syncjobs = FMnew.Syncjobs;
                        mysetting.Logsize = FMnew.Logsize;
                        mysetting.SaveXMLfile();
                        log("Creating a new Sync = " + FMnew.BackupName);
                        loglist("ExcludeFiles = ", FMnew.ExcludeFiles);
                        loglist("ExcludeDirs = ", FMnew.ExcludeDirs);
                        
                        int myhours = 0;
                        int myminutes = 0;
                        string[] words = FMnew.ScheduleTime.Split(':');
                        int.TryParse(words[0], out myhours);
                        int.TryParse(words[1], out myminutes);
                        switch (FMnew.Schedule)
                        {
                            case "Daily":
                                log("Creating a Daily Schedule");
                                BackupUtils.ScheduleDaily(conbackup, myhours, myminutes, InibackupXML, FMnew.ScheduleTaskName, InibackupXML);
                                break;

                            case "Weekly":
                                log("Creating a weekly Schedule");
                                BackupUtils.ScheduleWeekly(conbackup, myhours, myminutes, InibackupXML, FMnew.ScheduleTaskName, FMnew.ScheduleOn, InibackupXML);
                                break;

                            case "Monthly":
                                log("Creating a Monthly Schedule");
                                BackupUtils.ScheduleMonthly(conbackup, myhours, myminutes, InibackupXML, FMnew.ScheduleTaskName, FMnew.ScheduleOn, InibackupXML);
                                break;
                        }

                        var mybackup = new SyncFiles(InibackupXML);
                        mybackup.mylogger += myloggerevent;
                        mybackup.OnListFiles += OnSyncListFilesEvent;
                        COMBsyncjob.Text = FMnew.BackupName;
                        newsynclist.Clear();
                                          
                        runsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
        }

                

        private void BTNupdatesync_Click(object sender, EventArgs e)
        {
            if (File.Exists(appPath + "\\" + COMBsyncjob.Text))
            {
                LSsync.Items.Clear();
                runsync();
            }
            else MessageBox.Show("No sync to update");
        }

        private void runsync()
        {
            var args = new MySyncArgs(RADsynccopy.Checked, RADsyncmirror.Checked, RADsynceq.Checked,CHBtimestamp.Checked);
           // LSsync.Clear();
            var bworker = new BackgroundWorker();
            
            
            bworker.DoWork += new DoWorkEventHandler(createsync);
            bworker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompressionFinished);
            bworker.WorkerReportsProgress = true;
            bworker.WorkerSupportsCancellation = true;
            bworker.RunWorkerAsync(args);
           
        }

        private void COMBsyncjob_SelectedIndexChanged(object sender, EventArgs e)
        {
            newsynclist.Clear();
            LSsync.Items.Clear();

            InibackupXML = (appPath + "\\" + COMBsyncjob.Text);
            string logfname = Path.GetDirectoryName(InibackupXML) + "\\syn" + Path.GetFileName(InibackupXML);
            logfilepath = Path.ChangeExtension(logfname, ".log");
            LoadLogFile(logfilepath);
            var inibackup = new SyncSettings(InibackupXML);
            log("Opened " + InibackupXML);
            var mysync = new SyncFiles(InibackupXML);
            mysync.mylogger += myloggerevent;
            mysync.OnListFiles += OnSyncListFilesEvent;

            Cursor.Current = Cursors.WaitCursor;
            mysync.GetSyncFileList();
            Cursor.Current = Cursors.Default;
        }



        private void createsync(object sender, DoWorkEventArgs e)
        {
        	var arg = e.Argument as MySyncArgs;
            var mysync = new SyncFiles(InibackupXML);
            mysync.mylogger += myloggerevent;
            mysync.OnListFiles += AddFilestoLS;
            mysync.Timestamp = arg.timestamp;
            mysync.Equalisesync = arg.eq;
            
        	if (arg.copy == true)
        	{
               
                mysync.RunSync();
                
        	}
        	if (arg.eq == true)
        	{
                
                mysync.RunSync();
                mysync.RunSyncBackwards();
        		
        	}
        	
        	Endrefresh();
        }

        

//--------------------------------------------------Log---------------------------------------------------------//

        private void clearLogTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TXTlogscreen.Clear();
        }

        private void LoadLogFile(string fname)
        {
        	 if (!File.Exists(fname))
            {
                using (StreamWriter sw = File.CreateText(fname))
                {
                    sw.WriteLine("LOG FILE");
                    sw.Close();
                }	

            }
            TXTlogscreen.Clear();
            TXTlogscreen.Text = File.ReadAllText(fname);
        }

       
        private void log(string msg)
        {
          
        	if (msg.Length > 0)
            {
                try
                {
                    using (StreamWriter sw = File.AppendText(logfilepath))
                    {
                        TXTlogscreen.AppendText("[ " + DateTime.Now.ToShortDateString() + ": " + DateTime.Now.ToShortTimeString() + ": ]: " + msg);
                        sw.WriteLine("{0} {1}: {2}: {3}: {4}", "[", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "]", msg);
                        TXTlogscreen.AppendText("\r\n");
                        sw.Flush();
                    }
                }
                catch (IOException ex2)
                {
                    LoadLogFile(logfilepath);
                	MessageBox.Show("exception in file: " + ex2);
                }
            }
        }

        private void loglist(string msg, List<string> msglist)
        {
            if (msglist.Count > 0)
            {
            	try
                {
                using (StreamWriter sw = File.AppendText(logfilepath))
                {
                    foreach (string mymsg in msglist)
                    {
                        TXTlogscreen.AppendText("[ " + DateTime.Now.ToShortDateString() + ": " + DateTime.Now.ToShortTimeString() + ": ]: " + msg + mymsg);
                        sw.WriteLine("{0} {1}: {2}: {3}: {4}", "[", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), "]", msg + mymsg);
                        TXTlogscreen.AppendText("\r\n");
                    }
                    sw.Flush();
                }
                }
                catch (IOException ex2)
                {
                    LoadLogFile(logfilepath);
                	MessageBox.Show("exception in file: " + ex2);
                }
            }
        }

        private void Logsize(int myfilesize)
        {
            if (myfilesize != 0)
            {
                FileInfo txtfile = new FileInfo(logfilepath);
                if (txtfile.Length > (myfilesize * 1048576))
                {
                    TextReader TR = new StreamReader(logfilepath);
                    string sTemp = TR.ReadToEnd();
                    TR.Close();
                    int ftot = sTemp.Length - (myfilesize * 1048576);
                    if (ftot < 0) { ftot = 0; }
                    string sTempRemoved = sTemp.Remove(0, ftot);
                    StreamWriter TWR = new System.IO.StreamWriter(logfilepath, false);
                    TWR.Write(sTempRemoved);
                    TWR.Close();
                }
            }
        }

 
       
        
        void ClearLogFileToolStripMenuItemClick(object sender, EventArgs e)
        {
        	File.WriteAllText(logfilepath,string.Empty);
        	LoadLogFile(logfilepath);
        	
        }
    } // end of class
}// end of namespace