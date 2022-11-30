using System.Windows;
using TypingApp.Models;
using TypingApp.Services;
using Group = TypingApp.Models.Group;

namespace TypingApp.Commands
{
    internal class LinkToGroupSaveCommand : CommandBase
    {
        private readonly User _user;
        private readonly DatabaseConnection _connection;
        private readonly Group _code;
        private int _groupId;
        private NavigationService _studentDashboardNavigationService;

        public LinkToGroupSaveCommand(Group code, User user, DatabaseConnection connection, NavigationService studentDashboardNavigationService)
        {
            _code = code;
            _user = user;
            _connection = connection;
            _studentDashboardNavigationService = studentDashboardNavigationService;
        }

        public override void Execute(object? parameter)
        {
            var LinkGroupMessageBox = MessageBox.Show("Weet je zeker dat je  aan deze groep wilt koppelen", "Koppelen", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (LinkGroupMessageBox == MessageBoxResult.Yes)
            {
                //Save here to database
                var QueryString = $"SELECT id FROM Groups WHERE code='{_code.GroupCode}'";
                var reader = _connection.ExecuteSqlStatement(QueryString);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        _groupId = (int)reader["id"];
                    }
                    reader.Close();

                    var QueryString2 = $"SELECT id FROM Group_Student WHERE group_id='{_groupId}' AND student_id='{_user.Id}';";
                    reader = _connection.ExecuteSqlStatement(QueryString2);

                    if (reader.HasRows == false)
                    {
                        reader.Close();
                        var QueryString3 = $"INSERT INTO Group_Student (group_id,student_id) VALUES ('{_groupId}','{_user.Id}')";
                        _connection.ExecuteSqlStatement2(QueryString3);

                        var navigateCommand = new NavigateCommand(_studentDashboardNavigationService);
                        navigateCommand.Execute(this);
                        reader.Close();
                    }
                    else
                    {
                        reader.Close();
                        MessageBox.Show("Je bent al gekoppeld aan deze groep", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    reader.Close();
                    MessageBox.Show("Deze group code bestaat niet", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}



