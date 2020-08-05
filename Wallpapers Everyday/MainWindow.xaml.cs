using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;

using System.Windows;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Hardcodet.Wpf.TaskbarNotification;

namespace Wallpapers_Everyday
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TaskbarIcon TrayIcon;

        public MainWindow()
        {
            InitializeComponent();

            TrayIcon = new TaskbarIcon();
            TrayIcon.Icon = new Icon(Properties.Resources.trayIcon, 16, 16);
            TrayIcon.Visibility = Visibility.Visible;
            TrayIcon.ToolTipText = "Wallpapers Everyday";

            LoadSettingsToInterface();

            /*
             * План: разбиваем старый код на НОРМАЛЬНЫЕ ЧЕЛОВЕЧЕСКИЕ МЕТОДЫ .
             * ОДНА ФУНКЦИЯ - ОДНА ЗАДАЧА, БЛЯТЬ
             * В классе Logic будут специфичные функции логики проги, в том
             * числе и общая функция установки обоев, которая последовательно
             * будет выполнять все шаги, отлавливать исключения и вот это вот все.
             * При запуске проги - проверка на наличие ключа. Есть ключ - в
             * тихом режиме ставим обои той самой общей функцией в зависимости
             * от настроек проги - и все. Нет ключа - открываем окно с настройками.
             * 
             */
        }

        private void Button_Click(object sender, RoutedEventArgs e) => new AboutWindows().ShowDialog();
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => SaveSettingsFromInterface();
        
        void SaveSettingsFromInterface()
        {
            Properties.Settings.Default.Autorun = autorun.IsChecked.Value;
            Properties.Settings.Default.Debug = debug.IsChecked.Value;
            Properties.Settings.Default.AlwaysSet = alwaysSet.IsChecked.Value;
            Properties.Settings.Default.OnlyDownload = onlyDownload.IsChecked.Value;
            Properties.Settings.Default.NoNotify = withoutNotify.IsChecked.Value;
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
            debug.IsChecked = Properties.Settings.Default.Debug;
            alwaysSet.IsChecked = Properties.Settings.Default.AlwaysSet;
            onlyDownload.IsChecked = Properties.Settings.Default.OnlyDownload;
            withoutNotify.IsChecked = Properties.Settings.Default.NoNotify;
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
    }
}
