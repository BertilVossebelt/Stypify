using System.Windows.Controls;

namespace TypingApp.Views;

public partial class MyLessonsView : UserControl
{
    public MyLessonsView()
    {
        InitializeComponent();
    }

    private void GoRight(object sender, System.Windows.RoutedEventArgs e)
    {
        Scrollviewer.PageRight();
    }

    private void GoLeft(object sender, System.Windows.RoutedEventArgs e)
    {
        Scrollviewer.PageLeft();
    }

    private void Button_Click()
    {

    }
}