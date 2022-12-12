using System.Windows;
using System.Windows.Controls;

namespace TypingApp.Views
{
    /// <summary>
    /// Interaction logic for GroupView.xaml
    /// </summary>
    public partial class TeacherDashboardView : UserControl
    {
        public TeacherDashboardView()
        {
            InitializeComponent();
        }

        private void GoRight(object sender, RoutedEventArgs e)
        {
            ScrollViewer.PageRight();
        }

        private void GoLeft(object sender, RoutedEventArgs e)
        {
            ScrollViewer.PageLeft();
        }
    }
}
