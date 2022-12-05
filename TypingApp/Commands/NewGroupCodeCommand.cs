using TypingApp.Models;
using TypingApp.ViewModels;

namespace TypingApp.Commands
{
    internal class NewGroupCodeCommand : CommandBase
    {
        private Group _group;
        private AddGroupViewModel _addGroupViewModel;
        public NewGroupCodeCommand(Group newGroup,AddGroupViewModel addGroupViewModel)
        {
            _group = newGroup;
            _addGroupViewModel = addGroupViewModel;
        }

        public override void Execute(object? parameter)
        {
            _group.GroupCodeGeneratorMethod();
            _addGroupViewModel.GroupCodeText = _group.GroupCode;
        }
    }

}

