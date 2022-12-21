using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TypingApp.Commands;
using TypingApp.Services;

namespace TypingApp.ViewModels;

public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public virtual void RaisePropertyChanged(string propertyName)
    {
        OnPropertyChanged(propertyName);
    }
    
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}