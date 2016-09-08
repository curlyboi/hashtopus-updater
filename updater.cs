using System;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.ServiceProcess;

namespace hashtopus_updater
{
    class updater
    {
        static void Main(string[] args)
        {
            if (args.Length < 2) return;
            bool svc = false; ServiceController svr = new ServiceController();

            if (args.Length >= 3)
            {
                svr = new ServiceController(args[2]);
                svc = true;
            }

            if (svc)
            {
                svr.Stop();
            }
            waitForQuit(args[0]);

            File.Delete(args[0]);
            File.Move(args[1], args[0]);

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                Process.Start("mono", args[0]);
            }
            else
            {
                if (svc)
                {
                    svr.Start();
                }
                else
                {
                    Process.Start(args[0]);
                }
            }

        }

        public static void waitForQuit(string procname)
        {
            // keep waiting 100 ms until desired process exits
            Process[] bezi;
            do
            {
                Thread.Sleep(100);
                bezi = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(procname));
            } while (bezi.Length > 0);
        }

    }
}
