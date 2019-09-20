using Ionic.Zip;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Feedback
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void send_Click(object sender, EventArgs e)
        {
            if (topic.Text.Length == 0)
            {
                MessageBox.Show("Требуется заполнить поле темы!", "Отправка невозможна!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            if (sms.TextLength == 0 && !logs.Checked)
            {
                MessageBox.Show("Требуется заполнить поле сообщения или хотя бы отправить логи!", "Отправка невозможна!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            
            // проверка наличия интернета
            using (var tcpClient = new TcpClient())
            {
                try
                {
                    output.ForeColor = Color.Blue;
                    output.Text = "Проверка подключения к интернету...";
                    tcpClient.Connect("8.8.8.8", 443); // google, еще вариант 209.85.148.138
                }
                catch
                {
                    output.ForeColor = Color.Red;
                    output.Text = "Ошибка! Проверьте соединение с интернетом!";
                }
            }

            output.Text = "Подготовка...";

            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("pavlovich.ivan.game.2000@mail.ru", "Logs of Bing Wallpaper");
            // кому отправляем
            MailAddress to = new MailAddress("pavlovich.ivan.2000@mail.ru");
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            if (topic.TextLength == 0)
                m.Subject = "Отчет Bing Wallpaper " + Environment.MachineName + ", " + DateTime.Now;
            else
                m.Subject = topic.Text;
            // текст письма
            if (topic.TextLength == 0)
                m.Body = "<h2>" + Environment.MachineName + ", " + DateTime.Now + "</h2>";
            else
                m.Body = sms.Text;
            
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 2525);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("pavlovich.ivan.game.2000@mail.ru", "giveme1000igr");
            smtp.EnableSsl = true;

            if (logs.Checked)
            {
                if (File.Exists("logs.zip"))
                    File.Delete("logs.zip");
                if (!Directory.Exists("logs"))
                {
                    output.ForeColor = Color.Red;
                    output.Text = "Отчетов нет! Можно отправить лишь собщение!";
                    return;
                }

                ZipFile zf = new ZipFile("logs.zip");
                zf.AddDirectory("logs");
                try
                {
                    zf.Save();
                }
                catch (Exception exc)
                {
                    output.ForeColor = Color.Red;
                    output.Text = "Не удалось сохранить архив с логами! Можно отправить лишь сообщение!";
                    return;
                }
                
                // прикрепляем отчет
                m.Attachments.Add(new Attachment("logs.zip"));
            }


            output.Text = "Отправка...";
            try
            {
                smtp.Send(m);
            }
            catch (Exception exc)
            {
                output.ForeColor = Color.Red;
                output.Text = "Ошибка! Не удалось отправить сообщение!";
                MessageBox.Show("Не удалось отправить сообщение!\n" + exc.Message + "\nПопробуйте отправлять меньше логов, если они были прикреплены.", "Ошибка!");
            }
        }
    }
}