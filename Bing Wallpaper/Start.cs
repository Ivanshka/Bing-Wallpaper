using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Wallpapers_Everyday
{
    // класс приложения
    public class Start
    {
        /// <summary>
        /// Проверяет наличие соединения с интернетом
        /// </summary>
        static bool CheckConnection()
        {
            try
            {
                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingReply pingReply = ping.Send("google.com");
                if (pingReply.Status == System.Net.NetworkInformation.IPStatus.Success)
                    return true;
                else
                    return false;
            }
            catch { return false; }
        }

        /// <summary>
        /// Иконка в трее
        /// </summary>
        public static NotifyIcon tray = new NotifyIcon();

        [STAThread]
        static void Main(string[] args)
        {
            //MessageBox.Show($"{args[0]}\n{args[1]}");
            tray.Icon = new Icon(Properties.Resources.icon, new Size(16, 16));
            tray.Text = "Wallpapers Everyday";
            tray.BalloonTipIcon = ToolTipIcon.Info;
            tray.BalloonTipTitle = "Wallpapers Everyday";
            tray.Visible = true;

            // ===== ПРОВЕРКА КЛЮЧА =====
            if (args.Length != 0)
            {
                if (args[0] == "/?")
                {
                    MessageBox.Show("Справка по ключам запуска Wallpaper Everyday\n1) Без ключа - открытие настроек\n2) Ключ \"/?\" - справка по ключам\n3) Любой другой ключ - смена обоев");
                    return;
                }

                Work();
                tray.Visible = false;
                return;
            }

            // открываем настройки
            Application.Run(new SettingsWindow());
            tray.Visible = true;
        }

        /// <summary>
        /// Метод установки обоев
        /// </summary>
        public static void Work()
        {
            // проверяем разрешение экрана
            var size = Screen.PrimaryScreen.Bounds.Size;
            // исключаем старые маленькие разрешения и неподдерживаемое 5 : 4
            if ((size.Width < 1366 && size.Height < 768) || (size.Width == 1280 && size.Height == 1024))
            {
                MessageBox.Show("К сожалению, Ваше разрешение экрана не поддерживается!", "Wallpapers Everyday", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
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
                        Vars.OriginalName = Properties.Settings.Default.Name;
                        tray.BalloonTipText = "Отсутствует соединение с интернетом.\nСтавлю последние обои...";
                        tray.ShowBalloonTip(3000);
                        Thread.Sleep(3000);
                        WallSetter.Set();

                        tray.BalloonTipText = "Готово.";

                        if (Vars.FolderSize > Properties.Settings.Default.MaxMB)
                            tray.BalloonTipText += " Размер папки с обоями превышает рекомендуемый!";

                        tray.BalloonTipText += "\nЗавершение работы...";

                        // сохраняем настройки
                        Properties.Settings.Default.Save();

                        if (!Properties.Settings.Default.NoNotify)
                        {
                            tray.ShowBalloonTip(3000);
                        Thread.Sleep(3000);
                    }

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
            if (!Properties.Settings.Default.NoNotify)
                tray.ShowBalloonTip(3000);
            if (Properties.Settings.Default.Debug == true)
                Vars.Debug("Получение кода...");
            WallSetter.GetHTML();
            if (Properties.Settings.Default.Debug == true)
            {
                Vars.Debug("Код получен:");
                Vars.Debug(Vars.HTMLCode);
            }
            tray.BalloonTipText = "Выделение ссылки...";
            if (!Properties.Settings.Default.NoNotify)
                tray.ShowBalloonTip(3000);
            if (Properties.Settings.Default.Debug == true)
                Vars.Debug("Выделяю ссылку...");
            WallSetter.GetUrlAndName();
            if (Properties.Settings.Default.Debug == true)
            {
                Vars.Debug("Ссылка выделена:");
                Vars.Debug(Vars.Url);
            }
            tray.BalloonTipText = "Загрузка обоев...";
            if (!Properties.Settings.Default.NoNotify)
                tray.ShowBalloonTip(3000);
            if (Properties.Settings.Default.Debug == true)
                Vars.Debug("Скачиваю файл...");
            WallSetter.Download();

            // если нет - еще и обои ставим
            tray.BalloonTipText = "Последний этап...";
            if (!Properties.Settings.Default.NoNotify)
                tray.ShowBalloonTip(3000);
            if (Properties.Settings.Default.OnlyDown == false)
                WallSetter.Set();

            if (Properties.Settings.Default.Debug == true)
                Vars.Debug("Вес папки с обоями: " + Vars.FolderSize + " Mb\n");

            // сохраняем настройки, т.к. имя последней обоины было изменено
            Properties.Settings.Default.Save();

            if (!Properties.Settings.Default.NoNotify)
            {
                tray.BalloonTipText = "Сохранение фонов экрана блокировки...";
                tray.ShowBalloonTip(3000);
                Thread.Sleep(3000);
            }

            WallSetter.SaveWin10Interesting(Properties.Settings.Default.Win10IntrestingPath);

            

            if (!Properties.Settings.Default.NoNotify)
            {
                tray.BalloonTipText = "Готово.";

                if (Vars.FolderSize >= Properties.Settings.Default.MaxMB)
                    tray.BalloonTipText += " Размер папки с обоями превышает рекомендуемый!";

                tray.BalloonTipText += "\nЗавершение работы...";
                tray.ShowBalloonTip(3000);
                Thread.Sleep(3000);
            }
        }
    }
}