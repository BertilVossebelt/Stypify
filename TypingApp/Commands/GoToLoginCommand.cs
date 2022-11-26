using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypingApp.Stores;
using TypingApp.ViewModels;

namespace TypingApp.Commands
{
    public class GoToLoginCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly DatabaseConnection _connection;
        public GoToLoginCommand(NavigationStore navigationStore, DatabaseConnection connection)
        {
            _navigationStore = navigationStore;
            _connection = connection;
        }

        public override void Execute(object? parameter)
        {
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore, _connection);
        }
    }
}
