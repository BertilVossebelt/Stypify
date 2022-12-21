using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NavigationService = TypingApp.Services.NavigationService;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Stores;
using TypingApp.Commands;
using System.Windows.Input;
using TypingApp.Views;


namespace TypingApp.ViewModels
{
    public class CreateLessonViewModel : ViewModelBase
    {
        public static CreateLessonViewModel createLessonViewModel;
        private ObservableCollection<Exercise>? _exercises;
        private readonly UserStore _userStore;
        private ObservableCollection<Exercise>? _selectedExercises;
        public int test = 0;
        public static bool ready
        {
            get => ready;
            set
            {
                if (value == null) return;
                ready = value;
            }
        }

        public ICommand Test { get; set; }

        public ObservableCollection<Exercise>? Exercises
        {
            get => _exercises;
            set
            {
                if (value == null) return;
                _exercises = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Exercise>? SelectedExercises
        {
            get => _selectedExercises;
            set
            {
                _selectedExercises = value;
                //Console.WriteLine(CreateLessonView.ListBox.SelectedItems);
                
                OnPropertyChanged();
            }
        }
        // IN exercise model een bool toevoegen en dan foreach exercise en kijken of hij true of false is.

        public CreateLessonViewModel(UserStore userStore)
        {
            _userStore = userStore;
            Exercises = new ObservableCollection<Exercise>();
            createLessonViewModel = this;



            Console.WriteLine("tst");
            Test = new Class1(this);
            test = 20;
            // Populate Exercises
            if (userStore.Teacher == null)
            {
                Console.WriteLine("failed");
                return;
            }
            Console.WriteLine(userStore.Teacher.Id);
            var exercises = new ExerciseProvider().GetAll(userStore.Teacher.Id);
            if (exercises != null)
            {
                foreach (var exercise in exercises)
                {
                    if(exercise != null)
                    {
                        Exercise ex = new Exercise((string)exercise["text"], (string)exercise["name"]);
                        Exercises?.Add(ex);
                        //
                        
                    }
                    
                }
            }
            Console.WriteLine(CreateLessonView.ListBox);

            //CreateLessonView.ListBox.SelectedItems.Add(CreateLessonView.ListBox.Items.GetItemAt(0));
            //exercises?.ForEach(e => Exercises?.Add(new Exercise((string)e["text"], (string)e["name"])));

            //Exercises = new ObservableCollection<Exercise>();

            /* Exercises.Add(new Exercise("Test", "TestTeacher"));
             Exercises.Add(new Exercise("Test", "TestTeacher"));
             Exercises.Add(new Exercise("Test", "TestTeacher"));
             Exercises.Add(new Exercise("Test", "TestTeacher"));
             Exercises.Add(new Exercise("Test", "TestTeacher"));
             Exercises.Add(new Exercise("Test", "TestTeacher"));
             Exercises.Add(new Exercise("Test", "TestTeacher"));*/
        }
        
        public void SelectItems()
        {
            CreateLessonView.ListBox.SelectedItems.Add(CreateLessonView.ListBox.Items.GetItemAt(0));
            CreateLessonView.ListBox.SelectedItems.Add(CreateLessonView.ListBox.Items.GetItemAt(1));
        }
    }
}
