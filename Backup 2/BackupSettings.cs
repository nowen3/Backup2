using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Backup_2
{
    internal class BackupSettings
    {
        private string myPath = string.Empty;
 
        private List<string> includedir = new List<string>();
        private List<string> excludedirs = new List<string>();
        private List<string> excludefiles = new List<string>();
        private List<string> includefiles = new List<string>();
        private List<string> excludeextn = new List<string>();
        private Dictionary<string, string> beforedict = new Dictionary<string, string>();
        private Dictionary<string, string> afterdict = new Dictionary<string, string>();

        public BackupSettings(string fname)
        {
            this.myPath = fname;
            if (!File.Exists(myPath))
            {
                this.createxmltile("Root");
            }
            NumberDiffiles = 0;
            Recursive = true;
            DiffBackup = false;
            CompleteBackup = true;
            Logsize = 0;
            LoadXMLfile();
        }

       
        public String Filename { get; set; }
        public String SourceDir { get; set; }
        public String DestDir{ get; set; }
        public String ZIPFilename { get; set; }
        public String Compression { get; set; }
        public String TaskDefination { get; set; }
        public String ScheduleTime { get; set; }
        public String Schedule { get; set; }
        public String ScheduleOn { get; set; }
        public String CopyBackupFile { get; set; }
        public int NumberDiffiles { get; set; }
        public bool Recursive { get; set; }
        public bool DiffBackup { get; set; }
        public bool CompleteBackup { get; set; }
        public decimal Logsize { get; set; }

       
        
        public List<string> IncludeDir
        {
            get { return includedir; }
            set { includedir = value; }
        }
        public List<string> ExcludeDirs
        {
            get { return excludedirs; }
            set { excludedirs = value; }
        }
        public List<string> IncludeFiles
        {
            get { return includefiles; }
            set { includefiles = value; }
        }
        public List<string> ExcludeFiles
        {
            get { return excludefiles; }
            set { excludefiles = value; }
        }
        public List<string> ExcludeExtn
        {
            get { return excludeextn; }
            set { excludeextn = value; }
        }

        public Dictionary<string, string> BeforeBackup
        {
            get { return beforedict; }
            set { beforedict = value; }

        }

        public Dictionary<string, string> AfterBackup
        {
            get { return afterdict; }
            set { afterdict = value; }

        }
 
        private void LoadXMLfile()
        {
            XDocument doc = XDocument.Load(myPath);
            IEnumerable<XElement> childList = from el in doc.Root.Elements() select el;
            foreach (XElement e in childList)
            {
                switch (e.Name.ToString())
                {
                    case "Filename":
                        Filename = e.Value;
                        break;

                    case "SourceDir":
                        SourceDir = e.Value;
                        break;

                    case "DestDir":
                        DestDir = e.Value;
                        break;

                    case "ZIPFilename":
                        ZIPFilename = e.Value;
                        break;

                    case "compression":
                        Compression = e.Value;
                        break;

                    case "ScheduleTime":
                        ScheduleTime = e.Value;
                        break;

                    case "TaskDefination":
                        TaskDefination = e.Value;
                        break;

                    case "IncludeDirs":
                        includedir.Add(e.Value);
                        break;

                    case "ExcludeDirs":
                        excludedirs.Add(e.Value);
                        break;

                    case "IncludeFiles":
                        includefiles.Add(e.Value);
                        break;

                    case "ExcludeFiles":
                        excludefiles.Add(e.Value);
                        break;

                    case "ExcludeExtn":
                        excludeextn.Add(e.Value);
                        break;

                    case "Schedule":
                        Schedule = e.Value;
                        break;

                    case "ScheduleOn":
                        ScheduleOn = e.Value;
                        break;

                    case "Recursive":
                        Recursive = Boolean.Parse(e.Value);
                        break;

                    case "Completebackup":
                        CompleteBackup = Boolean.Parse(e.Value);
                        break;

                    case "Diffbackup":
                        DiffBackup = Boolean.Parse(e.Value);
                        break;

                    case "Logsize":
                        Logsize = Decimal.Parse(e.Value);
                        break;
                    case "NoDifFiles":
                        NumberDiffiles = int.Parse(e.Value);
                        break;

                    case "CopyBackupFile":
                        CopyBackupFile = e.Value;
                        break;
                    case "Before":
                        foreach (XElement el in e.Nodes())
                            beforedict.Add(el.Name.LocalName, el.Value);
                        break;
                    case "After":
                        foreach (XElement el in e.Nodes())
                            afterdict.Add(el.Name.LocalName, el.Value);
                        break;
                }
            }
        }

        public void SaveXMLfile()
        {
            var doc = new XElement("Root",
               from inc in includedir select new XElement("IncludeDirs", inc),
               from exd in excludedirs select new XElement("ExcludeDirs", exd),
               from incf in includefiles select new XElement("IncludeFiles", incf),
               from exf in excludefiles select new XElement("ExcludeFiles", exf),
               from exe in excludeextn select new XElement("ExcludeExtn", exe),
               new XElement("Filename", Filename),
                new XElement("SourceDir", SourceDir),
                 new XElement("DestDir", DestDir),
                  new XElement("ZIPFilename", ZIPFilename),
                   new XElement("compression", Compression),
                   new XElement("ScheduleTime", ScheduleTime),
                   new XElement("TaskDefination", TaskDefination),
                   new XElement("Schedule", Schedule),
                   new XElement("CopyBackupFile",CopyBackupFile),
                   new XElement("ScheduleOn", ScheduleOn),
                   new XElement("Recursive", Recursive.ToString()),
                   new XElement("Completebackup", CompleteBackup.ToString()),
                   new XElement("Diffbackup", DiffBackup.ToString()),
                   new XElement("NoDifFiles", NumberDiffiles.ToString()),
                   new XElement("Logsize", Logsize.ToString()),
                   new XElement("Before", from keyValue in beforedict select new XElement(keyValue.Key, keyValue.Value)),
                   new XElement("After", from keyValue in afterdict select new XElement(keyValue.Key, keyValue.Value)));                 
                   

            doc.Save(myPath);
        }

     
        private void createxmltile(string root)
        {
            if (!File.Exists(this.myPath))
            {
                XDocument doc = new XDocument(new XElement("Root", new XElement("Filename", ""),
                    new XElement("SourceDir", ""), new XElement("DestDir", ""), new XElement("ZIPFilename", "")));
                doc.Save(this.myPath);
            }
        }
    }
}