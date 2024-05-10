using Prism.Events;
using student.Extensions;
using System.Windows.Controls;

namespace student.Views.Dialogs
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView(IEventAggregator aggregator)
        {
            InitializeComponent();
            //注册提示消息
            aggregator.RegisterMessage(arg =>
            {
                LoginSnakeBar.MessageQueue.Enqueue(arg.Message);
            }, "Login");
        }
    }
}
