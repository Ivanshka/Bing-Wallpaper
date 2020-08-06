using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wallpapers_Everyday
{
    public class Commands
    {
        public static RoutedCommand NextWallpaper { get; set; }
        public static RoutedCommand PreviousWallpaper { get; set; }
        public static RoutedCommand SaveWinLogon { get; set; }
        public static RoutedCommand UpdateWallpaper { get; set; }
        public static RoutedCommand ShowAboutBox { get; set; }

        static Commands()
        {
            NextWallpaper = new RoutedCommand("NextWallpaper", typeof(MainWindow));
            PreviousWallpaper = new RoutedCommand("PreviousWallpaper", typeof(MainWindow));
            SaveWinLogon = new RoutedCommand("SaveWinLogon", typeof(MainWindow));
            UpdateWallpaper = new RoutedCommand("UpdateWallpaper", typeof(MainWindow));
            ShowAboutBox = new RoutedCommand("ShowAboutBox", typeof(MainWindow));
        }
    }
}
