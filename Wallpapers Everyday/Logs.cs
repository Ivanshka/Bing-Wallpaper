using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Wallpapers_Everyday
{
    public static class Logs
    {
        public static void WriteLogFile(string logMessage)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(baseDirectory + @"\Logs\"))
            {
                Directory.CreateDirectory(baseDirectory + @"\Logs\");
            }
            string path = baseDirectory + @"\Logs\Log_" + DateTime.UtcNow.ToString("[dd-MM-yyyy]", CultureInfo.InvariantCulture) + ".log";
            StreamWriter writer = File.Exists(path) ? File.AppendText(path) : new StreamWriter(path);
            writer.WriteLine(logMessage);
            writer.Flush();
            writer.Close();
        }
    }
}
