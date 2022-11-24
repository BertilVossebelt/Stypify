using System;
using System.Windows;
using System.Windows.Input;
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
        private int _currentIndex { get; set; }

        public MainWindow(NavigationStore currentViewModel)
        {
            _navigationStore = currentViewModel;
            _currentIndex = 0;
            InitializeComponent();
        }
    }
}