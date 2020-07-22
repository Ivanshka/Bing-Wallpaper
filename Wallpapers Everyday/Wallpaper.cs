using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Wallpapers_Everyday
{
    public static class Wallpaper
    {
        // подключаем и настраиваем API
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int SystemParametersInfo(
        int uAction, int uParam, string lpvParam, int fuWinIni);
        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x1;
        const int SPIF_SENDWININICHANGE = 0x2;

        /// <summary>
        /// Устанавливает растровое изображение на фон рабочего стола с ограничениями поддерживаемых форматов.
        /// Проверялись только BMP, PNG и JPG.
        /// WinXP: bmp;
        /// Win7: bmp, jpg;
        /// Win10: bmp, png, jpg.
        /// </summary>
        /// <param name="fullpath">Полный путь к файлу</param>
        public static void SetWallpaper(string fullpath) => SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, fullpath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

        /// <summary>
        /// Сохраняет заставки экрана блокировки. Если картинка имеет высоту > 1080, то она считается заставкой.
        /// P.S.
        /// </summary>
        /// <param name="savePath">Путь к папке сохранения</param>
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
                        temp.Save($@"{savePath}\{startCount + count + 1}.jpg", ImageFormat.Jpeg);
                        //hashes.Add(sha512);
                        count++;
                    }
                }
            }
        }
    }
}
