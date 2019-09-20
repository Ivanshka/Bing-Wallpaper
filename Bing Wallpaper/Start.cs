using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Bing_Wallpaper
{
    // класс приложения
    public class Start
    {
        /// <summary>
        /// Проверяет наличие соединения с интернетом
        /// </summary>
        static bool CheckConnection()
        {
            // доступно ли сетевое подключение
            try
            {
                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingReply pingReply = ping.Send("www.bing.com");
                if (pingReply.Status == System.Net.NetworkInformation.IPStatus.Success)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        // импорт метода библиотеки для завершения работы
        [DllImport("kernel32.dll")]
        public static extern void ExitProcess([In] uint uExitCode);

        /// <summary>
        /// Иконка в трее
        /// </summary>
        public static NotifyIcon tray = new NotifyIcon();

        /// <summary>
        /// Настраиваем программу и начинаем работу
        /// </summary>
        static void Main(string[] args)
        {
            // настраиваем оконку в трее
            tray.Icon = new Icon(Properties.Resources.icon, new Size(16, 16));
            tray.Text = "Bing Wallpaper";
            tray.BalloonTipIcon = ToolTipIcon.Info;
            tray.BalloonTipTitle = "Bing Wallpaper";
            tray.Visible = true;

            // ===== ПРОВЕРКА КЛЮЧА =====
            if (args.Length != 0)
            {
                switch (args[0])
                {
                    case "-s":
                        // открываем настройки
                        Settings.Show();
                        tray.Visible = false;
                        return;
                    case "-a":
                        // открываем "О программе"
                        About.Show();
                        tray.Visible = false;
                        return;
                    default:
                        MessageBox.Show("Неизвестный ключ запуска!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        tray.Visible = false;
                        return;
                }
            }

            tray.BalloonTipText = "Запуск...";
            tray.ShowBalloonTip(3000);

            // проверяем, какое разрешение экрана. если 5 : 4 - вырубаем
            if ((Screen.PrimaryScreen.Bounds.Size.Width == 1280) && (Screen.PrimaryScreen.Bounds.Size.Height == 1024))
                {
                    MessageBox.Show("К сожалению, Ваше разрешение экрана не поддерживается. Программа будет закрыта.", "Bing Wallpaper", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    ExitProcess(0);
                }

            // получаем полный путь к файлу
            Vars.FullExePath = Application.StartupPath.ToString();

            // готовимся к записи логов
            if (Properties.Settings.Default.Debug == true)
                Vars.DebugInit();


            // подсчитываем размер папки с обоями
            Vars.FolderSize = Vars.GetDirectorySize(Vars.FullExePath + "\\images");

            // если инета нет
            if (CheckConnection() == false)
            {
                // но стоит галка на постоянном обновлении и нет "только загрузка"
                if ((Properties.Settings.Default.AlwaysRun) && (!Properties.Settings.Default.OnlyDown))
                {
                    // то проверяем, первый ли запуск. и если не первый (т.е. уже есть какой-то скачанный файл)
                    if (Properties.Settings.Default.Name != "no")
                    {
                        if (Properties.Settings.Default.Debug == true)
                            Vars.Debug("Доступа к Интернету нет. Ставлю последние обои...");
                        // то ставим последнюю обоину
                        Vars.WEBFILENAME = Properties.Settings.Default.Name;
                        tray.BalloonTipText = "Отсутствует соединение с интернетом.\nСтавлю последние обои...";
                        tray.ShowBalloonTip(3000);
                        Thread.Sleep(3000);
                        WallInstaller.Install();

                        tray.BalloonTipText = "Готово.";

                        if (Vars.FolderSize > Properties.Settings.Default.MaxMB)
                            tray.BalloonTipText += " Размер папки с обоями превышает рекомендуемый!";

                        tray.BalloonTipText += "\nЗавершение работы...";

                        // сохраняем настройки
                        Properties.Settings.Default.Save();

                        tray.ShowBalloonTip(3000);
                        Thread.Sleep(3000);

                        tray.Visible = false;

                        return;
                    }
                    else
                    {
                        // если запуск проги не первый
                        tray.BalloonTipText = "Отсутствует соединение с интернетом и нет ни одного файла обоев!\nЗавершение работы...";
                        tray.ShowBalloonTip(3000);
                        Thread.Sleep(3000);
                        tray.Visible = false;
                        return;
                    }
                }
                else
                {
                    tray.BalloonTipText = "Ошибка! Отсутствует соединение с интернетом!\nЗавершение работы...";
                    tray.ShowBalloonTip(3000);
                    Thread.Sleep(3000);
                    tray.Visible = false;
                    return;
                }
            }

            // если папки для заставок нет, создаем ее
            if (!Directory.Exists(Vars.FullExePath + "\\images\\"))
            {
                Directory.CreateDirectory(Vars.FullExePath + "\\images");
            }

            // если "только загрузка" - только загружаем
            tray.BalloonTipText = "Получение кода...";
            tray.ShowBalloonTip(3000);
            if (Properties.Settings.Default.Debug == true)
                Vars.Debug("Получение кода...");
            WallInstaller.GetHTML();
            if (Properties.Settings.Default.Debug == true)
            {
                Vars.Debug("Код получен:");
                Vars.Debug(Vars.HTMLCode);
            }
            tray.BalloonTipText = "Выделение ссылки...";
            tray.ShowBalloonTip(3000);
            if (Properties.Settings.Default.Debug == true)
                Vars.Debug("Выделяю ссылку...");
            WallInstaller.GetURL();
            if (Properties.Settings.Default.Debug == true)
            {
                Vars.Debug("Ссылка выделена:");
                Vars.Debug(Vars.Url);
            }
            tray.BalloonTipText = "Загрузка обоев...";
            tray.ShowBalloonTip(3000);
            if (Properties.Settings.Default.Debug == true)
                Vars.Debug("Скачиваю файл...");
            WallInstaller.Download();

            // если нет - еще и обои ставим
            tray.BalloonTipText = "Последний этап...";
            tray.ShowBalloonTip(3000);
            if (Properties.Settings.Default.OnlyDown == false)
                WallInstaller.Install();

            if (Properties.Settings.Default.Debug == true)
                Vars.Debug("Вес папки с обоями: " + Vars.FolderSize + " Mb\n");

            // сохраняем настройки, т.к. имя последней обоины было изменено
            Properties.Settings.Default.Save();

            tray.BalloonTipText = "Готово.";

            if (Vars.FolderSize >= Properties.Settings.Default.MaxMB)
                tray.BalloonTipText += " Размер папки с обоями превышает рекомендуемый!";

            tray.BalloonTipText += "\nЗавершение работы...";

            tray.ShowBalloonTip(3000);
            Thread.Sleep(3000);

            tray.Visible = false;
        }
    }
}