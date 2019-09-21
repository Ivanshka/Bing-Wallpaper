using Microsoft.Win32;
using System.Drawing;
using System.Windows.Forms;

namespace Bing_Wallpaper
{
    public class Settings
    {
        // окно и его части
        static Form s = new Form(); // окно
        static PictureBox logo = new PictureBox(); // пикча
        static CheckBox autostart = new CheckBox(); // autorun
        static CheckBox pause = new CheckBox(); // only load
        static CheckBox notification = new CheckBox(); // уведомлялка о весе папки
        static NumericUpDown num = new NumericUpDown(); // кол-во мб
        static CheckBox dbg = new CheckBox(); // режим debug'а
        static CheckBox installAlways = new CheckBox(); // всегда обновлять обои
        static CheckBox noNotifications = new CheckBox(); // режим "без уведомлений"

        /// <summary>
        /// Сохраняет настройки
        /// </summary>
        public static void Save()
        {
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Управляет автозагрузкой программы.
        /// </summary>
        /// <param name="mode">Устанавливает значение: "true" - включить автозагрузку, "false" - выключить</param>
        public static void AutorunControl(bool mode)
        {
            string ExePath = Application.ExecutablePath;
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");

            // правим путь к файлу
            ExePath = ExePath.Replace("/", "\\");

            if (mode) // если ВКЛЮЧАЕМ автозагрузку
            {
                try
                {
                    // делаем запись в реестр
                    reg.SetValue("Bing Wallpaper", ExePath);
                    return;
                }
                catch
                {
                    MessageBox.Show("Не удалось добавить Bing Wallpaper в автозагрузку! Автоматическая смена обоев работать не будет.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else // если ВЫКЛЮЧАЕМ автозагрузку
            {
                try
                {
                    reg.DeleteValue("Bing Wallpaper");
                    return;
                }
                catch { }
            }
        }
        
        //Метод закрытия формы настроек. Сохраняет параметры.
        public static void CloseSettings()
        {
            if (autostart.CheckState == CheckState.Checked)
            {
                Properties.Settings.Default.AutoRun = true;
                AutorunControl(true);
            }
            else
            {
                Properties.Settings.Default.AutoRun = false;
                AutorunControl(false);
            }

            if (pause.CheckState == CheckState.Checked)
            {
                Properties.Settings.Default.OnlyDown = true;
            }
            else
            {
                Properties.Settings.Default.OnlyDown = false;
            }

            if (notification.CheckState == CheckState.Checked)
            {
                Properties.Settings.Default.Notify = true;
            }
            else
            {
                Properties.Settings.Default.Notify = false;
            }

            Properties.Settings.Default.MaxMB = (int)num.Value;

            if (dbg.CheckState == CheckState.Checked)
            {
                Properties.Settings.Default.Debug = true;
            }
            else
            {
                Properties.Settings.Default.Debug = false;
            }

            if (installAlways.CheckState == CheckState.Checked)
            {
                Properties.Settings.Default.AlwaysRun = true;
            }
            else
            {
                Properties.Settings.Default.AlwaysRun = false;
            }

            try
            {
                Properties.Settings.Default.Save();
                MessageBox.Show("Настройки успешно сохранены!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить настройки! Убедитесь, что достаточно прав на запись в папку.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            return;
        }



        /// <summary>
        /// Отображает форму настроек
        /// </summary>
        public static void Show()
        {
            Start.tray.BalloonTipTitle = "Настройка...";

            // Инициализация окна
            s.Text = "Настройки Bing Wallpaper";
            s.Size = new Size(420, 158);
            s.StartPosition = FormStartPosition.CenterScreen;
            s.Icon = new Icon(Properties.Resources.icon, new Size(16, 16));
            s.FormBorderStyle = FormBorderStyle.FixedDialog;

            // Инициализация лого
            logo.Image = Properties.Resources.picture;
            logo.Width = logo.Height = 128;
            logo.Left = s.Width - 138;
            logo.Top = 0;

            // Инициализация чекбокса автозапуска
            autostart.Text = "Запускать при старте Windows";
            autostart.Top = 5;
            if (Properties.Settings.Default.AutoRun) autostart.CheckState = CheckState.Checked;

            // инициализация подсказки к чекбоксу автозагрузки
            ToolTip tAutostart = new ToolTip();
            tAutostart.SetToolTip(autostart, "Указывает, будет ли Bing Wallpaper запускаться вместе с Windows.\nПо умолчанию выключен.");

            // Инициализация чекбокса паузы
            pause.Text = "Приостановить смену обоев (только загрузка)";
            pause.Top = 25;
            if (Properties.Settings.Default.OnlyDown) pause.CheckState = CheckState.Checked;

            // инициализация подсказки к чекбоксу паузы
            ToolTip tPause = new ToolTip();
            tPause.SetToolTip(pause, "Приоритет выше, чем у \"режима пирата\", поэтому при включении обоих\nпунктов обои будут только скачиваться.");

            // Инициализация чекбокса предупреждения
            notification.Text = "Уведомлять о размере папки с обоями," + "\r\n" + "если папка весит более (МБ):";
            notification.Top = 45;
            notification.Height = 30;
            if (Properties.Settings.Default.Notify) notification.CheckState = CheckState.Checked;

            // Инициализация "ограничителя"
            num.BorderStyle = BorderStyle.Fixed3D;
            num.Value = Properties.Settings.Default.MaxMB;
            num.Width = 40;
            num.Top = 50;
            num.Left = 243;

            autostart.Left = pause.Left = notification.Left = dbg.Left = installAlways.Left = 10;
            autostart.Width = pause.Width = dbg.Width = installAlways.Width = 300;
            notification.Width = 233;

            // Инициализация чекбокса дебага
            dbg.Text = "Режим отладки - запись логов программы";
            dbg.Top = 70;
            if (Properties.Settings.Default.Debug) dbg.CheckState = CheckState.Checked;

            // инициализация подсказки к чекбоксу дебага
            ToolTip tDebug = new ToolTip();
            tDebug.SetToolTip(dbg, "Включает логирование работы программы. Используется при тестировании.");

            // Инициализация чекбокса пирата
            installAlways.Top = 90;
            installAlways.Height = 30;
            installAlways.Text = "\"Пиратский режим\" (всегда обновлять обои)";
            if (Properties.Settings.Default.AlwaysRun) installAlways.CheckState = CheckState.Checked;
            
            // инициализация подсказки к чекбоксу пирата
            ToolTip tPirat = new ToolTip();
            tPirat.SetToolTip(installAlways, "Установка обоев будет происходить независимо от наличия новых обоев\nи подключения к Интернету, что полезно на пиратских копиях Windows.\nПри отстствии Интернета устанавливаться последние скачанные обои.");

            s.Controls.Add(logo);
            s.Controls.Add(autostart);
            s.Controls.Add(pause);
            s.Controls.Add(notification);
            s.Controls.Add(num);
            s.Controls.Add(dbg);
            s.Controls.Add(installAlways);
            Application.Run(s);
            CloseSettings();
        }
    }
}
