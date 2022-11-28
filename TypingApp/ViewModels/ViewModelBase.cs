using System.ComponentModel;
using System.Runtime.CompilerServices;
using TypingApp.Commands;

namespace TypingApp.ViewModels;

public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected DatabaseConnection _connection { get; set; }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}