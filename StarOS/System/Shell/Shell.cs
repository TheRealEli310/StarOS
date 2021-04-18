using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using StarOS.Apps.Terminal;

namespace StarOS.System.Terminal
{
    class Shell
    {
        public static void BeforeRun()
        {

        }
        public static void Run()
        {
            Console.Write(Kernel.CurrentVol + @":\" + Kernel.CurrentDir + ">");
            string cmd = Console.ReadLine();
            Exec(cmd);
        }
        /// <summary>
        /// Execute a command.
        /// </summary>
        /// <param name="cmd">The command to execute.</param>
        public static void Exec(string cmd)
        {
            string[] args = cmd.Split(" ");
            switch (args[0])
            {
                case "ver":
                    Console.WriteLine("StarOS (" + VersionInfo.revision + ")");
                    break;
                case "config":
                    Console.WriteLine(File.ReadAllText(@"0:\config.sos"));
                    break;
                case "cd":
                    #region messy code here
                    if (args[1] == "..")
                    {
                        if (Kernel.CurrentDir == "")
                        {
                            break;
                        }
                        char currletter = Kernel.CurrentDir[Kernel.CurrentDir.Length-1];
                        while (!(currletter == "\\".ToCharArray()[0]))
                        {
                            Kernel.CurrentDir = Kernel.CurrentDir.Remove(Kernel.CurrentDir.Length-1);
                            if (Kernel.CurrentDir.Length == 0) { break; }
                            currletter = Kernel.CurrentDir[Kernel.CurrentDir.Length-1];
                        }
                        if (Kernel.CurrentDir.Length == 0) { break; }
                        Kernel.CurrentDir = Kernel.CurrentDir.Remove(Kernel.CurrentDir.Length - 1);
                        break;
                    }
                    string bdir = Kernel.CurrentDir;
                    if (Kernel.CurrentDir == "")
                    {
                        Kernel.CurrentDir += args[1];
                    }
                    else
                    {
                        Kernel.CurrentDir += "\\" + args[1];
                    }
                    if (!Directory.Exists(Kernel.CurrentVol + ":\\" + Kernel.CurrentDir))
                    {
                        Kernel.CurrentDir = bdir;
                        Console.WriteLine("Directory not found!");
                    }
                    break;
                #endregion
                case "dir":
                case "ls":
                    string[] filePaths = Directory.GetFiles(Kernel.CurrentVol + @":\" + Kernel.CurrentDir);
                    var drive = new DriveInfo(Kernel.CurrentVol);
                    Console.WriteLine("Volume in drive 0 is " + $"{drive.VolumeLabel}");
                    Console.WriteLine("Directory of " + Kernel.CurrentVol + @":\" + Kernel.CurrentDir);
                    Console.WriteLine("\n");
                    for (int i = 0; i < filePaths.Length; ++i)
                    {
                        string path = filePaths[i];
                        Console.WriteLine(Path.GetFileName(path));
                    }
                    foreach (var d in Directory.GetDirectories(Kernel.CurrentVol + @":\" + Kernel.CurrentDir))
                    {
                        var dir = new DirectoryInfo(d);
                        var dirName = dir.Name;

                        Console.WriteLine(dirName + " <DIR>");
                    }
                    Console.WriteLine("\n");
                    Console.WriteLine("        " + $"{drive.TotalSize}" + " bytes");
                    Console.WriteLine("        " + $"{drive.AvailableFreeSpace}" + " bytes free");
                    break;
                case "clear":
                case "cls":
                    Console.Clear();
                    break;
                case "edit":
                case "miv":
                    if (args.Length == 1)
                    {
                        Console.WriteLine("Please give a filename!");
                    }
                    else
                    {
                        if (Kernel.CurrentDir == "")
                        {
                            MIV.StartMIV(args[1]);
                        }
                        else
                        {
                            MIV.StartMIV(args[1]);
                        }
                    }
                    break;
                case "cd..":
                    Exec("cd ..");
                    break;
                case "format":
                    Console.WriteLine("Formatting...");
                    Cosmos.System.FileSystem.VFS.VFSManager.Format(args[1],"fat32",true);
                    break;
                case "":
                    break;
                default:
                    if (args[0].EndsWith(".bat"))
                    {
                        if (File.Exists(Kernel.GetFullPath(args[0])))
                        {
                            Batch.Run(args[0]);
                            break;
                        }
                    }
                    Console.WriteLine("Command not found");
                    break;
            }
        }
    }
}
