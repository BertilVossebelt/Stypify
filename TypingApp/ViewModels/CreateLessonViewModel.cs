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
        private ObservableCollection<Group>? _groups;
        private int _amountOfExercises;
        private string _name;
        public bool LessonAlreadyExists { get; set; }
        public ICommand SaveLessonCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public int test = 0;
        public UserStore UserStore
        {
            get { return _userStore; }
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
                OnPropertyChanged();
            }
        }
        public int AmountOfExercises
        {
            get => _amountOfExercises;
            set
            {
                _amountOfExercises = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Group>? Groups
        {
            get => _groups;
            set
            {
                _groups = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        // IN exercise model een bool toevoegen en dan foreach exercise en kijken of hij true of false is.
        // Kijken hoe je Create if not exist kan doen met een value in mssql, anders moet je eerst het verwijderen en dan aanmaken, of select, update/aanmaken

        public CreateLessonViewModel(UserStore userStore, NavigationService MylessonsService)
        {
            _userStore = userStore;
            Exercises = new ObservableCollection<Exercise>();
            createLessonViewModel = this;
            SaveLessonCommand = new SaveLessonCommand(MylessonsService);
            CancelCommand = new CancelCommand(MylessonsService);

            if (userStore != null)
            {

            }

           
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
            AmountOfExercises = new LessonProvider().GetAmountOfExercisesFromTeacher(userStore.Teacher.Id);
            var groups = new LessonProvider().GetGroups(userStore.Teacher.Id);
            Groups = new ObservableCollection<Group>();
            groups?.ForEach(g => Groups?.Add(new Group( (int)g["id"], (string)g["name"],(string)g["code"] )));

            var exercises = new ExerciseProvider().GetAll(userStore.Teacher.Id);

            if (exercises != null)
            {
                foreach (var exercise in exercises)
                {
                    if(exercise != null)
                    {
                        Exercise ex = new Exercise((string)exercise["text"], (string)exercise["name"], (int)exercise["id"]);
                        Exercises?.Add(ex);
                    }
                }
            }
            Console.WriteLine(CreateLessonView.ExerciseListBox);

            //CreateLessonView.ListBox.SelectedItems.Add(CreateLessonView.ListBox.Items.GetItemAt(0));
            //exercises?.ForEach(e => Exercises?.Add(new Exercise((string)e["text"], (string)e["name"])));


        }
        public ObservableCollection<Exercise> GetExercises(int id)
        {
            var exercises = new ExerciseProvider().GetAll(id);

            ObservableCollection<Exercise> Exercises = new ObservableCollection<Exercise>();
            exercises?.ForEach(e => Exercises?.Add(new Exercise((string)e["text"], (string)e["name"], (int)e["id"])));
            return Exercises;
        }
        public void SelectItems() //ObservableCollection<Exercise> ExerciseList
        {
            if (this._userStore.LessonId != null)
            {
                ObservableCollection<Exercise> ToBeSelectedExercises = new ObservableCollection<Exercise>();
                ObservableCollection<Group> ToBeSelectedGroups = new ObservableCollection<Group>();

                var name = new LessonProvider().GetById((int)this._userStore.LessonId);
                Name = (string)name["name"];

                var exercises = new LessonProvider().GetAllExercisesByLessonId((int)this._userStore.LessonId);
                exercises?.ForEach(e => ToBeSelectedExercises?.Add(new Exercise((string)e["text"], (string)e["name"], (int)e["id"])));

                foreach (Exercise exercise in ToBeSelectedExercises)
                {
                    foreach (Exercise ListboxItem in CreateLessonView.ExerciseListBox.Items)
                    {
                        if (exercise.Id == ListboxItem.Id)
                        {
                            CreateLessonView.ExerciseListBox.SelectedItems.Add(ListboxItem);
                        }
                    }
                }

                var groups =  new LessonProvider().GetAllGroupsByLessonId((int)this._userStore.LessonId);
                groups?.ForEach(e => ToBeSelectedGroups?.Add(new Group((int)e["id"], (string)e["name"], (string)e["code"])));

                foreach (Group group in ToBeSelectedGroups)
                {
                    foreach (Group ListboxItem in CreateLessonView.GroupListbox.Items)
                    {
                        if (group.GroupId == ListboxItem.GroupId)
                        {
                            CreateLessonView.GroupListbox.SelectedItems.Add(ListboxItem);
                        }
                    }
                }
            }
        }
    }
}
