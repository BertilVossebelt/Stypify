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
            var lessonProvider = new LessonProvider();    
            createLessonViewModel = CreateLessonViewModel.createLessonViewModel;

            //checks if there are at least 1 group and exercise selected.
            if (CreateLessonView.ExerciseListBox.SelectedItems.Count > 0 && CreateLessonView.GroupListbox.SelectedItems.Count > 0 && createLessonViewModel.Name != null)
            {
                //Gives popup to confirm the decision to save the lesson
                var saveMessageBox = MessageBox.Show("Weet je zeker dat je deze les wilt aanmaken/updaten?", "Confirmatie", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (saveMessageBox != MessageBoxResult.Yes) return;

                //gets name from viewmodel
                Name = createLessonViewModel.Name;
                //SelectedExercises = CreateLessonView.ExerciseListBox.SelectedItems;

                //checks if there was a lesson selected
                if (createLessonViewModel.LessonStore.CurrentLesson == null) 
                {
                    //Create lesson and returns lesson (mainly needed for the Id)
                    var lesson = lessonProvider.Create(Name, createLessonViewModel.UserStore.Teacher.Id);

                    //Create Lesson_Exercises Links
                    foreach (Exercise exercise in CreateLessonView.ExerciseListBox.SelectedItems)
                    {
                        new ExerciseProvider().LinkToLesson((int)lesson?["id"], exercise.Id);
                    }
                    //Create Group_Lesson Links
                    foreach (Group group in CreateLessonView.GroupListbox.SelectedItems)
                    {
                        lessonProvider.LinkToGroup(group.GroupId, (int)lesson["id"]);
                    }
                }
                else //runs if the user needs a new lesson
                {
                    //Deletes all Links to lesson
                    lessonProvider.DeleteLinksToGroups(createLessonViewModel.LessonStore.CurrentLesson.Id);
                    lessonProvider.DeleteLinksToExercises(createLessonViewModel.LessonStore.CurrentLesson.Id);

                    //Update Lesson
                    lessonProvider.UpdateLesson(createLessonViewModel.LessonStore.CurrentLesson.Id, Name, createLessonViewModel.UserStore.Teacher.Id);

                    //Create new Exercise_Lesson links
                    foreach (Exercise exercise in CreateLessonView.ExerciseListBox.SelectedItems)
                    {
                        new ExerciseProvider().LinkToLesson(createLessonViewModel.LessonStore.CurrentLesson.Id, exercise.Id);
                    }
                    //Create new Group_Lesson links
                    foreach (Group group in CreateLessonView.GroupListbox.SelectedItems)
                    {
                        lessonProvider.LinkToGroup(group.GroupId, createLessonViewModel.LessonStore.CurrentLesson.Id);
                    }
                }
                //popup when the data is saved in database
                var savedMessagebox = MessageBox.Show("De les is opgeslagen", "Opgeslagen", MessageBoxButton.OK);

                //navigates back to the MyLessons window
                var navigateCommand = new NavigateCommand(_teacherMyLessonsNavigationService);
                navigateCommand.Execute(this);
                
            }
            else
            {
                //popup if there were no groups, no exercises or no name
                var errorMessageBox = MessageBox.Show("Een les moet minimaal 1 groep, 1 oefening en een naam hebben", "Error", MessageBoxButton.OK);
            }
        }
    }
}
