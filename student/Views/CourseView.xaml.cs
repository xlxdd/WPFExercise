using System.Windows.Controls;

namespace student.Views
{
    /// <summary>
    /// CourseView.xaml 的交互逻辑
    /// </summary>
    public partial class CourseView : UserControl
    {
        public CourseView()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            dataGrid.CommitEdit();
        }
    }
}
