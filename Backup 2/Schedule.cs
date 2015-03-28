using Microsoft.Win32.TaskScheduler;
using System;
using System.IO;
using System.Reflection;

namespace Backup_2
{
    internal class MySchedule
    {
        private string filepath = string.Empty;
        private DateTime startdate = DateTime.Today;
        private DaysOfTheWeek daysofweek = DaysOfTheWeek.Monday;
        private string arguments = string.Empty;
        private bool enabled = true;
        private int dayofmonth = 1;
        private string TaskDefinition = string.Empty;
        private Guid id = Guid.NewGuid();
        private string TriggerType = "Weekly";
        private int StartTime = 1;
        private int StartMin = 0;

        //schedule daily

        public MySchedule(string taskname)
        {
            TaskDefinition = taskname;
        }

        public MySchedule(string FilePath, int starttime, int startmin, string Arguments, string triggertype)
        {
            filepath = FilePath;
            StartTime = starttime;
            StartMin = startmin;
            arguments = Arguments;
            TaskDefinition = "Nicks-Daily-Task{" + id + "}";
            TriggerType = triggertype;
        }

        //schedule weekly
        public MySchedule(string FilePath, DateTime StartDate, DaysOfTheWeek DaysOfWeek, string Arguments, string triggertype)
        {
            filepath = FilePath;
            startdate = StartDate;
            daysofweek = DaysOfWeek;
            arguments = Arguments;
            TaskDefinition = "Nicks-weekly-Task{" + id + "}";
            TriggerType = triggertype;
        }

        // Schedule monthly
        public MySchedule(string FilePath, DateTime StartDate, int DayOfMonth, string Arguments, string triggertype)
        {
            filepath = FilePath;
            startdate = StartDate;
            dayofmonth = DayOfMonth;
            arguments = Arguments;
            TaskDefinition = "Nicks-monthly-Task{" + id + "}";
            TriggerType = triggertype;
        }

        public bool EnableSchedule
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public string TaskDefinitionName
        {
            get { return TaskDefinition; }
            set { TaskDefinition = value; }
        }

        public string TaskDefinitionNameGUID
        {
            get { return TaskDefinition; }
            set
            {
                string taskdef = TaskDefinition + "{" + id + "}";
                taskdef = value;
            }
        }

        public int SetHours
        {
            get { return StartTime; }
            set { StartTime = value; }
        }

        public int SetMinutes
        {
            get { return StartMin; }
            set { StartMin = value; }
        }

        public void SaveMySchedule()
        {
            SaveSchedule();
        }

        private void SaveSchedule()
        {
            var dom = new int[] { dayofmonth };
            var ts = new TaskService();
            // Create a new task definition and assign properties
            var td = ts.NewTask();
            td.RegistrationInfo.Author = "Nick Owen";
            td.RegistrationInfo.Description = "Nicks Backup of " + Path.GetFileName(arguments);

            if (TriggerType == "Daily")
            {
                DailyTrigger mTrigger = new DailyTrigger();
                mTrigger = GetDailyTrigger();
                td.Triggers.Add(mTrigger);
            }

            if (TriggerType == "Weekly")
            {
                WeeklyTrigger mTrigger = new WeeklyTrigger();
                mTrigger = GetWeeklyTigger();
                td.Triggers.Add(mTrigger);
            }

            if (TriggerType == "Monthly")
            {
                MonthlyTrigger mTrigger = new MonthlyTrigger();
                mTrigger = GetMonthlyTrigger();
                td.Triggers.Add(mTrigger);
            }

            //create new action
            var action = new ExecAction(Assembly.GetExecutingAssembly().Location, null, null);
            if (filepath != string.Empty && File.Exists(filepath))
            {
                action = new ExecAction("\"" + filepath + "\"");
            }
            action.Arguments = "\"-OB\" " + "\"" + arguments + "\"";
            td.Actions.Add(action);
            ts.RootFolder.RegisterTaskDefinition(TaskDefinition, td);
        }

        private BootTrigger GetBoottrigger()
        {
            BootTrigger mTrigger = new BootTrigger();
            mTrigger.Delay = TimeSpan.FromMinutes(5);
            return mTrigger;
        }

        private DailyTrigger GetDailyTrigger()
        {
            DailyTrigger mTrigger = new DailyTrigger();
            mTrigger.StartBoundary = DateTime.Today + TimeSpan.FromHours(StartTime) + TimeSpan.FromMinutes(StartMin);
            return mTrigger;
        }

        private WeeklyTrigger GetWeeklyTigger()
        {
            WeeklyTrigger mTrigger = new WeeklyTrigger();
            mTrigger.StartBoundary = startdate + TimeSpan.FromHours(StartTime) + TimeSpan.FromMinutes(StartMin);
            mTrigger.DaysOfWeek = daysofweek;
            mTrigger.Enabled = true;
            return mTrigger;
        }

        private MonthlyTrigger GetMonthlyTrigger()
        {
            MonthlyTrigger mTrigger = new MonthlyTrigger();
            var dom = new int[] { dayofmonth };
            mTrigger.StartBoundary = startdate + TimeSpan.FromHours(StartTime) + TimeSpan.FromMinutes(StartMin);
            mTrigger.DaysOfMonth = dom;
            mTrigger.Enabled = true;
            return mTrigger;
        }

        public bool DeleteTask()
        {
            var ts = new TaskService();
            if (ts.RootFolder.Tasks.Exists(TaskDefinition))
            {
                ts.RootFolder.DeleteTask(TaskDefinition);
                return true;
            }
            else return false;
        }

        public bool TaskExists()
        {
            var ts = new TaskService();
            if (ts.RootFolder.Tasks.Exists(TaskDefinition))
            {
                return true;
            }
            else return false;
        }
    }
}