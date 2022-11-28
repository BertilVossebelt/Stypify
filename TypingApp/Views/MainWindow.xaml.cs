using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Stores;
using TypingApp.ViewModels;

namespace TypingApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NavigationStore _navigationStore;
        private ExerciseStore _exerciseStore;
        private User _user;
        private int _completedExercises;
        
        private int _currentIndex { get; set; }

        public MainWindow(NavigationStore currentViewModel, ExerciseStore exerciseStore, User user)
        {
            _completedExercises = 0;
            _navigationStore = currentViewModel;
            _exerciseStore = exerciseStore;
            _currentIndex = 0;
            _user = user;
            InitializeComponent();
        }

        private void TextInputListener(object sender, RoutedEventArgs e)
        {
            var window = GetWindow(this);
            if (window != null) window.TextInput += HandleTextInput;
        }

        public void HandleTextInput(object sender, TextCompositionEventArgs e)
        {
            var keyChar = (char)System.Text.Encoding.ASCII.GetBytes(e.Text)[0];
            var textAsCharList = _exerciseStore.TextAsCharList;

            var charData = textAsCharList[_currentIndex];

            Console.WriteLine(textAsCharList.Count + " " + _currentIndex + " = " +
                              (textAsCharList.Count == _currentIndex));

            if (charData.Char == keyChar)
            {
                charData.Correct = true;
                _exerciseStore.UpdateExercise(textAsCharList);
                _currentIndex++;
            }
            else
            {
                charData.Wrong = true;
                _exerciseStore.UpdateExercise(textAsCharList);
            }

            if (textAsCharList.Count == _currentIndex)
            {
                Console.WriteLine("New exercise");
                var generateExerciseCommand = new GenerateExerciseCommand(_exerciseStore, _user.Characters);
                generateExerciseCommand.Execute(this);
                _completedExercises++;
                _currentIndex = 0;
            }
        }
    }
}