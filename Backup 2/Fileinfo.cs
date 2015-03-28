using System;

namespace Backup_2
{
    /// <summary>
    /// Description of Fileinfo.
    /// </summary>
    public class Fileinfo
    {
       
        public Fileinfo(String fname, String date, String time, String directory, string originalfilename)
        {
            DestFilename = originalfilename;
            Filename = fname;
            Created = date;
            Modified = time;
            Directory = directory;
        }

        public String Filename { get; set; }
       
        public String DestFilename { get; set; }
        
        public String Created { get; set; }
        
        public String Modified { get; set; }
        
        public String Directory { get; set; }
        
    }


    public class Syncinfo
    {
        private string m_source;
        private String m_destination;
        private Boolean m_rec;
       
        public Syncinfo(String source, String destination, Boolean rec)
        {
            m_source = source;
            m_destination = destination;
            m_rec = rec;
        }

        public String source
        {
            get
            {
                return this.m_source;
            }
            set
            {
                this.m_source = value;
            }
        }

        public String destination
        {
            get
            {
                return this.m_destination;
            }
            set
            {
                this.m_destination = value;
            }
        }

        public Boolean Recursive
        {
            get
            {
                return this.m_rec;
            }
            set
            {
                this.m_rec = value;
            }
        }

      
    }

    public class MySyncArgs
    {
        public MySyncArgs(bool mycopy,bool mymirror,bool myeq,bool tstamp)
        {
            copy = mycopy;
            mirror = mymirror;
            eq = myeq;
           timestamp = tstamp;
        }
        public bool copy { get; set; }
        public bool mirror { get; set; }
        public bool eq { get; set; }
        public bool timestamp { get; set; }
        public string rootdir { get; set; }
     }

}
