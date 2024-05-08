using System.Windows.Controls;

namespace student.Views
{
    /// <summary>
    /// ScoreView.xaml 的交互逻辑
    /// </summary>
    public partial class ScoreView : UserControl
    {
        public ScoreView()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            dataGrid.CommitEdit();
        }
    }
}
