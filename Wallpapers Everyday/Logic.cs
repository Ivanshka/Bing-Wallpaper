using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms.PropertyGridInternal;
using Hardcodet.Wpf.TaskbarNotification;

namespace Wallpapers_Everyday
{
    public static class Logic
    {
        /// <summary>
        /// Код завершения работы функции
        /// </summary>
        public enum FinishCode {
            /// <summary>
            /// Успешное завершение работы функции
            /// </summary>
            Good,
            /// <summary>
            /// Успешное завершение работы функции, но есть предупреждение
            /// </summary>
            Warning,
            /// <summary>
            /// Ошибка в работе функции
            /// </summary>
            Error,
            /// <summary>
            /// Отказ работы
            /// </summary>
            Fail
        }

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
        /// <returns>Код завершения функции и сопровождающее сообщение.</returns>
        public static (FinishCode, string) Work()
        {
            if (Properties.Settings.Default.Debug)
                Logs.WriteLogFile($"\n\n\n\n\n[{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}]: запуск...");

            var size = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
            // исключаем старые маленькие разрешения и неподдерживаемое 5 : 4
            if ((size.Width < 1280 && size.Height < 720) || (size.Width == 1280 && size.Height == 1024))
            {
                return (FinishCode.Fail, "К сожалению, Ваше разрешение экрана слишком мало или не поддерживается!");
            }

            string workDirectory = Directory.GetCurrentDirectory();
            // если папки для заставок нет, создаем ее
            Directory.CreateDirectory(workDirectory + @"\images");
            int imagesSize;
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
                    if (Properties.Settings.Default.Name != "no")
                    {
                        if (Properties.Settings.Default.Debug)
                            Logs.WriteLogFile("Доступа к Интернету нет. Ставлю последние обои...");
                        // то ставим последнюю обоину
                        Wallpaper.SetWallpaper(workDirectory + "\\images\\" + Properties.Settings.Default.Name);
                        string notification = "Отсутствует соединение с интернетом! Были поставлены последние обои.";

                        imagesSize = GetDirectorySize(workDirectory + "\\images");
                        if (Properties.Settings.Default.Notify && imagesSize > Properties.Settings.Default.MaxFolderSize)
                            notification += $"\nРазмер папки с обоями превышает рекомендуемый! ({imagesSize} MB)";
                        return (FinishCode.Error, notification);
                    }
                    else // если же первый
                        return (FinishCode.Error, "Отсутствует соединение с интернетом!");
                }
                else // если же нет "постоянного обновления" или есть "только загрузка"
                    return (FinishCode.Error, "Отсутствует соединение с интернетом!");
            }

            if (Properties.Settings.Default.Debug)
                Logs.WriteLogFile("Получение кода...");

            string temp = null;
            try
            {
                temp = Web.GetHtmlCode("https://www.bing.com/");
            }
            catch (Exception e)
            {
                if (Properties.Settings.Default.Debug)
                    Logs.WriteLogFile($"Исключение: {e.Message}\nВнутреннее исключение: {e.InnerException.Message}");
                return (FinishCode.Error, "В ходе работы произошла ошибка!\nПодробности в логе.");
            }

            if (Properties.Settings.Default.Debug)
                Logs.WriteLogFile($"Код получен:\n{temp}\nВыделение ссылки...");

            string url;
            try
            {
                url = temp = SelectUrlFromHtml(temp);
            }
            catch (Exception e)
            {
                if (Properties.Settings.Default.Debug)
                    Logs.WriteLogFile($"Исключение: {e.Message}\nВнутреннее исключение: {e.InnerException.Message}");
                return (FinishCode.Error, "В ходе работы произошла ошибка!\nПодробности в логе.");
            }

            if (Properties.Settings.Default.Debug)
                Logs.WriteLogFile($"Ссылка выделена:\n{temp}");

