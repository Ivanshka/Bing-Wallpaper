using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Wallpapers_Everyday
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            RunOnlyOne.ChekRunProgram("Wallpaper Everyday for you!");
            if (e.Args.Length > 0)
            {
                MainWindow win = new MainWindow();
                Application.Current.MainWindow = win;
                Logic.Work(win, true);
                Application.Current.Shutdown();
            }
            else
            {
                MainWindow win = new MainWindow();
                Application.Current.MainWindow = win;
                win.Show();
            }
        }
    }
}
