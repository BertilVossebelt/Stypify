using System.Text.RegularExpressions;
using System.Windows;
using System;
using TypingApp.Models;
using TypingApp.Stores;
using TypingApp.ViewModels;
using Group = TypingApp.Models.Group;

namespace TypingApp.Commands
{
    internal class LinkToGroupSaveCommand : CommandBase
    {
        private readonly User _user;
        private readonly NavigationStore _navigationStore;
        private readonly DatabaseConnection _connection;
        private readonly Group _code;
        private string _groupId;
        public LinkToGroupSaveCommand(Group code, User user, NavigationStore navigationStore, DatabaseConnection connection)
        {
            _code = code;
            _user = user;
            _navigationStore = navigationStore;
            _connection = connection;
        }
        public override void Execute(object? parameter)
        {
            string messageBoxText2 = "Weet je zeker dat je  aan deze groep wilt koppelen";
            string caption2 = "Koppelen";
            MessageBoxButton button2 = MessageBoxButton.YesNo;
            MessageBoxImage icon2 = MessageBoxImage.Question;
            MessageBoxResult result2;

            result2 = MessageBox.Show(messageBoxText2, caption2, button2, icon2);
            if (result2 == MessageBoxResult.Yes)
            {

                //Save here to database
                String QueryString = $"SELECT id FROM Groups WHERE code='{_code.GroupCode}'";
                var reader = _connection.ExecuteSqlStatement(QueryString);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        _groupId = reader["id"].ToString();
                    }
                    reader.Close();

                    String QueryString2 = $"SELECT id FROM Group_Student WHERE group_id='{_groupId}' AND student_id='{_user.Id}';";
                    reader = _connection.ExecuteSqlStatement(QueryString2);
                    if (reader.HasRows == false)
                    {
                        String QueryString3 = $"INSERT INTO Group_Student (group_id,student_id) VALUES ('{_groupId}','{_user.Id}')";
                        _connection.ExecuteSqlStatement2(QueryString3);
                        _navigationStore.CurrentViewModel = new StudentDashboardViewModel(_user, _navigationStore, _connection);
                    }
                    else
                    {
                        string messageBoxText3 = "Je bent al gekoppeld aan deze groep";
                        string caption3 = "Fout";
                        MessageBoxButton button3 = MessageBoxButton.OK;
                        MessageBoxImage icon3 = MessageBoxImage.Error;
                        MessageBoxResult result3;
                        result3 = MessageBox.Show(messageBoxText3, caption3, button3, icon3);
                    }
                }
                else
                {
                    string messageBoxText4 = "Deze group code bestaat niet";
                    string caption4 = "Fout";
                    MessageBoxButton button4 = MessageBoxButton.OK;
                    MessageBoxImage icon4 = MessageBoxImage.Error;
                    MessageBoxResult result4;
                    result4 = MessageBox.Show(messageBoxText4, caption4, button4, icon4);
                }
            }
        }
    }
}



