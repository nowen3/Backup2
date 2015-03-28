using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Backup_2
{
    public delegate void OnSyncLoggerEvent(string msg);
    public delegate void OnSyncListfilesEvent(string fname,string destname);

    class SyncFiles
    {
        private List<string> excludedirs = new List<string>();
        private List<string> excludeextn = new List<string>();
        private List<string> excludefiles = new List<string>();
        private List<Syncinfo> mysyncjobs = new List<Syncinfo>();
        private BindingList<Fileinfo> newfilelist = new BindingList<Fileinfo>();
        private string xmlbackupfile = string.Empty;


        public SyncFiles(string BackupXMLfile)
        {
            var mysetting = new SyncSettings(BackupXMLfile);
            xmlbackupfile = mysetting.Filename;
            excludedirs = mysetting.ExcludeDirs;
            excludefiles = mysetting.ExcludeFiles;
            excludeextn = mysetting.ExcludeExtn;
            mysyncjobs = mysetting.Syncjobs;
            Equalisesync = false;
            Timestamp = false;
            
        }

        public event OnSyncLoggerEvent mylogger;
        public event OnSyncListfilesEvent OnListFiles;

        public bool Equalisesync { get; set; }
        public bool Timestamp { get; set; }

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

        public void RunSync()
        {
            foreach (Syncinfo item in mysyncjobs)
            {
              PrepairSync(item.source, item.destination, item.Recursive);
            }
            Syncdir();

          
        }

        public void GetSyncFileList()
        {
            foreach (Syncinfo item in mysyncjobs)
            {
              PrepairSync(item.source, item.destination, item.Recursive);
            }

            //load listview with backup files through delegate
            for (int i = 0; i <= newfilelist.Count - 1; i++)
            {
                if (OnListFiles != null) { OnListFiles(newfilelist[i].Filename, newfilelist[i].DestFilename); }
            }
        }

        public void RunSyncBackwards()
        {
            string sdir = "";
            string temp = "";
            BindingList<Fileinfo> tempfilelist = new BindingList<Fileinfo>();
            newfilelist.Clear();
            foreach (Syncinfo item in mysyncjobs)
            {
                if (sdir != item.destination)
                {
                    int dirindex = item.source.LastIndexOf('\\') + 1;
                    temp = item.source.Substring(dirindex);
                    string SourceDir = item.destination + "\\" + temp;
                    string DestDir = item.source;
                    PrepairSync(SourceDir, DestDir, item.Recursive, true);
                    foreach (Fileinfo myitem in newfilelist)
                    {
                        tempfilelist.Add(myitem);
                    }
                    newfilelist.Clear();
                }
                sdir = item.destination + "\\" + temp;
            }
            newfilelist = tempfilelist;
            Syncdir();
           
        }

        private void PrepairSync(string source, string dest, bool rec, bool backwards = false)
        {
            var dir_info = new DirectoryInfo(source);
            try
            {
                //get list of files in source dir
                if (rec)
                {
                    Fileutils.SearchDirectory(dir_info, newfilelist);
                }
                else
                    Fileutils.SearchDirectory(dir_info, newfilelist, true);

                if (mylogger != null) { mylogger("Found " + newfilelist.Count.ToString() + " files in backup directory " + source); }
                int count = newfilelist.Count;

                //remove excluded files and dirs
                Removeexclude(ref newfilelist);
                if (mylogger != null) { mylogger("Removed " + (count - newfilelist.Count + " files from backup " + source)); }
                count = newfilelist.Count;

                
                if (backwards == false)
                {
                    for (int i = 0; i <= newfilelist.Count - 1; i++)
                    {
                        int dirindex = source.LastIndexOf('\\') + 1;
                        string temp = newfilelist[i].Filename;
                        temp = dest + "\\" + temp.Substring(dirindex);
                        newfilelist[i].DestFilename = temp;
                        
                    }
                }
                else
                {
                    for (int i = 0; i <= newfilelist.Count - 1; i++)
                    {
                        int dirindex = source.Length;
                        string temp = newfilelist[i].Filename;
                        string temp1 = temp.Substring(dirindex);
                        temp = dest + "\\" + temp.Substring(dirindex);
                        newfilelist[i].DestFilename = temp;
                    }
                }
            }
            catch (System.Exception ex1)
            {
                MessageBox.Show("exception in PrepairSync: " + ex1);
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

        private void Syncdir(bool tstamp = false)
        {
            for (int i = 0; i <= newfilelist.Count - 1; i++)
            {

                if (!Directory.Exists(Path.GetDirectoryName(newfilelist[i].DestFilename)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(newfilelist[i].DestFilename));
                }

                if (File.Exists(newfilelist[i].DestFilename) && (Fileutils.Comparemoddate(newfilelist[i].Filename, newfilelist[i].DestFilename) > 0))
                {
                    if (tstamp == true)
                    { Fileutils.CopyFileExactly(newfilelist[i].Filename, newfilelist[i].DestFilename); }
                    else { File.Copy(newfilelist[i].Filename, newfilelist[i].DestFilename, true); }
                    if (mylogger != null) { mylogger("Updated file " + (newfilelist[i].Filename + " To " + newfilelist[i].DestFilename)); }
                    if (OnListFiles != null) { OnListFiles(newfilelist[i].Filename, newfilelist[i].DestFilename); }
                   
                }
                if (File.Exists(newfilelist[i].Filename) && (Fileutils.Comparemoddate(newfilelist[i].Filename, newfilelist[i].DestFilename) < 0) && Equalisesync == true)
                {
                    if (tstamp == true)
                    { Fileutils.CopyFileExactly(newfilelist[i].DestFilename, newfilelist[i].Filename); }
                    else { File.Copy(newfilelist[i].DestFilename, newfilelist[i].Filename, true); }
                    if (mylogger != null) { mylogger("Updated file " + (newfilelist[i].Filename + " To " + newfilelist[i].DestFilename)); }
                    if (OnListFiles != null) { OnListFiles(newfilelist[i].Filename, newfilelist[i].DestFilename); }
                   
                }
                if (!File.Exists(newfilelist[i].DestFilename))
                {
                    if (tstamp == true)
                    { Fileutils.CopyFileExactly(newfilelist[i].Filename, newfilelist[i].DestFilename); }
                    else { File.Copy(newfilelist[i].Filename, newfilelist[i].DestFilename); }
                    if (mylogger != null) { mylogger("Copied file " + (newfilelist[i].Filename + " To " + newfilelist[i].DestFilename)); }
                    if (OnListFiles != null) { OnListFiles(newfilelist[i].Filename, newfilelist[i].DestFilename); }
                   
                }
            }

        }

    }


}
