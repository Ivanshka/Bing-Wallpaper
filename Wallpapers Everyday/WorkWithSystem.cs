using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wallpapers_Everyday
{
    public static class WorkWithSystem
    {
        /// <summary>
        /// Управляет автозагрузкой программы.
        /// </summary>
        /// <param name="mode">Устанавливает значение: "true" - включить автозагрузку, "false" - выключить</param>
        public static void AutorunControl(bool mode, string pathToExe, string name, string keys = null)
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");

            // правим путь к файлу
            pathToExe = pathToExe.Replace("/", "\\");

            if (mode) // если ВКЛЮЧАЕМ автозагрузку
            {
                try
                {
                    // делаем запись в реестр
                    if (keys != null)
                        reg.SetValue(name, pathToExe + " " + keys);
                    else
                        reg.SetValue(name, pathToExe);
                    return;
                }
                catch
                {
                    MessageBox.Show($"Не удалось добавить {name} в автозагрузку!", name, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else // если ВЫКЛЮЧАЕМ автозагрузку
            {
                try
                {
                    reg.DeleteValue(name);
                    return;
                }
                catch { }
            }
        }

        /// <summary>
        /// Вычисляет размер папки с обоями
        /// </summary>
        /// <param name="folderPath">Папка для вычисления размера</param>
        /// <returns>Размер папки в мегабайтах</returns>
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
