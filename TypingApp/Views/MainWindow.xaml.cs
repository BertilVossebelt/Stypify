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
        private ExerciseStore _exerciseStore;
        private User _user;
        private int _completedExercises;
        
        private int _currentIndex { get; set; }

        public MainWindow(ExerciseStore exerciseStore, User user)
        {
            _completedExercises = 0;
            _exerciseStore = exerciseStore;
            _currentIndex = 0;
            _user = user;
            InitializeComponent();
        }

        private void SetEventListeners(object sender, RoutedEventArgs e)
        {
            var window = GetWindow(this);
            if (window != null) window.TextInput += HandleTextInput;
            _exerciseStore.ExerciseCreated += (List<Character> obj) => _currentIndex = 0;
        }
        
        private void HandleTextInput(object sender, TextCompositionEventArgs e)
        {
            var keyChar = (char)System.Text.Encoding.ASCII.GetBytes(e.Text)[0];
            var textAsCharList = _exerciseStore.TextAsCharList;
            var charData = textAsCharList[_currentIndex];
            
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
                var generateExerciseCommand = new GenerateExerciseCommand(_exerciseStore, _user.Characters);
                generateExerciseCommand.Execute(this);
                _completedExercises++;
                _currentIndex = 0;
            }
        }
    }
}