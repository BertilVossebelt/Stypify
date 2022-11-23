using Microsoft.Extensions.DependencyInjection;
using TypingApp.Models;
using TypingApp.Stores;
using TypingApp.Commands;
using System;

namespace TypingApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly NavigationStore _navigationStore;
    public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
    public MainViewModel(NavigationStore navigationStore, DatabaseConnection connection)
    {
        _connection = connection;
        _navigationStore = navigationStore;
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}