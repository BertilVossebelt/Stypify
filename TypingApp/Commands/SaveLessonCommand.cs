using System.Collections.ObjectModel;
using System.Windows;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Services.DatabaseProviders;
using TypingApp.ViewModels;
using TypingApp.Views;

namespace TypingApp.Commands
{
    public class SaveLessonCommand : CommandBase
    {
        public CreateLessonViewModel createLessonViewModel { get; set; }
        public ObservableCollection<Exercise>? SelectedExercises { get; set; }
        public ObservableCollection<Group>? SelectedGroups { get; set; }
        private readonly NavigationService _teacherMyLessonsNavigationService;
        public string Name { get; set; }
        
        public SaveLessonCommand(NavigationService TeacherMyLessonsNavigationService)
        {
            _teacherMyLessonsNavigationService = TeacherMyLessonsNavigationService;
        }
        
        /*
         * This method is used to save a lesson to the database.
         * -----------------------------------------------------
         * Method should only be used for teachers.
         */
        public override void Execute(object? parameter)
        {
            var lessonProvider = new LessonProvider();
            lessonProvider.DeleteLinksToGroups(73);
            lessonProvider.DeleteLinksToExercises(73);
            createLessonViewModel = CreateLessonViewModel.createLessonViewModel;
            if (CreateLessonView.ExerciseListBox.SelectedItems.Count > 0 && CreateLessonView.GroupListbox.SelectedItems.Count > 0)
            {
                var saveMessageBox = MessageBox.Show("Weet je zeker dat je deze les wilt aanmaken/updaten?", "Confirmatie", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (saveMessageBox != MessageBoxResult.Yes) return;

                Name = createLessonViewModel.Name;
                //SelectedExercises = CreateLessonView.ExerciseListBox.SelectedItems;
                if (createLessonViewModel.UserStore.LessonId == null)
                {
                    var lesson = lessonProvider.Create(Name, createLessonViewModel.UserStore.Teacher.Id);

                    foreach (Exercise exercise in CreateLessonView.ExerciseListBox.SelectedItems)
                    {
                        new ExerciseProvider().LinkToLesson((int)lesson?["id"], exercise.Id);
                    }
                    foreach (Group group in CreateLessonView.GroupListbox.SelectedItems)
                    {
                        lessonProvider.LinkToGroup(group.GroupId, (int)lesson["id"]);
                    }
                }
                else
                {
                    lessonProvider.DeleteLinksToGroups(73);
                    lessonProvider.DeleteLinksToExercises(73);
                    lessonProvider.UpdateLesson((int)createLessonViewModel.UserStore.LessonId, Name, createLessonViewModel.UserStore.Teacher.Id);

                    foreach (Exercise exercise in CreateLessonView.ExerciseListBox.SelectedItems)
                    {
                        new ExerciseProvider().LinkToLesson((int)createLessonViewModel.UserStore.LessonId, exercise.Id);
                    }
                    foreach (Group group in CreateLessonView.GroupListbox.SelectedItems)
                    {
                        lessonProvider.LinkToGroup(group.GroupId, (int)createLessonViewModel.UserStore.LessonId);
                    }
                }
                var savedMessagebox = MessageBox.Show("De les is opgeslagen", "Opgeslagen", MessageBoxButton.OK);

                var navigateCommand = new NavigateCommand(_teacherMyLessonsNavigationService);
                navigateCommand.Execute(this);
            }
            else
            {
                var errorMessageBox = MessageBox.Show("Een les moet minimaal 1 groep, 1 les en een naam hebben", "Error", MessageBoxButton.OK);
            }
        }
    }
}
