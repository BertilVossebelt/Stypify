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
        public ICommand BackButton { get; set; }

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
       
        public CreateLessonViewModel(UserStore userStore, NavigationService MylessonsService)
        {
            _userStore = userStore;
            Exercises = new ObservableCollection<Exercise>();
            createLessonViewModel = this;
            SaveLessonCommand = new SaveLessonCommand(MylessonsService);
            CancelCommand = new CancelCommand(MylessonsService);
            BackButton = new CancelCommand(MylessonsService);

            Groups = new ObservableCollection<Group>();

            AmountOfExercises = new LessonProvider().GetAmountOfExercisesFromTeacher(userStore.Teacher.Id);

            var groups = new LessonProvider().GetGroups(userStore.Teacher.Id);
            groups?.ForEach(g => Groups?.Add(new Group( (int)g["id"], (string)g["name"],(string)g["code"] )));

            var exercises = new ExerciseProvider().GetAll(userStore.Teacher.Id);
            exercises?.ForEach(e => Exercises?.Add(new Exercise((string)e["text"], (string)e["name"], (int)e["id"])));


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
