using mailscreenshotl;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace mailscreenshot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Threading.Timer threadTimer;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            int interval;
            if (!int.TryParse(Interval.Text, out interval))
            {
                Message.Text += "非法输入！\n";
                return;
            };
            Message.Text += "开始发送！\n";
            while (true)
            {
                try
                {
                    EmailClient.SendEmail(Rec.Text, Theme.Text, Content.Text);
                    Message.Text += "发送成功！\n";
                }
                catch (Exception)
                {
                    Message.Text += "发送失败！\n";
                }
                await Task.Delay(interval * 1000);
            }
        }
    }
}
