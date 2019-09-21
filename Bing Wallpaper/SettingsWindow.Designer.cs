namespace Bing_Wallpaper
{
    partial class SettingsWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsWindow));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.autostart = new System.Windows.Forms.CheckBox();
            this.pause = new System.Windows.Forms.CheckBox();
            this.notification = new System.Windows.Forms.CheckBox();
            this.num = new System.Windows.Forms.NumericUpDown();
            this.installAlways = new System.Windows.Forms.CheckBox();
            this.noNotifications = new System.Windows.Forms.CheckBox();
            this.dbg = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.about = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Bing_Wallpaper.Properties.Resources.picture;
            this.pictureBox1.Location = new System.Drawing.Point(276, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 128);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // autostart
            // 
            this.autostart.AutoSize = true;
            this.autostart.Location = new System.Drawing.Point(12, 12);
            this.autostart.Name = "autostart";
            this.autostart.Size = new System.Drawing.Size(141, 17);
            this.autostart.TabIndex = 1;
            this.autostart.Text = "Автозапуск с Windows";
            this.autostart.UseVisualStyleBackColor = true;
            // 
            // pause
            // 
            this.pause.AutoSize = true;
            this.pause.Location = new System.Drawing.Point(12, 58);
            this.pause.Name = "pause";
            this.pause.Size = new System.Drawing.Size(262, 17);
            this.pause.TabIndex = 2;
            this.pause.Text = "Только скачивание обоев (высший приоритет)";
            this.pause.UseVisualStyleBackColor = true;
            // 
            // notification
            // 
            this.notification.AutoSize = true;
            this.notification.Location = new System.Drawing.Point(12, 104);
            this.notification.Name = "notification";
            this.notification.Size = new System.Drawing.Size(220, 30);
            this.notification.TabIndex = 3;
            this.notification.Text = "Уведомлять о размере папки с\r\nобоями, если папка весит более (МБ):";
            this.notification.UseVisualStyleBackColor = true;
            // 
            // num
            // 
            this.num.Location = new System.Drawing.Point(230, 110);
            this.num.Name = "num";
            this.num.Size = new System.Drawing.Size(45, 20);
            this.num.TabIndex = 4;
            // 
            // installAlways
            // 
            this.installAlways.AutoSize = true;
            this.installAlways.Location = new System.Drawing.Point(12, 35);
            this.installAlways.Name = "installAlways";
            this.installAlways.Size = new System.Drawing.Size(167, 17);
            this.installAlways.TabIndex = 5;
            this.installAlways.Text = "Всегда устанавливать обои";
            this.installAlways.UseVisualStyleBackColor = true;
            // 
            // noNotifications
            // 
            this.noNotifications.AutoSize = true;
            this.noNotifications.Location = new System.Drawing.Point(12, 81);
            this.noNotifications.Name = "noNotifications";
            this.noNotifications.Size = new System.Drawing.Size(219, 17);
            this.noNotifications.TabIndex = 6;
            this.noNotifications.Text = "Без уведомлений (высший приоритет)";
            this.noNotifications.UseVisualStyleBackColor = true;
            // 
            // dbg
            // 
            this.dbg.AutoSize = true;
            this.dbg.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dbg.Location = new System.Drawing.Point(170, 12);
            this.dbg.Name = "dbg";
            this.dbg.Size = new System.Drawing.Size(105, 30);
            this.dbg.TabIndex = 7;
            this.dbg.Text = "Режим отладки\r\n(запись логов)";
            this.dbg.UseVisualStyleBackColor = true;
            // 
            // about
            // 
            this.about.AutoSize = true;
            this.about.Location = new System.Drawing.Point(303, 131);
            this.about.Name = "about";
            this.about.Size = new System.Drawing.Size(75, 13);
            this.about.TabIndex = 8;
            this.about.TabStop = true;
            this.about.Text = "О программе";
            this.about.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.about_LinkClicked);
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 145);
            this.Controls.Add(this.about);
            this.Controls.Add(this.dbg);
            this.Controls.Add(this.noNotifications);
            this.Controls.Add(this.installAlways);
            this.Controls.Add(this.num);
            this.Controls.Add(this.notification);
            this.Controls.Add(this.pause);
            this.Controls.Add(this.autostart);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки Bing Wallpaper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsWindow_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox autostart;
        private System.Windows.Forms.CheckBox pause;
        private System.Windows.Forms.CheckBox notification;
        private System.Windows.Forms.NumericUpDown num;
        private System.Windows.Forms.CheckBox installAlways;
        private System.Windows.Forms.CheckBox noNotifications;
        private System.Windows.Forms.CheckBox dbg;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.LinkLabel about;
    }
}