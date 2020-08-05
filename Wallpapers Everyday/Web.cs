using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace Wallpapers_Everyday
{
    public static class Web
    {
        /// <summary>
        /// Проверяет наличие соединения с интернетом
        /// </summary>
        public static bool CheckConnection()
        {
            try
            {
                Ping ping = new Ping();
                PingReply pingReply = ping.Send("google.com");
                if (pingReply.Status == IPStatus.Success)
                    return true;
                else
                    return false;
            }
            catch { return false; }
        }

        /// <summary>
        /// Производит запрос и возвращает HTML-код указанной страницы.
        /// </summary>
        /// <returns>HTML-код указанной страницы</returns>
        /// <exception cref="Exception"/>
        public static string GetHtmlCode(string url)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                byte[] buf = new byte[8192];
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
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
                return sb.ToString();
            }
            catch (Exception e) { throw new Exception("Не удалось получить код страницы! Подробности во внутреннем исключении.", e); }
        }

        /// <summary>
        /// Загружает файл по указанному URL и сохраняет по пути <paramref name="savePath"/>
        /// </summary>
        /// <param name="url">Адрес URL для загрузки файла</param>
        /// <param name="savePath">Путь сохранения файла</param>
        /// <exception cref="Exception"/>
        public static void Download(string url, string savePath)
        {
            try
            {
                byte[] inBuf = new byte[102400];
                int bytesReadTotal = 0;
                
                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse ws = (HttpWebResponse)wr.GetResponse();

                Stream str = ws.GetResponseStream();
                FileStream fstr = new FileStream(savePath, FileMode.Create, FileAccess.Write);

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
            }
            catch (Exception e) { throw new Exception("Не удалось загрузить файл! Подробности во внутреннем исключении.", e); }
        }
    }
}
