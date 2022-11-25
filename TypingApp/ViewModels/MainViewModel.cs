using Microsoft.Extensions.DependencyInjection;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Stores;

namespace TypingApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly NavigationStore _navigationStore;
    private readonly DatabaseConnection _connection;
    public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
    public MainViewModel(NavigationStore navigationStore,DatabaseConnection connection)
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