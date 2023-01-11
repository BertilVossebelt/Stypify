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
        public static ListBox? ExerciseListBox;
        public static ListBox? GroupListbox;
        public static CreateLessonView? createLessonView;
        public CreateLessonView()
        {
            InitializeComponent();
            ExerciseListBox = ListBox1;
            GroupListbox = ListBox2;
            createLessonView = this;

            //Runs CreateLessonView_Loaded if the view is loaded
            Loaded += CreateLessonView_Loaded;
        }
        
        private void CreateLessonView_Loaded(object sender, RoutedEventArgs e)
        {
            CreateLessonViewModel.createLessonViewModel.SelectItems();  
        }
    }
}
