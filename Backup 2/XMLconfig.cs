using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Backup_2
{
    internal class XMLconfig
    {
        private string myPath = string.Empty;

        public XMLconfig(string fname)
        {
            this.myPath = fname;
            if (!File.Exists(myPath))
            {
                createxmltile();
            }
        }

        private void createxmltile()
        {
            if (!File.Exists(this.myPath))
            {
                XDocument doc = new XDocument(new XElement("Root"));
                doc.Save(this.myPath);
            }
        }

        public void Savelist(BindingList<Fileinfo> list, string root)
        {
            if (!File.Exists(this.myPath))
                createxmltile();
            XElement xmlfromlist = new XElement(root, from a in list
                                                        select
                                                            new XElement("Directory",
                                                            new XElement("Filename", this.Stripescape(a.Filename)),
                                                            new XElement("Created", a.Created),
                                                            new XElement("Modified", a.Modified)));
            xmlfromlist.Save(this.myPath);
        }

        public List<string> Loadlist(string catagory)
        {
            List<string> returnlist = new List<string>();
            XDocument doc = XDocument.Load(myPath);
            IEnumerable<XElement> childList = from el in doc.Descendants(catagory) select el;
            foreach (XElement p in childList)
            {
                returnlist.Add(p.Value.ToString());
            }
            return returnlist;
        }

        public string Getmoddate(string name)
        {
            XElement doc = XElement.Load(myPath);
            IEnumerable<XElement> childList = from el in doc.Elements("Directory") where (string)el.Element("Filename") == name select el;
            string result = childList.ElementAt(0).Element("Modified").Value.ToString();
            return result;
        }

        public void Modifyrecord(string name, string newtime)
        {
            string safeXmlString = System.Security.SecurityElement.Escape(name);
            XElement doc = XElement.Load(myPath);
            IEnumerable<XElement> myrecord = from el in doc.Elements("Directory") where (string)el.Element("Filename") == safeXmlString select el;
            if (myrecord != null)
            {
                myrecord.ElementAt(0).Element("Modified").Value = newtime;
            }
            doc.Save(this.myPath);
        }

        public BindingList<Fileinfo> Getmodified()
        {
            BindingList<Fileinfo> returnlist = new BindingList<Fileinfo>();
            XDocument doc = XDocument.Load(myPath);
            IEnumerable<XElement> childList = from el in doc.Descendants("Directory") where (string)el.Element("Created") != (string)el.Element("Modified") select el;
            foreach (XElement p in childList)
            {
            	returnlist.Add(new Fileinfo(p.Element("Filename").Value,p.Element("Created").Value,p.Element("Modified").Value, "Directory",p.Element("Filename").Value));
            }
            return returnlist;
        }

        public void Addrecord(string fname, string cdate, string mdate)
        {
            if (!File.Exists(this.myPath))
                createxmltile();
            XElement doc = XElement.Load(myPath);
            XElement newrecord = new XElement("Directory", new XElement("Filename", Stripescape(fname)), new XElement("Created", cdate), new XElement("Modified", mdate));
            

            doc.Add(newrecord);
            doc.Save(this.myPath);
        }

        public void Addelement(string myelement, string mycatagory)
        {
            if (!File.Exists(this.myPath))
                createxmltile();
            XElement doc = XElement.Load(myPath);
            XElement at = new XElement(myelement, mycatagory);
            doc.Add(at);
            doc.Save(this.myPath);
        }

        private string Stripescape(string xmlstring)
        {
            string tempxml = xmlstring.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
            return tempxml;
        }

        private string Restorescape(string xmlstring)
        {
            string tempxml = xmlstring.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&apos;", "'");
            return tempxml;
        }

        public void DeleteElement(string ename)
        {
            var mydoc = XDocument.Load(this.myPath);
            mydoc.Element("Root").Elements(ename).Remove();
            mydoc.Save(this.myPath);
        }
    }
}