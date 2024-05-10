using System.Windows;
using System.Windows.Controls;

namespace student.Views.Dialogs
{
    /// <summary>
    /// Summary.xaml 的交互逻辑
    /// </summary>
    public partial class SummaryView : UserControl
    {
        public SummaryView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pDialog = new PrintDialog();
            var result = pDialog.ShowDialog();

            // If the result is OK then print the document.
            if (result == true)
            {
                pDialog.PrintVisual(ScoreSum, "Scores");
            }
        }
    }
}
