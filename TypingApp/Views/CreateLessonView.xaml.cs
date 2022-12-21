using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TypingApp.ViewModels;

namespace TypingApp.Views
{
    /// <summary>
    /// Interaction logic for CreateLessonView.xaml
    /// </summary>
    public partial class CreateLessonView : UserControl
    {
        public static ListBox? ListBox;
        public static CreateLessonView? createLessonView;
        public CreateLessonView()
        {
            InitializeComponent();
            ListBox = ListBox1;
            createLessonView = this;
            Loaded += CreateLessonView_Loaded;
        }
        private void CreateLessonView_Loaded(object sender, RoutedEventArgs e)
        {
            CreateLessonViewModel.createLessonViewModel.SelectItems();
        }
    }
}
