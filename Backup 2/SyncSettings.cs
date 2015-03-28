using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Backup_2
{
    class SyncSettings
    {
         private string myPath = string.Empty;
 
       
        private List<string> excludedirs = new List<string>();
        private List<string> excludefiles = new List<string>();
        private List<string> excludeextn = new List<string>();
        private List<Syncinfo> mysyncjobs = new List<Syncinfo>();
        private Dictionary<string, string> beforedict = new Dictionary<string, string>();
        private Dictionary<string, string> afterdict = new Dictionary<string, string>();

        public SyncSettings(string fname)
        {
            this.myPath = fname;
            if (!File.Exists(myPath))
            {
                this.createxmltile("Root");
            }
            
            Recursive = true;
           
            Logsize = 0;
            LoadXMLfile();
        }

       
        public String Filename { get; set; }
        public String TaskDefination { get; set; }
        public String ScheduleTime { get; set; }
        public String Schedule { get; set; }
        public String ScheduleOn { get; set; }
        public bool Recursive { get; set; }
        public decimal Logsize { get; set; }

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
                     case "ScheduleTime":
                        ScheduleTime = e.Value;
                        break;

                    case "TaskDefination":
                        TaskDefination = e.Value;
                        break;
                    case "ExcludeDirs":
                        excludedirs.Add(e.Value);
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
                    case "Logsize":
                        Logsize = Decimal.Parse(e.Value);
                        break;
                    case "Before":
                        foreach (XElement el in e.Nodes())
                            beforedict.Add(el.Name.LocalName, el.Value);
                        break;
                    case "After":
                        foreach (XElement el in e.Nodes())
                            afterdict.Add(el.Name.LocalName, el.Value);
                        break;
                    case "Syncjobs":
                        string sou = "";
                        string dest = "";
                        string rec = "";
                        foreach (XElement el in e.Nodes())
                        {
                            if (el.Name.LocalName == "source") {  sou = el.Value; }
                            if (el.Name.LocalName == "destination") {  dest = el.Value; }
                            if (el.Name.LocalName == "recursive") {  rec = el.Value; }
                        }
                        if (sou !="" && dest != "" && rec !="")
                        { mysyncjobs.Add(new Syncinfo(sou, dest, Convert.ToBoolean(rec))); }
                            
                        break;
                }
            }
        }

        public void SaveXMLfile()
        {
            var doc = new XElement("Root",
               from exd in excludedirs select new XElement("ExcludeDirs", exd),
               from exf in excludefiles select new XElement("ExcludeFiles", exf),
               from exe in excludeextn select new XElement("ExcludeExtn", exe),
               from jobs in mysyncjobs select new XElement("Syncjobs",
                             new XElement("source", jobs.source),
                               new XElement("destination", jobs.destination),
                               new XElement("recursive", jobs.Recursive.ToString())),

                   new XElement("Filename", Filename),
                   new XElement("ScheduleTime", ScheduleTime),
                   new XElement("TaskDefination", TaskDefination),
                   new XElement("Schedule", Schedule),
                   new XElement("ScheduleOn", ScheduleOn),
                   new XElement("Logsize", Logsize.ToString()),
                   new XElement("Before", from keyValue in beforedict select new XElement(keyValue.Key, keyValue.Value)),
                   new XElement("After", from keyValue in afterdict select new XElement(keyValue.Key, keyValue.Value)));                 
                   

            doc.Save(myPath);
        }

     
        private void createxmltile(string root)
        {
            if (!File.Exists(this.myPath))
            {
                XDocument doc = new XDocument(new XElement("Root", new XElement("Filename", "")));
                doc.Save(this.myPath);
            }
        }
    }
}
