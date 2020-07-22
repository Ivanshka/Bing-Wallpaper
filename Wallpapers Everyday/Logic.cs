using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public static string SelectUrlFromHtml(string htmlCode)
        {
            try
            {
                int point;
                char[] temp = new char[256];
                // выделяем ссылку из текста через поиск точек
                point = htmlCode.IndexOf("link rel=\"preload\" href=\""); // ищем точку начала ссылки
                htmlCode = htmlCode.Remove(0, point + 25); //удаляем лишний код с учетом точки для поиска
                point = htmlCode.IndexOf("\" as=\"image\""); // ищем точку конца ссылки
                htmlCode.CopyTo(0, temp, 0, point);

                return new string(temp);
            }
            catch (Exception e) { throw new Exception("Ошибка выделения ссылки!", e); }
        }

        /// <summary>
        /// Выделяет имя файла из ссылки на него.
        /// </summary>
        /// <param name="url">URL для выделения имени.</param>
        /// <returns>Имя файла для сохранения.</returns>
        public static string SelectNameFromUrl(string url) => url.Substring(7, url.IndexOf(".jpg") - 6 + 3); // +3 из-за расширения
    }
}
