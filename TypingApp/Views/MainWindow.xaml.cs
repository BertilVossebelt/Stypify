﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Stores;

namespace TypingApp.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private ExerciseStore _exerciseStore;
    private UserStore _userStore;
    private int _completedExercises;

    private int _currentIndex { get; set; }

    public MainWindow(ExerciseStore exerciseStore, UserStore userStore)
    {
        _completedExercises = 0;
        _exerciseStore = exerciseStore;
        _currentIndex = 0;
        _userStore = userStore;
        InitializeComponent();
    }

    private void SetEventListeners(object sender, RoutedEventArgs e)
    {
        var window = GetWindow(this);
        if (window != null)
        {
            window.TextInput -= HandleTextInput;
            window.TextInput += HandleTextInput;
        }

        _exerciseStore.ExerciseCreated += (List<Character> obj) => _currentIndex = 0;
        
        if (_userStore.Student == null) return;
        var studentStatistics = new StudentProvider().GetStudentStatistics(_userStore.Student.Id);
        if (studentStatistics == null) new StudentProvider().CreateStudentStatistics(_userStore.Student.Id);
        
        _completedExercises = (int)(studentStatistics?["completed_exercises"] ?? 0);
    }

    private void HandleTextInput(object sender, TextCompositionEventArgs e)
    {
        if (_userStore.Student?.Characters == null) return;
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
            _completedExercises++;
            new StudentProvider().UpdateStudentStatistics(_userStore.Student.Id, _completedExercises);

            new GenerateExerciseCommand(_exerciseStore, _userStore.Student.Characters).Execute(this);
            _currentIndex = 0;
        }
    }
}