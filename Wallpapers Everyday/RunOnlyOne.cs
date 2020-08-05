using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;

namespace Wallpapers_Everyday
{
    public static class RunOnlyOne
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int ShowWindow(int hwnd, int nCmdShow);

        //[DllImport("user32.dll", SetLastError = true)]
        //internal static extern int GetWindow(int hwnd, int nCmdShow);

        static Mutex _syncObject;
        static readonly string AppPath = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName);

        /// <summary>
        /// Находит запущенную копию приложения и разворачивает окно
        /// </summary>
        /// <param name="UniqueValue">уникальное значение для каждой программы (можно имя)</param>
        /// <returns>true - если приложение было запущено</returns>
        public static bool ChekRunProgram(string UniqueValue)
        {
            bool applicationRun = false;
            _syncObject = new Mutex(true, UniqueValue, out applicationRun);
            if (!applicationRun)
            {//восстановить/развернуть окно                              
                try
                {
                    Process[] procs = Process.GetProcessesByName(AppPath);

                    foreach (Process proc in procs)
                        if (proc.Id != Process.GetCurrentProcess().Id)
                        {
                            ShowWindow((int)proc.MainWindowHandle, 1);//нормально развернутое
                            //ShowWindow((int)proc.MainWindowHandle, 3);//максимально развернутое
                            SetForegroundWindow(proc.MainWindowHandle);
                            break;
                        }
                }
                catch { return false; }
            }
            return !applicationRun;
        }
    }
}
