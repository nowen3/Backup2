//hello
using Microsoft.Win32.TaskScheduler;
using System;

//using SevenZip.Sdk.Compression.Lzma;
//using SevenZip;

namespace Backup_2
{
    public class Textract
    {
        public string dir { get; set; }
        public string fname { get; set; }
    }

    public static class BackupUtils
    {
        public static void ScheduleDaily(string fname, int myhours, int myminutes, string arguments, string ScheduleTaskName, string inifile)
        {
            var mysetting = new BackupSettings(inifile);
            var dailyschedule = new MySchedule(fname, myhours, myminutes, arguments, "Daily");
            dailyschedule.TaskDefinitionName = ScheduleTaskName;
            mysetting.TaskDefination = ScheduleTaskName;
            mysetting.Schedule = "Daily";
            mysetting.ScheduleTime = myhours.ToString() + ":" + myminutes.ToString();
            mysetting.SaveXMLfile();
            dailyschedule.SaveMySchedule();
        }

        public static void ScheduleWeekly(string fname, int myhours, int myminutes, string arguments, string ScheduleTaskName, string scheduleon, string inifile)
        {
            // var inibackup = new XMLconfig(inifile);
            var mysetting = new BackupSettings(inifile);
            DaysOfTheWeek dow = DaysOfTheWeek.Sunday;
            dow = (DaysOfTheWeek)Enum.Parse(typeof(DaysOfTheWeek), scheduleon, true);
            var weekyschedule = new MySchedule(fname, DateTime.Today, dow, arguments, "Weekly");
            weekyschedule.SetHours = myhours;
            weekyschedule.SetMinutes = myminutes;
            weekyschedule.TaskDefinitionName = ScheduleTaskName;
            mysetting.TaskDefination = ScheduleTaskName;
            mysetting.Schedule = "Weekly";
            mysetting.ScheduleTime = myhours.ToString() + ":" + myminutes.ToString();
            mysetting.ScheduleOn = scheduleon;
            mysetting.SaveXMLfile();
            weekyschedule.SaveMySchedule();
        }

        public static void ScheduleMonthly(string fname, int myhours, int myminutes, string arguments, string ScheduleTaskName, string scheduleon, string inifile)
        {
            var mysetting = new BackupSettings(inifile);
            int dom;
            int.TryParse(scheduleon, out dom);
            var monthlyschedule = new MySchedule(fname, DateTime.Today, dom, arguments, "Monthly");
            monthlyschedule.SetHours = (myhours);
            monthlyschedule.SetMinutes = myminutes;
            monthlyschedule.TaskDefinitionName = ScheduleTaskName;
            mysetting.TaskDefination = ScheduleTaskName;
            mysetting.Schedule = "Monthly";
            mysetting.ScheduleTime = myhours.ToString() + ":" + myminutes.ToString();
            mysetting.ScheduleOn = scheduleon;
            mysetting.SaveXMLfile();
            monthlyschedule.SaveMySchedule();
        }
    }// end of class
}