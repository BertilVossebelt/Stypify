using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TypingApp.ViewModels;

namespace TypingApp.Views;

public partial class ExerciseView : UserControl
{
    private int _currentIndex;

    public ExerciseView()
    {
        InitializeComponent();
        
    }
}