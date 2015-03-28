using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Backup_2
{
	internal class Fileutils
	{
		public static void SearchDirectory(DirectoryInfo dir_info, BindingList<Fileinfo> file_list)
		{
			try
			{
				foreach (DirectoryInfo subdir_info in dir_info.GetDirectories())
				{
					SearchDirectory(subdir_info, file_list);
				}
			}
			catch
			{
			}

			try
			{
				foreach (FileInfo file_info in dir_info.GetFiles())
				{
					file_list.Add(new Fileinfo(file_info.FullName, Fileutils.Getdate(file_info.FullName), Fileutils.Getmoddate(file_info.FullName), "Directory", file_info.FullName));
				}
			}
			catch
			{
			}
		}

		public static void SearchDirectory(DirectoryInfo dir_info, BindingList<Fileinfo> file_list, bool recurse)
		{
			try
			{
				foreach (FileInfo file_info in dir_info.GetFiles())
				{
					file_list.Add(new Fileinfo(file_info.FullName, Fileutils.Getdate(file_info.FullName), Fileutils.Getmoddate(file_info.FullName), "Directory", file_info.FullName));
				}
			}
			catch
			{
			}
		}

		public static string formatDate(DateTime dtDate)
		{
			//Get date and time in short format
            string stringDate = "";
			stringDate = dtDate.ToShortDateString().ToString() + " " + dtDate.ToShortTimeString().ToString();
			return stringDate;
		}

		public static string Getdate(string fname)
		{
			DateTime dtCreateDate;
			var objFileSize = new FileInfo(fname);
			dtCreateDate = objFileSize.CreationTime; //GetCreationTime(stringFileName);
			//dtModifyDate = objFileSize.LastWriteTime; //GetLastWriteTime(stringFileName);
			//check if file is in local current day light saving time
            if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(dtCreateDate) == false)
			{
				//not in day light saving time adjust time
                return (Fileutils.formatDate(dtCreateDate.AddHours(1)));
			}
			else
			{
				//not in day light saving time adjust time
                return (Fileutils.formatDate(dtCreateDate));
			}
		}

		public static string Getmoddate(string fname)
		{
			DateTime dtModifyDate;
			FileInfo objFileSize = new FileInfo(fname);
			dtModifyDate = objFileSize.LastWriteTime; //GetLastWriteTime(stringFileName);
			//check if file is in local current day light saving time
            if (TimeZone.CurrentTimeZone.IsDaylightSavingTime(dtModifyDate) == false)
			{
				//not in day light saving time adjust time
                return (Fileutils.formatDate(dtModifyDate.AddHours(1)));
			}
			else
			{
				//not in day light saving time adjust time
                return (Fileutils.formatDate(dtModifyDate));
			}
		}

		public static int Comparemoddate(string source, string dest)
		{
			FileInfo fidest = new FileInfo(dest);
			FileInfo fisource = new FileInfo(source);
			DateTime datesource = fisource.LastWriteTime;
			DateTime datedest = fidest.LastWriteTime;
			datesource = datesource.AddMilliseconds(-datesource.Millisecond);
			datedest = datedest.AddMilliseconds(-datedest.Millisecond);
			return DateTime.Compare(datesource, datedest);
			// <0 if source is older than dest
			// ==0 if source is same as dest
			// >0 if dest is older than source
		}
		
		public static void CopyFileExactly(string copyFromPath, string copyToPath)
    	{
        var origin = new FileInfo(copyFromPath);

        origin.CopyTo(copyToPath, true);

        var destination = new FileInfo(copyToPath);
        destination.CreationTime = origin.CreationTime;
        destination.LastWriteTime = origin.LastWriteTime;
        destination.LastAccessTime = origin.LastAccessTime;
    }

        public static string formatSize(Int64 lSize)
        {
            //Format number to KB
            string stringSize = "";
            NumberFormatInfo myNfi = new NumberFormatInfo();

            Int64 lKBSize = 0;

            if (lSize < 1024)
            {
                if (lSize == 0)
                {
                    //zero byte
                    stringSize = "0";
                }
                else
                {
                    //less than 1K but not zero byte
                    stringSize = "1";
                }
            }
            else
            {
                //convert to KB
                lKBSize = lSize / 1024;
                //format number with default format
                stringSize = lKBSize.ToString("n", myNfi);
                //remove decimal
                stringSize = stringSize.Replace(".00", "");
            }

            return stringSize + " KB";
        }

        public static string getFullPath(string stringPath)
        {
            //Get Full path
            string stringParse = "";
            //remove My Computer from path.
            stringParse = stringPath.Replace("My Computer\\", "");

            return stringParse;
        }

        public static string GetPathName(string stringPath)
        {
            //Get Name of folder
            string[] stringSplit = stringPath.Split('\\');
            int _maxIndex = stringSplit.Length;
            return stringSplit[_maxIndex - 1];
        }

        public static string EscapeArguments(params string[] args)
        {
            StringBuilder arguments = new StringBuilder();
            Regex invalidChar = new Regex("[\x00\x0a\x0d]");//  these can not be escaped
            Regex needsQuotes = new Regex(@"\s|""");//          contains whitespace or two quote characters
            Regex escapeQuote = new Regex(@"(\\*)(""|$)");//    one or more '\' followed with a quote or end of string
            for (int carg = 0; args != null && carg < args.Length; carg++)
            {
                if (args[carg] == null) { throw new ArgumentNullException("args[" + carg + "]"); }
                if (invalidChar.IsMatch(args[carg])) { throw new ArgumentOutOfRangeException("args[" + carg + "]"); }
                if (args[carg] == String.Empty) { arguments.Append("\"\""); }
                else if (!needsQuotes.IsMatch(args[carg])) { arguments.Append(args[carg]); }
                else
                {
                    arguments.Append('"');
                    arguments.Append(escapeQuote.Replace(args[carg], m =>
                    m.Groups[1].Value + m.Groups[1].Value +
                    (m.Groups[2].Value == "\"" ? "\\\"" : "")
                    ));
                    arguments.Append('"');
                }
                if (carg + 1 < args.Length)
                    arguments.Append(' ');
            }
            return arguments.ToString();
        }

        public static string Getdiffilename()
        {
            string newzip = DateTime.Now.ToShortDateString() +"-"+ DateTime.Now.ToShortTimeString();
            newzip = newzip.Replace("/", "-").Replace(":", "-");
            return newzip + ".7z";

        }

        public static void DeleteDiffBackup(string path,string fname,int nofilestokeep)
        {
            if (nofilestokeep == 0) { return; }
            fname = fname + "-*.7z";
            foreach (var fi in new DirectoryInfo(@path).GetFiles(fname).OrderByDescending(x => x.LastWriteTime).Skip(nofilestokeep))
            {
                fi.Delete();
            }
        }
    }
}