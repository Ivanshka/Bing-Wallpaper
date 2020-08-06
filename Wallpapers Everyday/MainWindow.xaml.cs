using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;

namespace Wallpapers_Everyday
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TaskbarIcon icon = new TaskbarIcon();

        public MainWindow()
        {
            InitializeComponent();
            icon.Icon = new Icon(Properties.Resources.trayIcon, 16, 16);
            icon.ToolTipText = "Wallpapers Everyday";
            icon.Visibility = Visibility.Visible;
            LoadSettingsToInterface();
            icon.ShowBalloonTip("Wallpapers Everyday", "Нстройки применяются только после закрытия окна!", BalloonIcon.Info);
        }

        private void Window_Closing(object sender, CancelEventArgs e) { icon.Visibility = Visibility.Hidden; SaveSettingsFromInterface(); }
        
        void SaveSettingsFromInterface()
        {
            Properties.Settings.Default.Autorun = autorun.IsChecked.Value;
            Properties.Settings.Default.Debug = debug.IsChecked.Value;
            Properties.Settings.Default.AlwaysSet = alwaysSet.IsChecked.Value;
            Properties.Settings.Default.OnlyDownload = onlyDownload.IsChecked.Value;
            Properties.Settings.Default.Notify = bigFolderNotify.IsChecked.Value;
            Properties.Settings.Default.MaxFolderSize = bigFolderSize.Value;
            Properties.Settings.Default.RemoveOld = autoRemoveOldPictures.IsChecked.Value;
            Properties.Settings.Default.Win10Intresting = saveWin10Interesting.IsChecked.Value;
            Properties.Settings.Default.Win10IntrestingPath = saveWin10InterestingPath.DirectoryPath;

            Properties.Settings.Default.Save();
        }

        void LoadSettingsToInterface()
        {
            autorun.IsChecked = Properties.Settings.Default.Autorun;
            AutorunControl(Properties.Settings.Default.Autorun);
            debug.IsChecked = Properties.Settings.Default.Debug;
            alwaysSet.IsChecked = Properties.Settings.Default.AlwaysSet;
            onlyDownload.IsChecked = Properties.Settings.Default.OnlyDownload;
            bigFolderNotify.IsChecked = Properties.Settings.Default.Notify;
            bigFolderSize.Value = Properties.Settings.Default.MaxFolderSize;
            autoRemoveOldPictures.IsChecked = Properties.Settings.Default.RemoveOld;
            saveWin10Interesting.IsChecked = Properties.Settings.Default.Win10Intresting;
            saveWin10InterestingPath.DirectoryPath = Properties.Settings.Default.Win10IntrestingPath;

            if (!Properties.Settings.Default.Notify)
                bigFolderSize.IsEnabled = false;

            if (!GetOsName().Contains("Windows 10")) // если не win 10
            {
                saveWin10Interesting.IsEnabled = false;
                saveWin10InterestingPath.IsEnabled = false;
                saveWin10InterestingRun.IsEnabled = false;
            } // если win 10 и включено сохранение
            else if (Properties.Settings.Default.Win10Intresting)
            {
                saveWin10Interesting.IsChecked = true;
                Directory.CreateDirectory("Login backgrounds");
            }
            else // если не включено
            {
                saveWin10InterestingPath.IsEnabled = false;
                saveWin10InterestingRun.IsEnabled = false;
            }
            if (Properties.Settings.Default.Win10IntrestingPath == "default")
                Properties.Settings.Default.Win10IntrestingPath = Directory.GetCurrentDirectory() + "\\Login backgrounds";

            saveWin10InterestingPath.DirectoryPath = Properties.Settings.Default.Win10IntrestingPath;

            string GetOsName()
            {
                var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                            select x.GetPropertyValue("Caption")).FirstOrDefault();
                return name != null ? name.ToString() : "Неизвестно";
            }
        }

        /// <summary>
        /// Управляет автозагрузкой программы.
        /// </summary>
        /// <param name="mode">Устанавливает значение: "true" - включить автозагрузку, "false" - выключить</param>
        private static void AutorunControl(bool mode)
        {
            string ExePath = Assembly.GetExecutingAssembly().Location + " -autorun";
            RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");

            // правим путь к файлу
            ExePath = ExePath.Replace("/", "\\");

            if (mode) // если ВКЛЮЧАЕМ автозагрузку
            {
                try
                {
                    // делаем запись в реестр
                    reg.SetValue("Wallpapers Everyday", ExePath);
                    return;
                }
                catch
                {
                    MessageBox.Show("Не удалось добавить Wallpapers Everyday в автозагрузку! Автоматическая смена обоев работать не будет.", "Wallpapers Everyday", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else // если ВЫКЛЮЧАЕМ автозагрузку
            {
                try
                {
                    reg.DeleteValue("Wallpapers Everyday");
                    return;
                }
                catch { }
            }
        }

        private void bigFolderNotify_Checked(object sender, RoutedEventArgs e) => bigFolderSize.IsEnabled = true;
        private void bigFolderNotify_UnChecked(object sender, RoutedEventArgs e) => bigFolderSize.IsEnabled = false;
        private void saveWin10Interesting_Checked(object sender, RoutedEventArgs e)
        {
            saveWin10InterestingPath.IsEnabled = true;
            saveWin10InterestingRun.IsEnabled = true;
        }
        private void saveWin10Interesting_UnChecked(object sender, RoutedEventArgs e)
        {
            saveWin10InterestingPath.IsEnabled = false;
            saveWin10InterestingRun.IsEnabled = false;
        }

        private void NextWallpaper_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // не можем вынести его как поле, т.к. если изменим обои кнопкой + обновим их, список не изменится, а нужно
            string[] images = Directory.GetFiles($"{Directory.GetCurrentDirectory()}\\Images").OrderByDescending(f => File.GetCreationTime(f)).ToArray(); // получаем список пикч упорядоченный по дате создания 
            if (Properties.Settings.Default.InstalledWallpaperIndex != 0)
            {
                Wallpaper.SetWallpaper(images[--Properties.Settings.Default.InstalledWallpaperIndex]);
                Properties.Settings.Default.Save();
            }
            else
                icon.ShowBalloonTip("Wallpapers Everyday", "Достигнуты новейшие обои!", BalloonIcon.Info);
        }

        private void PreviousWallpaper_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string[] images = Directory.GetFiles($"{Directory.GetCurrentDirectory()}\\Images").OrderByDescending(f => File.GetCreationTime(f)).ToArray(); // получаем список пикч упорядоченный по дате создания 
            if (Properties.Settings.Default.InstalledWallpaperIndex != images.Length - 1)
            {
                Wallpaper.SetWallpaper(images[++Properties.Settings.Default.InstalledWallpaperIndex]);
                Properties.Settings.Default.Save();
            }
            else
                icon.ShowBalloonTip("Wallpapers Everyday", "Достигнуты старейшие обои!", BalloonIcon.Info);
        }

        private void SaveWinLogon_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var b = sender as Button;
            if (b == null)
            {
                System.Windows.MessageBox.Show("Команда UpdateWallpaper: отправитель команды не распознан!", "Wallpapers Everyday");
                return;
            }
            b.IsEnabled = false;
            b.Content = "Минутку...";

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, ee) => Wallpaper.SaveWin10Interesting(Properties.Settings.Default.Win10IntrestingPath);

            bw.RunWorkerCompleted += (s, ee) => {
                b.IsEnabled = true;
                b.Content = "Сохранить заставки \"Windows: Интересное\"";
            };

            bw.RunWorkerAsync();
        }

        private void UpdateWallpaper_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var b = sender as Button;
            if (b == null)
            {
                System.Windows.MessageBox.Show("Команда UpdateWallpaper: отправитель команды не распознан!", "Wallpapers Everyday");
                return;
            }
            b.IsEnabled = false;
            b.Content = "Минутку...";

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, ee) => {
                var state = Logic.Work();
                switch (state.Item1)
                {
                    case Logic.FinishCode.Good: break;
                    case Logic.FinishCode.Warning: Dispatcher.Invoke(() => icon.ShowBalloonTip("Wallpapers Everyday", state.Item2, BalloonIcon.Info)); break;
                    case Logic.FinishCode.Error: Dispatcher.Invoke(() => icon.ShowBalloonTip("Wallpapers Everyday", state.Item2, BalloonIcon.Error)); break;
                    case Logic.FinishCode.Fail: MessageBox.Show(state.Item2, "Wallpapers Everyday", MessageBoxButton.OK, MessageBoxImage.Error); break;
                }
            };

            bw.RunWorkerCompleted += (s, ee) => {
                b.IsEnabled = true;
                b.Content = "Обновить обои сейчас";
            };

            bw.RunWorkerAsync();
        }

        private void ShowAboutBox_Executed(object sender, ExecutedRoutedEventArgs e) => new AboutWindows().ShowDialog();
    }
}
