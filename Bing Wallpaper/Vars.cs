using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Bing_Wallpaper
{
    /// <summary>
    /// Класс переменных, констант и побочных методов
    /// </summary>
    class Vars
    {
        // settings
        /// <summary>
        /// Ссылка на обои
        /// </summary>
        public static string Url;

        /// <summary>
        /// HTML-код страницы bing.com
        /// </summary>
        public static string HTMLCode;

        /// <summary>
        /// Настоящее имя файла
        /// </summary>
        public static string WEBFILENAME;

        /// <summary>
        /// Время записи лога. Переменная нужна для беспрерывной записи инфы в файл при смене времени в ходе работы программы
        /// </summary>
        static string LogTime;

        /// <summary>
        /// Полный путь к exe'шнику программы
        /// </summary>
        public static string FullExePath;
        
        /// <summary>
        /// Подготавливает программу к записи логов: 
        /// </summary>
        public static void DebugInit()
        {
            if (!Directory.Exists(FullExePath + "\\logs\\"))
            {
                Directory.CreateDirectory(FullExePath + "\\logs");
            }

            LogTime = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;

            Debug("Запуск: " + "[" + LogTime + "] [" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "]");
        }

        /// <summary>
        /// Метод записи логов
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            StreamWriter sr = new StreamWriter(FullExePath + "\\logs\\log_[" + LogTime + "].log", true);
            sr.WriteLine(message);
            sr.Close();
        }

        /// <summary>
        /// Размер папки с обоями
        /// </summary>
        public static int FolderSize;

        /// <summary>
        /// Возвращает объем папки с обоями
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static int GetDirectorySize(string folderPath)
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
