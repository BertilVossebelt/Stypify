using System.Windows.Controls;

namespace TypingApp.Views;

public partial class StudentDashboardView : UserControl
{
    public StudentDashboardView()
    {
        InitializeComponent();
    }


    private void GoRight(object sender, System.Windows.RoutedEventArgs e)
    {
        ScrollViewer.PageRight();
    }

    private void GoLeft(object sender, System.Windows.RoutedEventArgs e)
    {
        ScrollViewer.PageLeft();
    }

}

