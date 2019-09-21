using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Bing_Wallpaper
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
            if (Properties.Settings.Default.AlwaysRun) installAlways.CheckState = CheckState.Checked;
            num.Value = Properties.Settings.Default.MaxMB;

            // инициализация подсказки к чекбоксу автозагрузки
            ToolTip tAutostart = new ToolTip();
            tAutostart.SetToolTip(autostart, "Указывает, будет ли Bing Wallpaper запускаться вместе с Windows.\nПо умолчанию выключен.");

            // инициализация подсказки к чекбоксу паузы
            ToolTip tPause = new ToolTip();
            tPause.SetToolTip(pause, "Приоритет выше, чем у предыдущего параметра, поэтому при\nвключении обоих пунктов обои будут только скачиваться.");

            // инициализация подсказки к чекбоксу дебага
            ToolTip tDebug = new ToolTip();
            tDebug.SetToolTip(dbg, "Включает логирование работы программы. Используется при тестировании.");
            
            // инициализация подсказки к чекбоксу пирата
            ToolTip tInstallAlways = new ToolTip();
            tInstallAlways.SetToolTip(installAlways, "Установка обоев будет происходить независимо от наличия новых обоев\nи подключения к Интернету, что полезно на пиратских копиях Windows.\nПри отстствии Интернета устанавливаться последние скачанные обои.");
        }
        
        /// <summary>
        /// Управляет автозагрузкой программы.
        /// </summary>
        /// <param name="mode">Устанавливает значение: "true" - включить автозагрузку, "false" - выключить</param>
        private static void AutorunControl(bool mode)
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

        /// <summary>
        /// Сохраняет настройки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
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
        }

        private void about_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.Show();
        }
    }
}
