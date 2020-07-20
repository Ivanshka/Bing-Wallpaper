using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using System.Windows.Forms;
namespace Wallpapers_Everyday
{
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();

            if (Properties.Settings.Default.AutoRun) autostart.CheckState = CheckState.Checked;
            if (Properties.Settings.Default.OnlyDown) pause.CheckState = CheckState.Checked;
            if (Properties.Settings.Default.Notify) notification.CheckState = CheckState.Checked;
            if (Properties.Settings.Default.Debug) dbg.CheckState = CheckState.Checked;
            if (Properties.Settings.Default.AlwaysRun) setAlways.CheckState = CheckState.Checked;
            if (Properties.Settings.Default.NoNotify) noNotifications.CheckState = CheckState.Checked;
            num.Value = Properties.Settings.Default.MaxMB;
            if (!GetOsName().Contains("Windows 10")) // если не win 10
            {
                saveWin10Interesting.Enabled = false;
                savePath.Enabled = false;
                viewPath.Enabled = false;
                saveWin10Intresting.Enabled = false;
            } // если win 10 и включено сохранение
            else if (Properties.Settings.Default.Win10Intresting)
            {
                saveWin10Interesting.CheckState = CheckState.Checked;
                Directory.CreateDirectory("Login backgrounds");
            }
            else // если не включено
            {
                savePath.Enabled = false;
                viewPath.Enabled = false;
                saveWin10Intresting.Enabled = false;
            }
            if (Properties.Settings.Default.Win10IntrestingPath == "default")
                Properties.Settings.Default.Win10IntrestingPath = Application.StartupPath + "\\Login backgrounds";

            savePath.Text = Properties.Settings.Default.Win10IntrestingPath;

            string GetOsName()
            {
                var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                            select x.GetPropertyValue("Caption")).FirstOrDefault();
                return name != null ? name.ToString() : "Неизвестно";
            }

            // инициализация подсказок
            new ToolTip().SetToolTip(autostart, "Указывает, будет ли Wallpapers Everyday запускаться вместе с Windows и менять обои.\nПо умолчанию выключен.");
            new ToolTip().SetToolTip(pause, "Приоритет выше, чем у предыдущего параметра, поэтому при\nвключении обоих пунктов обои будут только скачиваться.");
            new ToolTip().SetToolTip(dbg, "Включает логирование работы программы. Используется при тестировании.");
            new ToolTip().SetToolTip(setAlways, "Установка обоев будет происходить независимо от наличия новых обоев\nи подключения к Интернету, что полезно на пиратских копиях Windows.\nПри отстствии Интернета устанавливаться последние скачанные обои.");
            new ToolTip().SetToolTip(noNotifications, "При работе уведомления будут лишь в случае ошибок. Имеет более высокий\nприоритет по сравнению с параметром, отвечающим за уведомление о размере папки.");
            new ToolTip().SetToolTip(saveWin10Interesting, "Только для Windows 10. Если в параметрах системы в качестве заставки экрана блокировки\nстоит \"Windows 10: Интересное\", Wallpaper EveryDay может сохранять эти фоны в удобную для вас папку.");
        }

        /// <summary>
        /// Управляет автозагрузкой программы.
        /// </summary>
        /// <param name="mode">Устанавливает значение: "true" - включить автозагрузку, "false" - выключить</param>
        private static void AutorunControl(bool mode)
        {
            string ExePath = Application.ExecutablePath + " -change";
            RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");

            // правим путь к файлу
            ExePath = ExePath.Replace("/", "\\");

            if (mode) // если ВКЛЮЧАЕМ автозагрузку
            {
                try
                {
                    // делаем запись в реестр
                    reg.SetValue("Wallpapers Everyday", ExePath);
                    return;
                }
                catch
                {
                    MessageBox.Show("Не удалось добавить Wallpapers Everyday в автозагрузку! Автоматическая смена обоев работать не будет.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else // если ВЫКЛЮЧАЕМ автозагрузку
            {
                try
                {
                    reg.DeleteValue("Wallpapers Everyday");
                    return;
                }
                catch { }
            }
        }

        /// <summary>
        /// Сохраняет настройки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (autostart.Checked)
            {
                Properties.Settings.Default.AutoRun = true;
                AutorunControl(true);
            }
            else
            {
                Properties.Settings.Default.AutoRun = false;
                AutorunControl(false);
            }

            if (pause.Checked)
                Properties.Settings.Default.OnlyDown = true;
            else
                Properties.Settings.Default.OnlyDown = false;

            if (notification.Checked)
                Properties.Settings.Default.Notify = true;
            else
                Properties.Settings.Default.Notify = false;

            Properties.Settings.Default.MaxMB = (int)num.Value;

            if (dbg.Checked)
                Properties.Settings.Default.Debug = true;
            else
                Properties.Settings.Default.Debug = false;

            if (setAlways.Checked)
                Properties.Settings.Default.AlwaysRun = true;
            else
                Properties.Settings.Default.AlwaysRun = false;

            if (noNotifications.Checked)
                Properties.Settings.Default.NoNotify = true;
            else
                Properties.Settings.Default.NoNotify = false;

            if (saveWin10Interesting.Checked)
                Properties.Settings.Default.Win10Intresting = true;
            else
                Properties.Settings.Default.Win10Intresting = false;

            Properties.Settings.Default.Win10IntrestingPath = savePath.Text;

            try
            {
                Properties.Settings.Default.Save();
                MessageBox.Show("Настройки успешно сохранены!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить настройки!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void aboutButton_Click(object sender, System.EventArgs e) => new AboutBox().Show();

        private void viewPath_Click(object sender, System.EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.RootFolder = System.Environment.SpecialFolder.Desktop;
            if (dialog.ShowDialog() == DialogResult.OK)
                savePath.Text = dialog.SelectedPath;
        }

        private void saveWin10Interesting_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!saveWin10Interesting.Checked)
            {
                savePath.Enabled = false;
                viewPath.Enabled = false;
                saveWin10Intresting.Enabled = false;
            }
            else
            {
                savePath.Enabled = true;
                viewPath.Enabled = true;
                saveWin10Intresting.Enabled = true;
            }
        }

        private void saveWin10Intresting_Click(object sender, System.EventArgs e) => new Thread(() => WallSetter.SaveWin10Interesting(Properties.Settings.Default.Win10IntrestingPath));
    }
}
