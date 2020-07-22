using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Wallpapers_Everyday
{
    public static class Web
    {
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
            catch (Exception e) { throw new Exception("Не удалось получить код страницы!", e); }
        }
    }
}
