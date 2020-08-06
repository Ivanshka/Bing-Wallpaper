using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
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
            if (RunOnlyOne.ChekRunProgram("Wallpapers Everyday for you!"))
            {
                Window win = new Window();
                Application.Current.MainWindow = win;
                Application.Current.Shutdown();
            }

            if (e.Args.Length > 0)
            {
                TaskbarIcon icon = new TaskbarIcon();
                icon.Icon = new Icon(Wallpapers_Everyday.Properties.Resources.trayIcon, 16, 16);
                icon.Visibility = Visibility.Visible;
                icon.ToolTipText = "Wallpapers Everyday";

                // заглушка для успешного завершения программы, без окна и App.Cur.Shutdown() прога не закроется и будет висеть в озу
                Window win = new Window();
                Application.Current.MainWindow = win;

                var state = Logic.Work();
                switch (state.Item1)
                {
                    case Logic.FinishCode.Good: break;
                    case Logic.FinishCode.Warning: icon.ShowBalloonTip("Wallpapers Everyday", state.Item2, BalloonIcon.Info); break;
                    case Logic.FinishCode.Error: icon.ShowBalloonTip("Wallpapers Everyday", state.Item2, BalloonIcon.Error); break;
                    case Logic.FinishCode.Fail: MessageBox.Show(state.Item2, "Wallpapers Everyday", MessageBoxButton.OK, MessageBoxImage.Error); break;
                }

                Thread.Sleep(5000);
                icon.Visibility = Visibility.Hidden;
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
