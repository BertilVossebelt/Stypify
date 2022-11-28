using System;
using TypingApp.Stores;
using TypingApp.ViewModels;

namespace TypingApp.Services;

public class NavigationService
{
    private readonly NavigationStore _navigationStore;
    private readonly Func<ViewModelBase> _createViewModel;

    public NavigationService(NavigationStore navigationStore, Func<ViewModelBase> createViewModel)
    {
        _createViewModel = createViewModel;
        _navigationStore = navigationStore;
    }

    public void Navigate()
    {
        _navigationStore.CurrentViewModel = _createViewModel();
    }
}