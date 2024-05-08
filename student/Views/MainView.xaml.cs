using Prism.Events;
using student.Extensions;
using System.Windows;
using System.Windows.Input;

namespace student.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(IEventAggregator aggregator)
        {
            InitializeComponent();
            //注册提示消息
            aggregator.RegisterMessage(arg =>
            {
                SnackBar.MessageQueue.Enqueue(arg);
            });

            //注册等待消息窗口
            aggregator.Register(a =>
            {
                DialogHost.IsOpen = a.IsOpen;
                if (a.IsOpen)
                {
                    DialogHost.DialogContent = new ProgressView();
                }
            });
            menuBar.SelectionChanged += (s, e) =>
            {
                DrawerHost.IsLeftDrawerOpen = false;
            };
            BtnMin.Click += (s, e) =>
            {
                this.WindowState = WindowState.Minimized;
            };
            BtnMax.Click += (s, e) =>
            {
                if (WindowState.Maximized == this.WindowState) WindowState = WindowState.Normal;
                else
                {
                    this.WindowState = WindowState.Maximized;
                }
            };
            BtnClose.Click += (s, e) =>
            {
                this.Close();
            };
            ColorZone.MouseMove += (s, e) =>
            {
                if (MouseButtonState.Pressed == e.LeftButton)
                    this.DragMove();
            };
        }
    }
}
