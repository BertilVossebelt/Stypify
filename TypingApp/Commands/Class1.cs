using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypingApp.Models;
using TypingApp.ViewModels;
using TypingApp.Views;


namespace TypingApp.Commands
{
    public class Class1 : CommandBase
    {
        private ObservableCollection<Exercise>? _selectedExercises;
        public CreateLessonViewModel test;
        public Class1(CreateLessonViewModel tes)
        {
            _selectedExercises = tes.SelectedExercises;
            test = tes;

        }
        public override void Execute(object? parameter)
        {
            //CreateLessonView.ListBox.SelectedItems.Add(CreateLessonView.ListBox.Items.GetItemAt(CreateLessonView.ListBox.Items.IndexOf(EXERCISE)));
            foreach (Exercise ex in CreateLessonView.ExerciseListBox.SelectedItems)
            {
                Console.WriteLine(ex.Name);
 
            }
        }
    }
}
