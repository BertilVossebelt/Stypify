using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public override void Execute(object? parameter)
        {
            var test = new LessonProvider();
            test.DeleteLesson(73);
            createLessonViewModel = CreateLessonViewModel.createLessonViewModel;
            if (CreateLessonView.ExerciseListBox.SelectedItems.Count > 0 && CreateLessonView.GroupListbox.SelectedItems.Count > 0)
            {
                var saveMessageBox = MessageBox.Show("Weet je zeker dat je deze les wilt aanmaken/updaten?", "Confirmatie", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (saveMessageBox != MessageBoxResult.Yes) return;

                Name = createLessonViewModel.Name;
                //SelectedExercises = CreateLessonView.ExerciseListBox.SelectedItems;
                if (createLessonViewModel.UserStore.LessonId == null)
                {
                    var lessonProvider = new LessonProvider();
                    var lesson = lessonProvider.CreateLesson(Name, createLessonViewModel.UserStore.Teacher.Id);

                    foreach (Exercise exercise in CreateLessonView.ExerciseListBox.SelectedItems)
                    {
                        lessonProvider.CreateLessonExerciseLink((int)lesson["id"], exercise.Id);
                    }
                    foreach (Group group in CreateLessonView.GroupListbox.SelectedItems)
                    {
                        lessonProvider.CreateGroupLessonLink(group.GroupId, (int)lesson["id"]);
                    }
                    
                }
                else
                {
                    var lessonProvider = new LessonProvider();
                    lessonProvider.DeleteLesson((int)createLessonViewModel.UserStore.LessonId);
                    lessonProvider.UpdateLesson((int)createLessonViewModel.UserStore.LessonId, Name, createLessonViewModel.UserStore.Teacher.Id);

                    foreach (Exercise exercise in CreateLessonView.ExerciseListBox.SelectedItems)
                    {
                        lessonProvider.CreateLessonExerciseLink((int)createLessonViewModel.UserStore.LessonId, exercise.Id);
                    }
                    foreach (Group group in CreateLessonView.GroupListbox.SelectedItems)
                    {
                        lessonProvider.CreateGroupLessonLink(group.GroupId, (int)createLessonViewModel.UserStore.LessonId);
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
