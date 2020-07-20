using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Wallpapers_Everyday
{
    public static class WallSetter
    {
        // подключаем и настраиваем API
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SystemParametersInfo(
        int uAction, int uParam, string lpvParam, int fuWinIni);
        public const int SPI_SETDESKWALLPAPER = 20;
        public const int SPIF_UPDATEINIFILE = 0x1;
        public const int SPIF_SENDWININICHANGE = 0x2;

        /// <summary>
        /// Получает код страницы
        /// </summary>
        /// <returns></returns>
        public static void GetHTML()
        {
            // получаем код страницы
            try
            {
                StringBuilder sb = new StringBuilder();
                byte[] buf = new byte[8192];
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.bing.com");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();
                int count = 0;
                do
                {
                    count = resStream.Read(buf, 0, buf.Length);
                    if (count != 0)
                    {
                        sb.Append(Encoding.Default.GetString(buf, 0, count));
                    }
                }
                while (count > 0);
                Vars.HTMLCode = sb.ToString();
            }
            catch(Exception e) { Vars.Debug("Ошибка:\n" + e.Message); }
        }

        /// <summary>
        /// Метод получения ссылки из HTML-кода страницы
        /// </summary>
        /// <returns></returns>
        public static void GetUrlAndName()
        {
            /// Временная переменная для выделения ссылки
            int point;
            /// Временная переменная для записи ссылки
            //null;// = 0ew StringBuilder();

            try
            {
                char[] temp = new char[256];
                // выделяем ссылку из текста через поиск точек
                point = Vars.HTMLCode.IndexOf("link rel=\"preload\" href=\""); // ищем точку начала ссылки
                Vars.HTMLCode = Vars.HTMLCode.Remove(0, point + 25); //удаляем лишний код с учетом точки для поиска
                point = Vars.HTMLCode.IndexOf("\" as=\"image\""); // ищем точку конца ссылки
                Vars.HTMLCode.CopyTo(0, temp, 0, point);
                //for (int i = 0; i < point; i++) //выделяем ссылку из текста
                //    temp.Append(Vars.HTMLCode[i]);

                Vars.Url = new string(temp);
            }
            catch (Exception e) { Vars.Debug($"Ошибка выделения ссылки: {e.Message}"); }
            // подгоняем ссылку на файл с нужным разрешением: 
            //640 х 480
            //800 х 600
            //1024 х 768
            //1920 х 1080
            //1920 х 1200

            Vars.Url = Vars.ReverseString(Vars.Url); // 1. переворачиваем строку

            point = Vars.Url.IndexOf("_"); // 2. ищем место, где пишется разрешение картинки в имени файла
            Vars.Url = Vars.Url.Remove(0, point); // 3. удаляем лишний код с учетом точки для поиска
            Vars.Url = Vars.ReverseString(Vars.Url); // 4. переворачиваем строку

            // 5. теперь добавляем к имени файла нужный вариант разрешения

            float scale = (float)Screen.PrimaryScreen.Bounds.Size.Width / Screen.PrimaryScreen.Bounds.Size.Height;
            scale = (float)((int)(scale * 10)) / 10;

            if (scale == 1.6f) // 16 : 10
                Vars.Url += "1920x1200.jpg";
            else // 16 : 9 (scale = 1.77) и остальные
                Vars.Url += "1920x1080.jpg";

            // выделяем настоящее имя файла из ссылки
            Vars.OriginalName = Vars.Url;
            //MessageBox.Show(Vars.OriginalName);
            // чистим имя с начала строки
            Vars.OriginalName = Vars.OriginalName.Remove(0, 11);
            // и с конца
            Vars.OriginalName = Vars.OriginalName.Remove(Vars.OriginalName.IndexOf(".jpg") + 4, Vars.OriginalName.Length - (Vars.OriginalName.IndexOf(".jpg") + 4));
            //MessageBox.Show(Vars.OriginalName);
        }

        public static void Download()
        {
            // если такая обоина уже есть
            if (File.Exists(Vars.FullExePath + "\\images\\" + Vars.OriginalName))
            {
                if (Properties.Settings.Default.Debug == true)
                    Vars.Debug("Новые обои отсутствуют.");
                // то уведомляем
                Start.tray.BalloonTipText = "Новых обоев пока нет. Попробуйте позже.";
                if (Properties.Settings.Default.AlwaysRun == false)
                    Start.tray.BalloonTipText += "\nЗавершение работы...";
                Start.tray.ShowBalloonTip(3000);
                Thread.Sleep(3000);
                if (Properties.Settings.Default.Debug == true)
                    Vars.Debug("Новых обоев нет.");
                if (Properties.Settings.Default.AlwaysRun == false)
                {
                    Start.tray.Visible = false;
                    Application.Exit();
                }
            }
            else // если же нет
            {
                // качаем новую обоину
                try
                {
                    HttpWebRequest wr = (HttpWebRequest)WebRequest.Create("https://www.bing.com/" + Vars.Url);
                    HttpWebResponse ws = (HttpWebResponse)wr.GetResponse();
                    Stream str = ws.GetResponseStream();

                    byte[] inBuf = new byte[102400];
                    int bytesReadTotal = 0;

                    FileStream fstr = new FileStream(Vars.FullExePath + "\\images\\" + Vars.OriginalName, FileMode.Create, FileAccess.Write);

                    while (true)
                    {
                        int n = str.Read(inBuf, 0, 102400);
                        if ((n == 0) || (n == -1))
                        {
                            break;
                        }

                        fstr.Write(inBuf, 0, n);

                        bytesReadTotal += n;
                    }

                    str.Close();
                    fstr.Close();

                    if (Properties.Settings.Default.Debug == true)
                    {
                        Vars.Debug("Файл загружен: " + Vars.OriginalName);
                    }
                }
                catch(Exception e)
                {
                    // то уведомляем
                    Start.tray.BalloonTipText = "Ошибка загрузки обоев! Попробуйте позже.\nЗавершение работы...";
                    Start.tray.ShowBalloonTip(3000);
                    if (Properties.Settings.Default.Debug == true)
                    {
                        Vars.Debug($"Ошибка загрузки файла:\n{e.Message}\n");
                    }
                    Thread.Sleep(3000);
                    Start.tray.Visible = false;
                    // и закрываем прогу
                    Application.Exit();
                }
            }
        }

        /// <summary>
        /// Устанавливает обои
        /// </summary>
        public static void Set()
        {
            // для конвертации обоев
            Image img = null;

            // конвертируем обои
            try
            {
                img = Image.FromFile(Vars.FullExePath + "\\images\\" + Vars.OriginalName); // добавляем полный путь к программе, иначе в Win 10 не находит файл
            }
            catch
            {
                // ставим новые обои
                if (Properties.Settings.Default.Debug == true)
                    Vars.Debug("Нужных обоев не оказалось. Возможно, файл был удален.");
                Start.tray.BalloonTipText = "Нужный файл не был найден. Возможно, он был удален.\nЗавершение работы...";
                Start.tray.ShowBalloonTip(3000);
                Thread.Sleep(3000);
                Application.Exit();
            }
            
            // обрезаем ".jpg", добавляем ".bmp" и сохраняем в новом формате
            Vars.OriginalName = Vars.OriginalName.Remove(Vars.OriginalName.Length - 4) + ".bmp";
            img.Save(Vars.FullExePath + "\\images\\" + Vars.OriginalName, ImageFormat.Bmp);

            // пишем в дебаг полный путь к программе
            if (Properties.Settings.Default.Debug == true)
                Vars.Debug("Полный путь к exe'шнику: \n" + Vars.FullExePath + "\nРазрешение экрана: " + Screen.PrimaryScreen.Bounds.Width + " x " + Screen.PrimaryScreen.Bounds.Height);

            // ставим новые обои 
            if (Properties.Settings.Default.Debug == true)
                Vars.Debug("Устанавливаю обои...");
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 1, Vars.FullExePath + "\\images\\" + Vars.OriginalName, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

            // пишем имя последнего файла
            Properties.Settings.Default.Name = Vars.OriginalName.Remove(Vars.OriginalName.Length - 4) + ".jpg";

            // удаляем .bmp-копии
            string[] bmp = Directory.GetFiles(Vars.FullExePath + "\\images\\", "*.bmp");
            foreach (string f in bmp)
                try { File.Delete(f); } catch (Exception e) { Vars.Debug($"Не удалось удалить bmp-файл: {e.Message}"); }
        }

        /// <summary>
        /// Функция сохранения заставок экрана блокировки
        /// </summary>
        /// <param name="savePath">Пусть сохранения</param>
        public static void SaveWin10Interesting(string savePath)
        {
            // локальная функция для высчитывания хэша файла. нужна для определения, был файл или нет
            string GetSHA512ofFile(string path)
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    SHA512 sha512 = new SHA512Managed();
                    byte[] fileData = new byte[fs.Length];
                    fs.Read(fileData, 0, (int)fs.Length);
                    byte[] checkSum = sha512.ComputeHash(fileData);
                    return BitConverter.ToString(checkSum).Replace("-", string.Empty);
                }
            }

            Directory.CreateDirectory(savePath);
            var have = Directory.GetFiles(savePath);
            List<string> hashes = new List<string>(have.Length);
            for (int i = 0; i < have.Length; i++)
                hashes.Add(GetSHA512ofFile(have[i]));

            string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets");
            int count = 0;

            int startCount = Directory.GetFiles(savePath).Length;

            for (int i = 0; i < files.Length; i++)
            {
                Bitmap temp = new Bitmap(files[i]);
                Console.Write(temp.Width);
                if (temp.Width > 1080) // если это не мобилкина обоина
                {
                    string sha512 = GetSHA512ofFile(files[i]);
                    if (!hashes.Contains(sha512)) // если файла еще не было
                    {
                        temp.Save($@"WinLogonImages\{startCount + count + 1}.jpg", ImageFormat.Jpeg);
                        //hashes.Add(sha512);
                        count++;
                    }
                }
            }
        }
    }
}
