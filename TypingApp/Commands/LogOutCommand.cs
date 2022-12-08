using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Stores;

namespace TypingApp.Commands
{
    public class LogOutCommand : CommandBase
    {
        private UserStore _userStore;
        private NavigationService _loginNavigationService;
        public LogOutCommand( UserStore userStore, NavigationService loginNavigationService)
        {
            _userStore = userStore;
            _loginNavigationService = loginNavigationService;
        }

        public override void Execute(object? parameter)
        {
            _userStore.DeleteTeacher();
            _userStore.DeleteStudent();
            _userStore.DeleteAdmin();
            var navigateCommand = new NavigateCommand(_loginNavigationService);
            navigateCommand.Execute(this);
        }
    }
}
