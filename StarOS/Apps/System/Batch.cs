using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace StarOS.Apps.Terminal
{
    class Batch
    {
        public static void Run(string filename)
        {
            string[] lines = File.ReadAllLines(Kernel.GetFullPath(filename));
            foreach (var cmd in lines)
            {
                System.Terminal.Shell.Exec(cmd);
            }
        }
    }
}
