using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

using Hardcodet.Wpf.TaskbarNotification;

namespace Wallpapers_Everyday
{
    public static class Logic
    {
        /// <summary>
        /// Метод получения ссылки из HTML-кода страницы.
        /// </summary>
        /// <param name="htmlCode">HTML-код для выделения ссылки.</param>
        /// <exception cref="Exception"/>
        /// <returns>Строка, содержащая ссылку на изображение.</returns>
        static string SelectUrlFromHtml(string htmlCode)
        {
            try
            {
                int point;
                // выделяем ссылку из текста через поиск точек
                point = htmlCode.IndexOf("link rel=\"preload\" href=\""); // ищем точку начала ссылки
                htmlCode = htmlCode.Remove(0, point + 25); //удаляем лишний код с учетом точки для поиска
                point = htmlCode.IndexOf("\" as=\"image\""); // ищем точку конца ссылки
                char[] temp = new char[point];
                htmlCode.CopyTo(0, temp, 0, point);
                return new string(temp);
            }
            catch (Exception e) { throw new Exception("Ошибка выделения ссылки!", e); }
        }

        /// <summary>
        /// Логический метод установки обоев. Выполняет все требуемые шаги (получение кода, выделение ссылки и т.д.), требуемые для установки свежих обоев.
        /// </summary>
        /// <param name="win">окно, содержащее иконку; окно требуется для обработки событий иконки, в иконку будет помещаться информация</param>
        /// <param name="hide">Флаг, обозначающий, требуется ли скрывать окно при работе. Должен принять значение true, если передано "new MainWindow()"</param>
        public static void Work(MainWindow win, bool hide)
        {
            /*NotifyIcon win.TrayIcon = new NotifyIcon();
            win.TrayIcon.Icon = new Icon(Properties.Resources.icon, 16, 16);
            win.TrayIcon.Text = "Wallpapers Everyday";
            win.TrayIcon.BalloonTipIcon = ToolTipIcon.Info;
            win.TrayIcon.BalloonTipTitle = "Wallpapers Everyday";
            win.TrayIcon.Visible = true;*/

            //MessageBox.Show("Начало теста");

            Logs.WriteLogFile($@"{DateTime.Now}: запуск...");
            if (hide)
            {
                win.WindowState = WindowState.Minimized;
                win.Show();
            }

            win.TrayIcon.ToolTipText = "Проверка параметров...";

            var size = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
            // исключаем старые маленькие разрешения и неподдерживаемое 5 : 4
            if ((size.Width < 1366 && size.Height < 768) || (size.Width == 1280 && size.Height == 1024))
            {
                MessageBox.Show("К сожалению, Ваше разрешение экрана не поддерживается!", "Wallpapers Everyday", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            string workDirectory = Directory.GetCurrentDirectory();
            // если папки для заставок нет, создаем ее
            Directory.CreateDirectory(workDirectory + @"\images");
            int imagesSize = GetDirectorySize(workDirectory + "\\images");
            string originalName;
            // ===============================================================
            // ===============================================================
            // ===============================================================

            // если инета нет
            if (!Web.CheckConnection())
            {
                // но стоит галка на постоянном обновлении и нет "только загрузка"
                if ((Properties.Settings.Default.AlwaysSet) && (!Properties.Settings.Default.OnlyDownload))
                {
                    // то проверяем, первый ли запуск. и если не первый (т.е. уже есть какой-то скачанный файл)
                    string[] images = Directory.GetFiles(@"\Images").OrderBy(f => File.GetCreationTime(f)).ToArray(); // получаем список пикч упорядоченный по дате создания 
                    if (images.Length > 0)
                    {
                        Logs.WriteLogFile("Доступа к Интернету нет. Ставлю последние обои...");
                        // то ставим последнюю обоину
                        originalName = images[0];
                        win.TrayIcon.ShowBalloonTip("Wallpapers Everyday", "Отсутствует соединение с интернетом.\nСтавлю последние обои.", BalloonIcon.Error);
                        Wallpaper.SetWallpaper(originalName);

                        if (imagesSize > Properties.Settings.Default.MaxFolderSize && !Properties.Settings.Default.NoNotify)
                            win.TrayIcon.ShowBalloonTip("Wallpapers Everyday", $"Размер папки с обоями превышает рекомендуемый! ({imagesSize} MB)", BalloonIcon.Warning);

                        // сохраняем настройки
                        Properties.Settings.Default.Save();

                        if (hide)
                        {
                            Thread.Sleep(3000);
                            win.TrayIcon.Visibility = Visibility.Hidden;
                            Application.Current.Shutdown();
                        }
                    }
                    else
                    {
                        // если же первый
                        win.TrayIcon.ShowBalloonTip("Wallpapers Everyday", "Отсутствует соединение с интернетом!", BalloonIcon.Error);
                        if (hide)
                        {
                            Thread.Sleep(3000);
                            win.TrayIcon.Visibility = Visibility.Hidden;
                            Application.Current.Shutdown();
                        }
                        return;
                    }
                }
                else // если же нет "постоянного обновления" или есть "только загрузка"
                {
                    win.TrayIcon.ShowBalloonTip("Wallpapers Everyday", "Отсутствует соединение с интернетом!", BalloonIcon.Error);
                    if (hide)
                    {
                        Thread.Sleep(3000);
                        win.TrayIcon.Visibility = Visibility.Hidden;
                        Application.Current.Shutdown();
                    }
                }
            }

            // если "только загрузка" - только загружаем
            win.TrayIcon.ToolTipText = "Получение кода...";
            if (Properties.Settings.Default.Debug)
                Logs.WriteLogFile("Получение кода...");

            string temp = null;
            try
            {
                temp = Web.GetHtmlCode("https://www.bing.com/");
            }
            catch (Exception e)
            {
                Logs.WriteLogFile($"Исключение: {e.Message}\nВнутреннее исключение: {e.InnerException.Message}");
                win.TrayIcon.ShowBalloonTip("Wallpapers Everyday", "В ходе работы произошла ошибка!\nПодробности в логе.", BalloonIcon.Error);
                if (hide)
                {
                    Thread.Sleep(3000);
                    win.TrayIcon.Visibility = Visibility.Hidden;
                    Application.Current.Shutdown();
                }
            }
            
            Logs.WriteLogFile("Код получен:");
            Logs.WriteLogFile(temp);

            win.TrayIcon.ToolTipText = "Выделение ссылки...";
            if (Properties.Settings.Default.Debug)
                Logs.WriteLogFile("Выделение ссылки...");

            try
            {
                temp = SelectUrlFromHtml(temp);
            }
            catch (Exception e)
            {
                Logs.WriteLogFile($"Исключение: {e.Message}\nВнутреннее исключение: {e.InnerException.Message}");
                win.TrayIcon.ShowBalloonTip("Wallpapers Everyday", "В ходе работы произошла ошибка!\nПодробности в логе.", BalloonIcon.Error);
                if (hide)
                {
                    Thread.Sleep(3000);
                    win.TrayIcon.Visibility = Visibility.Hidden;
                    Application.Current.Shutdown();
                }
            }

            if (Properties.Settings.Default.Debug)
            {
                Logs.WriteLogFile("Ссылка выделена:");
                Logs.WriteLogFile(temp);
            }

            win.TrayIcon.ToolTipText = "Загрузка обоев...";
            if (Properties.Settings.Default.Debug)
                Logs.WriteLogFile("Загрузка обоев...");

            string name = null;

            try
            {
                Web.Download("https://www.bing.com" + temp, $@"{workDirectory}\images\{name = SelectNameFromUrl(temp)}");
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    Logs.WriteLogFile($"Исключение: {e.Message}\nВнутреннее исключение: {e.InnerException.Message}");
                else
                    Logs.WriteLogFile($"Исключение: {e.Message}");
                win.TrayIcon.ShowBalloonTip("Wallpapers Everyday", "В ходе работы произошла ошибка!\nПодробности в логе.", BalloonIcon.Error);
                if (hide)
                {
                    Thread.Sleep(3000);
                    win.TrayIcon.Visibility = Visibility.Hidden;
                    return;
                }
            }

            // если нет - еще и обои ставим
            if (!Properties.Settings.Default.OnlyDownload)
            {
                win.TrayIcon.ToolTipText = "Установка обоев...";
                Wallpaper.SetWallpaper($@"{workDirectory}\images\{name}");
            }

            if (Properties.Settings.Default.Debug)
                Logs.WriteLogFile($"Вес папки с обоями: {imagesSize} Mb");

            if (Properties.Settings.Default.Win10Intresting)
            {
                win.TrayIcon.ToolTipText = "Сохранение фонов экрана блокировки...";
                Wallpaper.SaveWin10Interesting(Properties.Settings.Default.Win10IntrestingPath);
                GC.Collect(); // после сохранения WE жрет ОЧЕНЬ много памяти, большая часть которой ему уже нах не нужна => очищаем
            }

            if (!Properties.Settings.Default.NoNotify)
            {
                win.TrayIcon.ToolTipText = "Wallpapers Everyday";

                if (imagesSize > Properties.Settings.Default.MaxFolderSize)
                    win.TrayIcon.ShowBalloonTip("Wallpapers Everyday", $"Размер папки с обоями превышает рекомендуемый! ({imagesSize} MB)", BalloonIcon.Warning);
                if (hide)
                {
                    Thread.Sleep(3000);
                    win.TrayIcon.Visibility = Visibility.Hidden;
                    return;
                }
            }

            // ===============================================================
            // ===============================================================
            // ===============================================================
            //MessageBox.Show("Конец теста");
            win.TrayIcon.Visibility = Visibility.Hidden;

            return;
        }

        /// <summary>
        /// Выделяет имя файла из ссылки на него.
        /// </summary>
        /// <param name="url">URL для выделения имени.</param>
        /// <returns>Имя файла для сохранения.</returns>
        static string SelectNameFromUrl(string url) => url.Substring(7, url.IndexOf(".jpg") - 6 + 3); // +3 из-за расширения

        /// <summary>
        /// Вычисляет объем папки с обоями
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns>Размер папки в мегабайтах</returns>
        static int GetDirectorySize(string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
            long sum = 0;
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fi = new FileInfo(files[i]);
                sum += fi.Length;
            }

            return (int)(sum / 1024 / 1024);
        }
    }
}
