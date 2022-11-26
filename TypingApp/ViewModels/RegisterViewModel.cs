using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Stores;

namespace TypingApp.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public ICommand GoToLoginButton { get; }
        public ICommand RegisterCommand { get; }

        public RegisterViewModel(NavigationStore navigationStore, DatabaseConnection connection) 
        {
                _connection = connection;
                GoToLoginButton = new GoToLoginCommand(navigationStore, _connection);
        }
    }
}
