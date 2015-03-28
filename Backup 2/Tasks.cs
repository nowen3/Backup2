using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Backup_2
{
    public delegate void OnTaskLoggerEvent(string msg);

    internal class Tasks
    {
        private Dictionary<string, string> beforedict = new Dictionary<string, string>();

        public event OnTaskLoggerEvent mylogger;

        public Tasks(Dictionary<string, string> mytasks)
        {
            beforedict = mytasks;
        }

        public void RunTasks()
        {
            foreach (KeyValuePair<string, string> entry in beforedict)
            {
                string taskname = entry.Key;
                string taskcommand = entry.Value;

                switch (taskname)
                {
                    case "StartProgram":
                        startexe(taskcommand, "");
                        break;

                    case "Pause":
                        PauseProgram(Convert.ToInt32(taskcommand));
                        break;
                }
            }
        }

        private void startexe(string fname, string args)
        {
            if (args == "")
            {
                if (mylogger != null) { mylogger("Starting program: " + fname); }
                Process.Start(fname);
            }
            else
            {
                if (mylogger != null) { mylogger("Starting program with arguments: " + fname); }
                Process.Start(fname, args);
            }
        }

        private void PauseProgram(int noseconds)
        {
            if (mylogger != null) { mylogger("Pause for: " + noseconds.ToString() + " seconds"); }
            System.Threading.Thread.Sleep(noseconds * 1000);
            if (mylogger != null) { mylogger("Finished Pause"); }
        }
    }
}