            temp = SelectNameFromUrl(temp);
            if (Properties.Settings.Default.Name == temp)
            {
                string notification = "Новых обоев пока нет!";
                if (Properties.Settings.Default.AlwaysSet)
                {
                    Wallpaper.SetWallpaper($@"{workDirectory}\images\{temp}");
                    notification += " Были поставлены последние обои.";
                }
                if (Properties.Settings.Default.Debug)
                    Logs.WriteLogFile(notification);

                return (FinishCode.Warning, notification);
            }

            if (Properties.Settings.Default.Debug)
                Logs.WriteLogFile("Загрузка обоев...");

            try
            {
                Web.Download("https://www.bing.com" + url, $@"{workDirectory}\images\{Properties.Settings.Default.Name = temp}");
                
                if (Properties.Settings.Default.Debug)
                    Logs.WriteLogFile($"Обои загружены: {Properties.Settings.Default.Name}");
            }
            catch (Exception e)
            {
                if (Properties.Settings.Default.Debug)
                    if (e.InnerException != null)
                        Logs.WriteLogFile($"Исключение: {e.Message}\nВнутреннее исключение: {e.InnerException.Message}");
                    else
                        Logs.WriteLogFile($"Исключение: {e.Message}");
                return (FinishCode.Error, "В ходе работы произошла ошибка!\nПодробности в логе.");
            }

            Properties.Settings.Default.InstalledWallpaperIndex++; // новые обои попадают в начало - сдвигаем индекс, чтобы он оставался на том же файле в массиве при переключении
            
            // если нужно, ставим обои
            if (!Properties.Settings.Default.OnlyDownload)
            {
                Wallpaper.SetWallpaper($@"{workDirectory}\images\{Properties.Settings.Default.Name}");
                Properties.Settings.Default.InstalledWallpaperIndex = 0;
            }

            // сохраняем новые параметры
            Properties.Settings.Default.Save();

            if (Properties.Settings.Default.Win10Intresting)
            {
                Wallpaper.SaveWin10Interesting(Properties.Settings.Default.Win10IntrestingPath);
                GC.Collect(); // после сохранения WE жрет ОЧЕНЬ много памяти из-за хэширования, но теперь ему столько памяти не нужно
            }

            // если нужно, удаляем старые обои, пока не войдем в указанный предел
            imagesSize = GetDirectorySize(workDirectory + "\\images");
            if (imagesSize > Properties.Settings.Default.MaxFolderSize && Properties.Settings.Default.RemoveOld)
            {
                FileInfo[] files = new DirectoryInfo(workDirectory + "\\images").GetFiles().OrderBy(f => f.LastWriteTime).ToArray();
                MessageBox.Show($"1 - {files[0].CreationTime}; -1 - {files[files.Length-1].CreationTime}");
                while (imagesSize > Properties.Settings.Default.MaxFolderSize)
                {
                    int i = 0;
                    imagesSize -= (int)(files[i].Length / 1048576); // переводим в MB
                    files[i].Delete();
                    i++;
                }
                return (FinishCode.Warning, $"Старые обои были удалены! ({imagesSize} MB)");
            }

            // если нужно, уведомляем
            if (Properties.Settings.Default.Notify)
            {
                
                if (Properties.Settings.Default.Debug)
                    Logs.WriteLogFile($"Вес папки с обоями: {imagesSize} Mb");
                if (imagesSize > Properties.Settings.Default.MaxFolderSize)
                    return (FinishCode.Warning, $"Размер папки с обоями превышает установленную границу! ({imagesSize} MB)");
            }

            return (FinishCode.Good, "");
        }

        /// <summary>
        /// Выделяет имя файла из ссылки на него.
        /// </summary>
        /// <param name="url">URL для выделения имени.</param>
        /// <returns>Имя файла для сохранения.</returns>
        static string SelectNameFromUrl(string url) => url.Substring(7, url.IndexOf(".jpg") - 6 + 3); // +3 из-за расширения

        /// <summary>
        /// Вычисляет размер папки с обоями
        /// </summary>
        /// <param name="folderPath">Папка для вычисления размера</param>
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
