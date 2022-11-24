using System.Windows;
using TypingApp.Models;
using TypingApp.Stores;
using TypingApp.ViewModels;

namespace TypingApp.Commands;

public class SaveGroupCommand : CommandBase
{
    private readonly User _user;
    private readonly NavigationStore _navigationStore;
    private Group _group;
    public SaveGroupCommand(Group newGroup, NavigationStore navigationStore, User user)
    {
        _group = newGroup;
        _user = user;
        _navigationStore = navigationStore;
    }

    public override void Execute(object? parameter)
    {
        if(_group.GroupName == "" || _group.GroupName == null)
        {
            string messageBoxText1 = "Je moet een naam invullen";
            string caption1 = "Geen naam";
            MessageBoxButton button1 = MessageBoxButton.OK;
            MessageBoxImage icon1 = MessageBoxImage.Error;
            MessageBoxResult result1;

            result1 = MessageBox.Show(messageBoxText1, caption1, button1, icon1);
        }
        else
        {
            string messageBoxText2 = "Weet je zeker dat je deze groep wilt opslaan";
            string caption2 = "Opslaan";
            MessageBoxButton button2 = MessageBoxButton.YesNo;
            MessageBoxImage icon2 = MessageBoxImage.Question;
            MessageBoxResult result2;

            result2 = MessageBox.Show(messageBoxText2, caption2, button2, icon2);
            if (result2 == MessageBoxResult.Yes)
            {
                _navigationStore.CurrentViewModel = new TeacherDashboardViewModel(_user, _navigationStore);
                System.Console.WriteLine(_group.GroupName);
                System.Console.WriteLine(_group.GroupCode);
                //Save here to database

                //
            };
        }

        
    }
}