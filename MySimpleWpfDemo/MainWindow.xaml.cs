using System.Windows;
using System.Windows.Controls;

namespace MySimpleWpfDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 这个方法对应 XAML 中按钮的 Click 事件
        private void GreetButton_Click(object sender, RoutedEventArgs e)
        {
            // 通过 x:Name 访问 XAML 中的控件
            // 和在 WinForms 中访问 this.textBox1.Text 类似
            if (!string.IsNullOrEmpty(NameTextBox.Text))
            {
                // 更新 TextBlock 的 Text 属性
                GreetingTextBlock.Text = $"你好, {NameTextBox.Text}!";
            }
            else
            {
                GreetingTextBlock.Text = "你好, 世界!";
            }
        }
    }
}