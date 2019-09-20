using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bing_Wallpaper
{
    class About
    {
        /// <summary>
        /// Показывает окно "О программе"
        /// </summary>
        public static void Show()
        {
            Form a = new Form();
            a.Text = "О программе Bing Wallpaper";
            a.Size = new Size(500, 220);
            a.StartPosition = FormStartPosition.CenterScreen;
            a.Icon = new Icon(Properties.Resources.icon, new Size(16, 16));
            a.FormBorderStyle = FormBorderStyle.FixedDialog;

            PictureBox pic = new PictureBox();
            pic.Image = Properties.Resources.picture;
            pic.Left = pic.Top = 0;
            pic.Width = pic.Height = 128;

            LinkLabel text = new LinkLabel();
            text.LinkArea = new LinkArea(101, 8);
            text.LinkClicked += text_Click;
            Label text2 = new Label();
            text.Font = text2.Font = new Font("Tahoma", 9);
            text.Width = 360;
            text2.Width = 240;
            text2.Height *= 2 + 10; // *подобрал высоту, не зная базовой
            text2.Left = 5;
            text2.Top = 128;
            text.Height = 130;
            text.Left = 128;
            text.Text = "\nBing Wallpaper\nВерсия: " + Application.ProductVersion + "\nАвтор: © Павлович Иван, 2018 г.\nОбратная связь:\n          ВКонтакте: Ivanshka\n          E-mail: pavlovich.ivan.2000@mail.ru\n\nНадеюсь, что вам понравилась эта небольшая программа. :)";
            text2.Text = "Ключи запуска:\r\n     -a - окно \"О программе\", Вы тут\r\n     -s - настройки";

            a.Controls.Add(pic);
            a.Controls.Add(text);
            a.Controls.Add(text2);
            Application.Run(a);
        }

        private static void text_Click(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process.Start("https://vk.com/ivanshkaa");
        }
    }
}
