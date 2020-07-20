using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wallpapers_Everyday
{
    /// <summary>
    /// Логика взаимодействия для NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public static readonly DependencyProperty valueProperty;
        public int Value
        {
            get { return (int)GetValue(valueProperty); }
            set { SetValue(valueProperty, value); }
        }
        public static readonly DependencyProperty minValueProperty;
        public int MinValue
        {
            get { return (int)GetValue(minValueProperty); }
            set { SetValue(minValueProperty, value); }
        }
        public static readonly DependencyProperty maxValueProperty;
        public int MaxValue
        {
            get { return (int)GetValue(maxValueProperty); }
            set { SetValue(maxValueProperty, value); }
        }
        public static readonly DependencyProperty deltaValueProperty;
        public int DeltaValue
        {
            get { return (int)GetValue(deltaValueProperty); }
            set { SetValue(deltaValueProperty, value); }
        }

        private static object CorrectValue(DependencyObject sender, object value)
        {
            var s = (NumericUpDown)sender;
            int iValue = (int)value;
            if (iValue > s.MaxValue)
                return s.MaxValue;
            if (iValue < s.MinValue)
                return s.MinValue;
            return value;
        }

        public NumericUpDown()
        {
            InitializeComponent();
        }

        static NumericUpDown()
        {
            valueProperty = DependencyProperty.Register("Value",
                typeof(int),
                typeof(NumericUpDown), new FrameworkPropertyMetadata(0, null, new CoerceValueCallback(CorrectValue)));
            minValueProperty = DependencyProperty.Register("MinValue", typeof(int), typeof(NumericUpDown), new PropertyMetadata(0));
            maxValueProperty = DependencyProperty.Register("MaxValue", typeof(int), typeof(NumericUpDown), new PropertyMetadata(100));
            deltaValueProperty = DependencyProperty.Register("DeltaValue", typeof(int), typeof(NumericUpDown), new PropertyMetadata(1));
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e) => Value += DeltaValue;
        private void cmdDown_Click(object sender, RoutedEventArgs e) => Value -= DeltaValue;
    }
}
