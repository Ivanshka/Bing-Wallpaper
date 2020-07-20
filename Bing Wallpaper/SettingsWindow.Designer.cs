namespace Wallpapers_Everyday
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
            this.setAlways = new System.Windows.Forms.CheckBox();
            this.noNotifications = new System.Windows.Forms.CheckBox();
            this.dbg = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.saveWin10Interesting = new System.Windows.Forms.CheckBox();
            this.aboutButton = new System.Windows.Forms.Button();
            this.viewPath = new System.Windows.Forms.Button();
            this.savePath = new System.Windows.Forms.TextBox();
            this.saveWin10Intresting = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Wallpapers_Everyday.Properties.Resources.picture;
            this.pictureBox1.Location = new System.Drawing.Point(284, 0);
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
            // setAlways
            // 
            this.setAlways.AutoSize = true;
            this.setAlways.Location = new System.Drawing.Point(12, 35);
            this.setAlways.Name = "setAlways";
            this.setAlways.Size = new System.Drawing.Size(167, 17);
            this.setAlways.TabIndex = 5;
            this.setAlways.Text = "Всегда устанавливать обои";
            this.setAlways.UseVisualStyleBackColor = true;
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
            this.dbg.Location = new System.Drawing.Point(173, 12);
            this.dbg.Name = "dbg";
            this.dbg.Size = new System.Drawing.Size(105, 30);
            this.dbg.TabIndex = 7;
            this.dbg.Text = "Режим отладки\r\n(логирование)";
            this.dbg.UseVisualStyleBackColor = true;
            // 
            // saveWin10Interesting
            // 
            this.saveWin10Interesting.AutoSize = true;
            this.saveWin10Interesting.Location = new System.Drawing.Point(12, 140);
            this.saveWin10Interesting.Name = "saveWin10Interesting";
            this.saveWin10Interesting.Size = new System.Drawing.Size(256, 30);
            this.saveWin10Interesting.TabIndex = 9;
            this.saveWin10Interesting.Text = "Сохранять заставки \"Windows: Интересное\"\r\nв отдельную папку. (Только для Windows " +
    "10.)";
            this.saveWin10Interesting.UseVisualStyleBackColor = true;
            this.saveWin10Interesting.CheckedChanged += new System.EventHandler(this.saveWin10Interesting_CheckedChanged);
            // 
            // aboutButton
            // 
            this.aboutButton.Location = new System.Drawing.Point(305, 140);
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(86, 23);
            this.aboutButton.TabIndex = 10;
            this.aboutButton.Text = "О программе";
            this.aboutButton.UseVisualStyleBackColor = true;
            this.aboutButton.Click += new System.EventHandler(this.aboutButton_Click);
            // 
            // viewPath
            // 
            this.viewPath.Location = new System.Drawing.Point(327, 174);
            this.viewPath.Name = "viewPath";
            this.viewPath.Size = new System.Drawing.Size(75, 23);
            this.viewPath.TabIndex = 11;
            this.viewPath.Text = "Обзор";
            this.viewPath.UseVisualStyleBackColor = true;
            this.viewPath.Click += new System.EventHandler(this.viewPath_Click);
            // 
            // savePath
            // 
            this.savePath.Location = new System.Drawing.Point(12, 176);
            this.savePath.Name = "savePath";
            this.savePath.Size = new System.Drawing.Size(309, 20);
            this.savePath.TabIndex = 12;
            // 
            // saveWin10Intresting
            // 
            this.saveWin10Intresting.Location = new System.Drawing.Point(12, 202);
            this.saveWin10Intresting.Name = "saveWin10Intresting";
            this.saveWin10Intresting.Size = new System.Drawing.Size(244, 23);
            this.saveWin10Intresting.TabIndex = 14;
            this.saveWin10Intresting.Text = "Сохранить заставки Windows: Интересное";
            this.saveWin10Intresting.UseVisualStyleBackColor = true;
            this.saveWin10Intresting.Click += new System.EventHandler(this.saveWin10Intresting_Click);
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 235);
            this.Controls.Add(this.saveWin10Intresting);
            this.Controls.Add(this.savePath);
            this.Controls.Add(this.viewPath);
            this.Controls.Add(this.aboutButton);
            this.Controls.Add(this.saveWin10Interesting);
            this.Controls.Add(this.dbg);
            this.Controls.Add(this.noNotifications);
            this.Controls.Add(this.setAlways);
            this.Controls.Add(this.num);
            this.Controls.Add(this.notification);
            this.Controls.Add(this.pause);
            this.Controls.Add(this.autostart);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки Wallpapers Everyday";
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
        private System.Windows.Forms.CheckBox setAlways;
        private System.Windows.Forms.CheckBox noNotifications;
        private System.Windows.Forms.CheckBox dbg;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox saveWin10Interesting;
        private System.Windows.Forms.Button aboutButton;
        private System.Windows.Forms.Button viewPath;
        private System.Windows.Forms.TextBox savePath;
        private System.Windows.Forms.Button saveWin10Intresting;
    }
}