namespace Feedback
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.topic = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.sms = new System.Windows.Forms.TextBox();
            this.send = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.CheckBox();
            this.output = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(162, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Тема";
            // 
            // topic
            // 
            this.topic.Location = new System.Drawing.Point(12, 29);
            this.topic.Name = "topic";
            this.topic.Size = new System.Drawing.Size(324, 20);
            this.topic.TabIndex = 1;
            this.topic.Text = "Тема сообщения: пожелание, преложение и т.д.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(147, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Сообщение";
            // 
            // sms
            // 
            this.sms.Location = new System.Drawing.Point(12, 68);
            this.sms.Multiline = true;
            this.sms.Name = "sms";
            this.sms.Size = new System.Drawing.Size(324, 160);
            this.sms.TabIndex = 1;
            this.sms.Text = "Тут введите ваше пожаление, предложение или жалобу. Поддерживаются HTML-теги. Мож" +
    "ете также указать e-mail для связи.";
            // 
            // send
            // 
            this.send.Location = new System.Drawing.Point(261, 234);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(75, 23);
            this.send.TabIndex = 2;
            this.send.Text = "Отправить";
            this.send.UseVisualStyleBackColor = true;
            this.send.Click += new System.EventHandler(this.send_Click);
            // 
            // logs
            // 
            this.logs.AutoSize = true;
            this.logs.Location = new System.Drawing.Point(12, 238);
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(175, 17);
            this.logs.TabIndex = 3;
            this.logs.Text = "Прикрепить логи программы";
            this.logs.UseVisualStyleBackColor = true;
            // 
            // output
            // 
            this.output.AutoSize = true;
            this.output.Location = new System.Drawing.Point(12, 258);
            this.output.Name = "output";
            this.output.Size = new System.Drawing.Size(0, 13);
            this.output.TabIndex = 4;
            // 
            // Form1
            // 
            this.AcceptButton = this.send;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 282);
            this.Controls.Add(this.output);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.send);
            this.Controls.Add(this.sms);
            this.Controls.Add(this.topic);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bing Wallpaper: обратная связь";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox topic;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox sms;
        private System.Windows.Forms.Button send;
        private System.Windows.Forms.CheckBox logs;
        private System.Windows.Forms.Label output;
    }
}

