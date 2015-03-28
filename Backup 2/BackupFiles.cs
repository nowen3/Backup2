using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Backup_2
{
    public delegate void OnListfilesEvent(string fname);

    public delegate void OnLoggerEvent(string msg);

    public delegate void OnMessageEvent(string msg);

    public delegate void UpdateProgressEvent(int prog);

    internal class BackupFiles
    {
        private string xmlbackupfile = string.Empty;
        private bool recursive = true;
        private List<string> excludedirs = new List<string>();
        private List<string> excludeextn = new List<string>();
        private List<string> excludefiles = new List<string>();
        private List<string> existfiles = new List<string>();
        private List<string> includedir = new List<string>();
        private List<string> includefiles = new List<string>();
        private List<Syncinfo> mysyncjobs = new List<Syncinfo>();
        private BindingList<Fileinfo> newfilelist = new BindingList<Fileinfo>();

        public BackupFiles(string BackupXMLfile)
        {
                Diffbackup = false;
                var mysetting = new BackupSettings(BackupXMLfile);
                xmlbackupfile = mysetting.Filename;
                SourceDir = mysetting.SourceDir;
                DestDir = mysetting.DestDir;
                includedir = mysetting.IncludeFiles;
                includedir = mysetting.IncludeDir;
                excludedirs = mysetting.ExcludeDirs;
                excludefiles = mysetting.ExcludeFiles;
                excludeextn = mysetting.ExcludeExtn;
                BackupPath = mysetting.ZIPFilename;
                recursive = mysetting.Recursive;

                int fileExtPos = BackupPath.LastIndexOf(".");
                if (fileExtPos >= 0)
                    BackupPath = BackupPath.Substring(0, fileExtPos);
                if ((!File.Exists(xmlbackupfile)) & Path.GetExtension(xmlbackupfile).ToLower() == ".xml")
                {
                    PrepairBackup();
                    var tempbackup = new XMLconfig(BackupPath + ".xml");
                    tempbackup.Savelist(newfilelist, "Root");
                }
                if ((!File.Exists(xmlbackupfile)) & Path.GetExtension(xmlbackupfile).ToLower() == ".syn")
                {
                    PrepairBackup();
                    return;
                }

                var mybackup = new XMLconfig(xmlbackupfile);
                existfiles = mybackup.Loadlist("Filename");
          
        }

       
        public event OnLoggerEvent mylogger;

        public event OnListfilesEvent OnListFiles;

        public event OnMessageEvent OnMessage;

        public event UpdateProgressEvent UpdateProgress;

        public String BackupPath { get; set; }

        public String SourceDir { get; set; }

        public String DestDir { get; set; }

        public bool Diffbackup { get; set; }

        public List<Syncinfo> Syncjobs
        {
            get { return mysyncjobs; }
            set { mysyncjobs = value; }
        }

        public List<string> ExcludeDirs
        {
            get { return excludedirs; }
            set { excludedirs = value; }
        }

        public List<string> ExcludeExtn
        {
            get { return excludeextn; }
            set { excludeextn = value; }
        }

        public List<string> ExcludeFiles
        {
            get { return excludefiles; }
            set { excludefiles = value; }
        }

        public List<string> IncludeDir
        {
            get { return includedir; }
            set { includedir = value; }
        }

        public List<string> IncludeFiles
        {
            get { return includefiles; }
            set { includefiles = value; }
        }

        public BindingList<Fileinfo> BackupList
        {
            get { return newfilelist; }
        }

        public void RunBackup()
        {
            PrepairBackup();
            var mybackup = new XMLconfig(BackupPath + ".xml");
            mybackup.Savelist(newfilelist, "Root");
        }

        public BindingList<Fileinfo> RunUpdate()
        {
            return UpdateBackup();
        }

        public BindingList<Fileinfo> RunUpdateDiff()
        {
            return UpdateBackup();
        }

        // gets a list of files in the source dir, then adds/removes included/excluded
        private void PrepairBackup()
        {
            var dir_info = new DirectoryInfo(SourceDir);
            try
            {
                if (recursive)
                {
                    Fileutils.SearchDirectory(dir_info, newfilelist);
                }
                else
                    Fileutils.SearchDirectory(dir_info, newfilelist, true);
                if (mylogger != null) { mylogger("Found " + newfilelist.Count.ToString() + " files in backup directory " + SourceDir); }
                int count = newfilelist.Count;

                //remove excluded files and dirs
                Removeexclude(ref newfilelist);
                if (mylogger != null) { mylogger("Removed " + (count - newfilelist.Count + " files from backup " + SourceDir)); }
                count = newfilelist.Count;
                //add included files and dirs
                AddIncluded(ref newfilelist);
                if (mylogger != null) { mylogger("Adding " + (newfilelist.Count - count).ToString() + " files from include list"); }
                //load listview with backup files through delegate
                for (int i = 0; i <= newfilelist.Count - 1; i++)
                {
                    if (OnListFiles != null) { OnListFiles(newfilelist[i].Filename); }
                }
            }
            catch (System.Exception ex1)
            {
                MessageBox.Show("exception in PrepairBackup: " + ex1);
            }
        }

        private void Removeexclude(ref BindingList<Fileinfo> oldlist)
        {
            var newlist = new BindingList<Fileinfo>();
            bool ex = false;
            // remove exclude files and dirs
            for (int i = 0; i <= oldlist.Count - 1; i++)
            {
                try
                {
                    foreach (string s in excludefiles)
                    {
                        if (oldlist[i].Filename == s)
                        {
                            ex = true;
                            break;
                        }
                    }

                    foreach (string sd in excludedirs)
                    {
                        if (sd.Length < oldlist[i].Filename.Length && oldlist[i].Filename.Substring(0, sd.Length) == sd)
                        {
                            ex = true;
                            break;
                        }
                    }

                    foreach (string s in excludeextn)
                    {
                        if (Path.GetExtension(oldlist[i].Filename) == "." + s)
                        {
                            ex = true;
                            if (mylogger != null) { mylogger("Removed File with extn " + s + "--" + oldlist[i].Filename); }
                            break;
                        }
                    }
                }
                catch (System.Exception ex1)
                {
                    MessageBox.Show("exception in Removeexclude: " + ex1);
                }

                if (ex == false)
                    newlist.Add(oldlist[i]);
                ex = false;
            }

            oldlist.Clear();
            oldlist = newlist;
        }

        private void AddIncluded(ref BindingList<Fileinfo> oldlist)
        {
            //  var include = new List<string>();
            //  var includedir = new List<string>();

            try
            {
                foreach (string s in includefiles)
                {
                    if (File.Exists(s))
                    {
                        oldlist.Add(new Fileinfo(s, Fileutils.Getdate(s), Fileutils.Getmoddate(s), "Directory", s));
                    }
                }

                foreach (string sd in includedir)
                {
                    DirectoryInfo dir_info = new DirectoryInfo(sd);
                    Fileutils.SearchDirectory(dir_info, oldlist);
                }
            }
            catch (System.Exception ex1)
            {
                MessageBox.Show("exception in Addincluded: " + ex1);
            }
        }

        private BindingList<Fileinfo> UpdateBackup()
        {
            var mybackup = new XMLconfig(xmlbackupfile);
            var filelist = new BindingList<Fileinfo>();
            var newlist = new BindingList<Fileinfo>();
            var oldlist = new BindingList<Fileinfo>();
            if (mylogger != null) { mylogger("Started to refresh the backup"); }
            if (mylogger != null) { mylogger("Checking for new files"); }
            oldlist = mybackup.Getmodified();
            if (OnMessage != null) { OnMessage("Checking for new files in include dir"); }
            try
            {
                //check for new include files
                foreach (string s in includefiles)
                {
                    if (!existfiles.Contains(s))
                    {
                        newlist.Add(new Fileinfo(s, Fileutils.Getdate(s), Fileutils.Getmoddate(s), "Directory", s));
                        mybackup.Addrecord(s, Fileutils.Getdate(s), Fileutils.Getmoddate(s));
                        if (mylogger != null) { mylogger(s); }
                        if (OnListFiles != null) { OnListFiles(s); }
                    }
                }

                if (mylogger != null) { mylogger("check include folders for new files"); }
                if (mylogger != null) { mylogger("Checking for new files in include dir"); }
                //check each of the include dirs for new and modified files
                foreach (string sd in includedir)
                {
                    var dir_info = new DirectoryInfo(sd);
                    Fileutils.SearchDirectory(dir_info, filelist);
                    for (int i = 0; i <= filelist.Count - 1; i++)
                    {
                        if (!existfiles.Contains(filelist[i].Filename))
                        {
                            newlist.Add((new Fileinfo(filelist[i].Filename, Fileutils.Getdate(filelist[i].Filename), Fileutils.Getmoddate(filelist[i].Filename), "Directory", filelist[i].Filename)));
                            mybackup.Addrecord(filelist[i].Filename, filelist[i].Created, filelist[i].Modified);
                            if (mylogger != null) { mylogger("New file " + filelist[i].Filename); }
                        }

                        int x = existfiles.IndexOf(filelist[i].Filename);
                        if (x >= 0 && x < existfiles.Count && mybackup.Getmoddate(existfiles[x]) != filelist[i].Modified)
                        {
                            newlist.Add(new Fileinfo(filelist[i].Filename, Fileutils.Getdate(filelist[i].Filename), Fileutils.Getmoddate(filelist[i].Filename), "Directory", filelist[i].Filename));
                            mybackup.Modifyrecord(filelist[i].Filename, filelist[i].Modified);
                            if (mylogger != null) { mylogger("Modify File " + filelist[i].Filename); }
                        }

                        if (OnListFiles != null) { OnListFiles(filelist[i].Filename); }
                        if (UpdateProgress != null) { UpdateProgress(i * 100 / filelist.Count); }
                    }
                }

                if (mylogger != null) { mylogger("Checking for new files in backup directory"); }
                if (OnMessage != null) { OnMessage("Checking for new files in backup directory"); }
                var dir_info1 = new DirectoryInfo(SourceDir);
                filelist.Clear();
                Fileutils.SearchDirectory(dir_info1, filelist);
                Removeexclude(ref filelist);
                //check source directory for new and modified files
                for (int i = 0; i <= filelist.Count - 1; i++)
                {
                    //found new file add record to xml file
                    if (!existfiles.Contains(filelist[i].Filename))
                    {
                        newlist.Add(new Fileinfo(filelist[i].Filename, Fileutils.Getdate(filelist[i].Filename), Fileutils.Getmoddate(filelist[i].Filename), "Directory", filelist[i].Filename));
                        mybackup.Addrecord(filelist[i].Filename, filelist[i].Created, filelist[i].Modified);
                        if (mylogger != null) { mylogger("New file " + filelist[i].Filename); }
                    }

                    //found modified file modify xml file
                    int x = existfiles.IndexOf(filelist[i].Filename);
                    if (x >= 0 && x < existfiles.Count && mybackup.Getmoddate(existfiles[x]) != filelist[i].Modified)
                    {
                        newlist.Add(new Fileinfo(filelist[i].Filename, Fileutils.Getdate(filelist[i].Filename), Fileutils.Getmoddate(filelist[i].Filename), "Directory", filelist[i].Filename));
                        mybackup.Modifyrecord(filelist[i].Filename, filelist[i].Modified);
                        if (mylogger != null) { mylogger("Modify File " + filelist[i].Filename); }
                    }

                    if (OnListFiles != null) { OnListFiles(filelist[i].Filename); }
                    if (UpdateProgress != null) { UpdateProgress(i * 100 / filelist.Count); }
                }

                //remove excluded files from list
                Removeexclude(ref newlist);
                if (newlist.Count > 0 & Diffbackup == true)
                {
                    //oldlist contains all modified files before the update
                    //newlist comtains all new updated record
                    //scan oldlist and see if the are any duplicates

                    var newlist1 = oldlist.Concat(newlist).GroupBy(item => item.Filename).Select(group => group.First()).ToList();
                    newlist.Clear();
                    foreach (Fileinfo item in newlist1)
                    {
                        newlist.Add(item);
                    }
                }
            }
            catch (System.Exception ex1)
            {
                MessageBox.Show("exception: " + ex1);
            }

            return newlist;
        }
    }
}