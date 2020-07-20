using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wallpapers_Everyday
{
    /// <summary>
    /// Логика взаимодействия для DirectoryReviewer.xaml
    /// </summary>
    public partial class DirectoryReviewer : System.Windows.Controls.UserControl
    {
        public static readonly DependencyProperty directoryPathProperty;
        public string DirectoryPath
        {
            get { return (string)GetValue(directoryPathProperty); }
            set { SetValue(directoryPathProperty, value); }
        }
        public static readonly DependencyProperty descriptionProperty;
        public string Description
        {
            get { return (string)GetValue(descriptionProperty); }
            set { SetValue(descriptionProperty, value); }
        }
        public static readonly DependencyProperty rootFolderProperty;
        public Environment.SpecialFolder RootFolder
        {
            get { return (Environment.SpecialFolder)GetValue(rootFolderProperty); }
            set { SetValue(rootFolderProperty, value); }
        }

        static DirectoryReviewer()
        {
            directoryPathProperty = DependencyProperty.Register("DirectoryPath", typeof(string), typeof(DirectoryReviewer), new PropertyMetadata(String.Empty));
            descriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(NumericUpDown), new PropertyMetadata(String.Empty));
            rootFolderProperty = DependencyProperty.Register("RootFolder", typeof(Environment.SpecialFolder), typeof(DirectoryReviewer), new PropertyMetadata(Environment.SpecialFolder.Desktop));
        }

        public DirectoryReviewer()
        {
            InitializeComponent();
        }

        private void Review_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = Description;
                dialog.ShowNewFolderButton = true;
                dialog.RootFolder = RootFolder;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    DirectoryPath = dialog.SelectedPath;
                }
            }
        }
    }
}
