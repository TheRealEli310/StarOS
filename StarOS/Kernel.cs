using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using StarOS.System.Terminal;
using StarOS.System.Utils;
using StarOS.System;

namespace StarOS
{
    public class Kernel : Sys.Kernel
    {
        Sys.FileSystem.CosmosVFS fs = new Sys.FileSystem.CosmosVFS();
        public static string CurrentDir = @"";
        public static string CurrentVol = "0";
        public static string boottime = Time.MonthString() + "/" + Time.DayString() + "/" + Time.YearString() + ", " + Time.TimeString(true, true, true);
        public static string CurrentOperation = "";
        public static Settings config;
        protected override void BeforeRun()
        {
            try
            {
                Console.WriteLine("Booting StarOS...");
                Console.WriteLine("FS init");
                Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
                Console.WriteLine("Reading settings");
                config = new Settings(@"0:\config.sos");
                string firstboot = config.Get("FirstBoot");
                if (firstboot == "null")
                {
                    config.Edit("FirstBoot", boottime);
                    config.Push();
                }
                CurrentOperation = "Shell init";
                Console.WriteLine("Shell init");
                Shell.BeforeRun();
                Console.WriteLine("StarOS booted.");
                CurrentOperation = "";
                Console.Clear();
                Shell.Exec("ver");

            }
            catch (Exception e)
            {
                Crash(e);
            }
            
        }

        protected override void Run()
        {
            try
            {
                Shell.Run();
            }
            catch (Exception e)
            {
                Crash(e);
            }
            
        }
        /// <summary>
        /// Crash the system.
        /// </summary>
        /// <param name="e">The exception that occured.</param>
        public static void Crash(Exception e)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.WriteLine("StarOS ran into a problem and cannot continue.");
            Console.WriteLine();
            Console.WriteLine(e.ToString());
            Console.WriteLine(CurrentOperation);
            while (true);
        }
        /// <summary>
        /// Get the full path of a file.
        /// </summary>
        /// <param name="name">The filename</param>
        /// <returns>The full path to the file.</returns>
        public static string GetFullPath(string name)
        {
            if (CurrentDir == "")
            {
                return Kernel.CurrentVol + @":\" + name;
            }
            else
            {
                return Kernel.CurrentVol + @":\" + Kernel.CurrentDir + "\\" + name;
            }
        }
    }
}
